using System;
using System.Windows.Forms;

namespace EisBackup
{
    public partial class SettingsControl : UserControl
    {
        public event EventHandler ButtonClick;

        public SettingsControl()
        {
            InitializeComponent();
            tbPassword.Text = Properties.Settings.Default.Password.DecryptString();
        }

        private void BtSaveSet_Click(object sender, EventArgs e)
        {
            //Properties.Settings.Default.Login = tbLogin.Text;
            Properties.Settings.Default.Database = tbDatabase.Text;
            Properties.Settings.Default.Server = tbServer.Text;

            //string TempPass = tbPassword.Text.EncryptString();
            //if (Properties.Settings.Default.Password != TempPass)
            //    Properties.Settings.Default.Password = TempPass;
            //TempPass = "";

            Properties.Settings.Default.Save();

            this.ButtonClick?.Invoke(this, e);
        }
    }
}