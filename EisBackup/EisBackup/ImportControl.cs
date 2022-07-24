using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace EisBackup
{
    public partial class ImportControl : UserControl
    {
        public CurrentFile sourceObject = new CurrentFile();
        private readonly int nmImIntervalValue = 1000;
        private BackgroundWorker bwImport;
        private bool cancel = false;
        private MySqlCommand cmd;
        private MySqlConnection conn;
        private int curBytes;
        private MySqlBackup mb;
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private Timer timer1;
        private int totalBytes;

        public event EventHandler ButtonClick;

        public ImportControl()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(RowsDataExportMode));
            dt.Columns.Add("name");
            foreach (RowsDataExportMode mode in Enum.GetValues(typeof(RowsDataExportMode)))
            {
                dt.Rows.Add(mode, mode.ToString());
            }

            mb = new MySqlBackup();
            mb.ImportProgressChanged += mb_ImportProgressChanged;

            timer1 = new Timer();
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;

            bwImport = new BackgroundWorker();
            bwImport.DoWork += bwImport_DoWork;
            bwImport.RunWorkerCompleted += bwImport_RunWorkerCompleted;

            lb2import.DataBindings.Add("Text", sourceObject, "MyCurrentFile", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }

        private void btImport_Click(object sender, EventArgs e)
        {
            Program.Inactive = true;
            this.ButtonClick?.Invoke(this, e);
            openFileDialog1.Title = "Выберите файл резервной копии";
            openFileDialog1.Filter = "Файл базы данных|*.sql";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                Program.Inactive = false;
                this.ButtonClick?.Invoke(this, e);
                return;
            }
            sourceObject.MyCurrentFile = openFileDialog1.FileName;

            //if (!Program.SourceFileExists())
            //    return;

            progressBar1.Value = 0;
            lbStatus.Text = "0 из 0 байт";
            this.Refresh();

            cancel = false;
            curBytes = 0;
            totalBytes = 0;

            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }

            try
            {
                conn = new MySqlConnection(Program.ConnectionString);
                cmd = new MySqlCommand();
                cmd.Connection = conn;
                conn.Open();
                string sql = "set global net_buffer_length=1000000; SET GLOBAL max_allowed_packet=1073741824;";
                MySqlCommand cmd_my = new MySqlCommand(sql, conn);
                cmd_my.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не удается подключиться к серверу MySQL, проверьте настройки.", "Ошибка");
                return;
            }

            timer1.Start();

            mb.ImportInfo.IntervalForProgressReport = nmImIntervalValue;
            mb.Command = cmd;

            bwImport.RunWorkerAsync();
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                mb.ImportFromFile(Program.TargetFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer1.Stop();

            CloseConnection();

            if (cancel)
            {
                MessageBox.Show("Восстановление отменено.");
            }
            else
            {
                if (mb.LastError == null)
                {
                    progressBar1.Value = progressBar1.Maximum;
                    lbStatus.Text = progressBar1.Value + " из " + progressBar1.Maximum;
                    this.Refresh();

                    MessageBox.Show("Восстановление успешно завершено.");
                }
                else
                    MessageBox.Show("Восстановление завершено с ошибкой." + Environment.NewLine + Environment.NewLine + mb.LastError.ToString());
            }

            progressBar1.Value = 0;
            cancel = false;
            sourceObject.MyCurrentFile = "Не выбран";
            lbStatus.Text = "0 из 0 байт";
            Program.Inactive = false;
            this.ButtonClick?.Invoke(this, e);
        }

        private void CloseConnection()
        {
            if (conn != null)
                conn.Close();

            if (conn != null)
                conn.Dispose();

            if (cmd != null)
                cmd.Dispose();
        }

        private void mb_ImportProgressChanged(object sender, ImportProgressArgs e)
        {
            if (cancel)
                mb.StopAllProcess();

            long mbsize = 1024 * 1024; //* 1024;
            int total_MB = (int)(e.TotalBytes / mbsize);
            int cur_MB = (int)(e.CurrentBytes / mbsize);
            totalBytes = total_MB;
            curBytes = cur_MB;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cancel)
            {
                timer1.Stop();
                return;
            }

            if (totalBytes >= 0)
                progressBar1.Maximum = totalBytes;

            if (curBytes < progressBar1.Maximum)
                progressBar1.Value = curBytes;

            lbStatus.Text = progressBar1.Value + " MB из " + progressBar1.Maximum + " MB";
        }

        public class CurrentFile : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public string MyCurrentFile
            {
                get { return Program.TargetFile; }
                set
                {
                    Program.TargetFile = value;
                    InvokePropertyChanged(new PropertyChangedEventArgs("MyCurrentFile"));
                }
            }

            public void InvokePropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChanged?.Invoke(this, e);
            }
        }
    }
}