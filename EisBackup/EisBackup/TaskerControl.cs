using System;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace EisBackup
{
    public partial class TaskerControl : UserControl
    {
        public TaskerControl()
        {
            InitializeComponent();
            GetTaskStatus();
        }

        private const string taskName = "Резервирование баз данных ЕИС Енот";

        public void GetTaskStatus()
        {
            lblStatus.Text = "Резервное копирование не запланировано.";
            Task t = TaskService.Instance.FindTask(taskName);
            if (t != null)
                if (t.Enabled)
                    lblStatus.Text = "Резервное копирование запланировано!";
        }

        private void BtnDeleteSet_Click(object sender, EventArgs e)
        {
            Task t = TaskService.Instance.FindTask(taskName);
            if (t != null) t.Enabled = false;
            GetTaskStatus();
        }

        private void BtSaveSet_Click(object sender, EventArgs e)
        {
            // Куда сохраняем
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    Program.TargetFile = fbd.SelectedPath;
                else return;
            }

            // Планировщик заданий
            TriggerEditDialog dlg = new TriggerEditDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel) return;

            Task t = TaskService.Instance.AddTask(taskName, dlg.Trigger,
            new ExecAction(System.Environment.CurrentDirectory + "\\EisBackup.exe", "/s " + Program.TargetFile, System.Environment.CurrentDirectory));
            t.Definition.Principal.LogonType = TaskLogonType.InteractiveToken;
            GetTaskStatus();
        }
    }
}