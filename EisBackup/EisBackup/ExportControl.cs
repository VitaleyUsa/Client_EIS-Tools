using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace EisBackup
{
    public partial class ExportControl : UserControl
    {
        private int _currentRowIndexInAllTable = 0;
        private int _currentRowIndexInCurrentTable = 0;
        private int _currentTableIndex = 0;
        private string _currentTableName = "";
        private int _totalRowsInAllTables = 0;
        private int _totalRowsInCurrentTable = 0;
        private int _totalTables = 0;
        private BackgroundWorker bwExport;
        private bool cancel = false;
        private MySqlCommand cmd;
        private MySqlConnection conn;
        private RowsDataExportMode exportMode;
        private MySqlBackup mb;
        private Timer timer1;
        private bool InternalError;

        public event EventHandler ButtonClick;

        public ExportControl()
        {
            InitializeComponent();

            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(RowsDataExportMode));
            dt.Columns.Add("Name");
            foreach (RowsDataExportMode mode in Enum.GetValues(typeof(RowsDataExportMode)))
            {
                dt.Rows.Add(mode, mode.ToString());
            }

            mb = new MySqlBackup();
            mb.ExportProgressChanged += mb_ExportProgressChanged;

            timer1 = new Timer();
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;

            bwExport = new BackgroundWorker();
            bwExport.DoWork += bwExport_DoWork;
            bwExport.RunWorkerCompleted += bwExport_RunWorkerCompleted;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }

        public void btExport_Click(object sender, EventArgs e)
        {
            Program.Inactive = true;
            this.ButtonClick?.Invoke(this, e);

            btExport.Enabled = false;
            InternalError = false;

            if (!Program.SilentExport)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        Program.TargetFile = fbd.SelectedPath;
                    else
                    {
                        btExport.Enabled = true;
                        Program.Inactive = false;
                        this.ButtonClick?.Invoke(this, e);
                        return;
                    }
                }
            }
            else Program.TargetFile = Program.SilentFilePath;

            _currentTableName = "";
            _totalRowsInCurrentTable = 0;
            _totalRowsInAllTables = 0;
            _currentRowIndexInCurrentTable = 0;
            _currentRowIndexInAllTable = 0;
            _totalTables = 0;
            _currentTableIndex = 0;
            exportMode = RowsDataExportMode.Insert;
            Program.ConnectionString = String.Format("server={0};user={1};pwd={2};database={3};",
                                                                Properties.Settings.Default.Server,
                                                                Properties.Settings.Default.Login,
                                                                Properties.Settings.Default.Password.DecryptString(),
                                                                Properties.Settings.Default.Database);

            try
            {
                conn = new MySqlConnection(Program.ConnectionString);
                cmd = new MySqlCommand();
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                conn.Open();
                string sql = "set global net_buffer_length=1000000; SET GLOBAL max_allowed_packet=1073741824;";
                MySqlCommand cmd_my = new MySqlCommand(sql, conn);
                cmd_my.ExecuteNonQuery();
            }
            catch
            {
                if (!Program.SilentExport)
                    MessageBox.Show("Не удается подключиться к серверу MySQL, проверьте настройки.", "Ошибка");
                return;
            }

            timer1.Interval = 1000;
            timer1.Start();

            mb.ExportInfo.IntervalForProgressReport = 1000;

            //mb.ExportInfo.GetTotalRowsMode = GetTotalRowsMethod.InformationSchema;
            mb.ExportInfo.GetTotalRowsMode = GetTotalRowsMethod.SelectCount;
            //mb.ExportInfo.GetTotalRowsMode = GetTotalRowsMethod.Skip;

            mb.Command = cmd;

            bwExport.RunWorkerAsync();
        }

        private void bwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                mb.ExportInfo.RowsExportMode = exportMode;
                mb.ExportToFile(Program.TargetFile + @"/eis_backup-" +
                                DateTime.Now.Day.ToString() + "-" +
                                DateTime.Now.Month.ToString() + "-" +
                                DateTime.Now.Year.ToString() + ".sql");
            }
            catch (Exception ex)
            {
                CloseConnection();
                if (!Program.SilentExport)
                    MessageBox.Show(ex.Message);
                InternalError = true;
            }
        }

        private void bwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseConnection();
            timer1.Stop();

            if (Program.SilentExport) Environment.Exit(0);

            if (cancel)
            {
                MessageBox.Show("Экспорт отменен.");
            }
            else
            {
                if (mb.LastError == null)
                {
                    timer1_Tick(null, null);

                    //pbRowInAllTable.Value = pbRowInAllTable.Maximum;
                    //pbRowInCurTable.Value = pbRowInCurTable.Maximum;
                    //pbTable.Value = pbTable.Maximum;

                    //lbRowInCurTable.Text = pbRowInCurTable.Value + " of " + pbRowInCurTable.Maximum;
                    //lbRowInAllTable.Text = pbRowInAllTable.Value + " of " + pbRowInAllTable.Maximum;
                    //lbTableCount.Text = _currentTableIndex + " of " + _totalTables;

                    this.Refresh();
                    if (!InternalError)
                        if (!Program.SilentExport)
                            MessageBox.Show("Экспорт успешно завершен.");
                }
                else
                    if (!Program.SilentExport)
                    MessageBox.Show("Экспорт завершен с ошибками." + Environment.NewLine + Environment.NewLine + mb.LastError.ToString());
            }

            first_start();
            btExport.Enabled = true;
            Program.Inactive = false;
            this.ButtonClick?.Invoke(this, e);
        }

        private void CloseConnection()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }

            if (cmd != null)
                cmd.Dispose();
        }

        private void mb_ExportProgressChanged(object sender, ExportProgressArgs e)
        {
            if (cancel)
            {
                mb.StopAllProcess();
                return;
            }

            _currentRowIndexInAllTable = (int)e.CurrentRowIndexInAllTables;
            _currentRowIndexInCurrentTable = (int)e.CurrentRowIndexInCurrentTable;
            _currentTableIndex = e.CurrentTableIndex;
            _currentTableName = e.CurrentTableName;
            _totalRowsInAllTables = (int)e.TotalRowsInAllTables;
            _totalRowsInCurrentTable = (int)e.TotalRowsInCurrentTable;
            _totalTables = e.TotalTables;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cancel)
            {
                timer1.Stop();
                return;
            }

            pbTable.Maximum = _totalTables;
            if (_currentTableIndex <= pbTable.Maximum)
                pbTable.Value = _currentTableIndex;

            pbRowInCurTable.Maximum = _totalRowsInCurrentTable;
            if (_currentRowIndexInCurrentTable <= pbRowInCurTable.Maximum)
                pbRowInCurTable.Value = _currentRowIndexInCurrentTable;

            pbRowInAllTable.Maximum = _totalRowsInAllTables;
            if (_currentRowIndexInAllTable <= pbRowInAllTable.Maximum)
                pbRowInAllTable.Value = _currentRowIndexInAllTable;

            lbCurrentTableName.Text = _currentTableName;
            lbRowInCurTable.Text = _currentRowIndexInCurrentTable + " из " + _totalRowsInCurrentTable;
            lbRowInAllTable.Text = _currentRowIndexInAllTable + " из " + _totalRowsInAllTables;
            lbTableCount.Text = _currentTableIndex + " из " + _totalTables;
        }

        public void first_start()
        {
            _currentRowIndexInAllTable = 0;
            _currentRowIndexInCurrentTable = 0;
            _currentTableIndex = 0;
            _currentTableName = "";
            _totalRowsInAllTables = 0;
            _totalRowsInCurrentTable = 0;
            _totalTables = 0;
            lbCurrentTableName.Text = "Текущая таблица";
            lbRowInCurTable.Text = "0 из 0";
            lbRowInAllTable.Text = "0 из 0";
            lbTableCount.Text = "0 из 0";
            pbTable.Value = 0;
            pbRowInCurTable.Value = 0;
            pbRowInAllTable.Value = 0;

            cancel = false;
        }
    }
}