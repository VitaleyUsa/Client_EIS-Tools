using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EisBackup
{
    public partial class AuthorizationForm : Form
    {
        public MainForm mainForm;
        public AuthorizationForm(string[] args)
        {
            InitializeComponent();
            mainForm = new MainForm(args);
        }

        private void btEnter_Click(object sender, EventArgs e)
        {
            if (CreateMD5(tbPassword.Text) == "DF64DC2EB4A0B85091DD31EB4923EAAC")
            {
                this.Hide();
                mainForm.Closed += (s, args) => this.Close();
                mainForm.Show();
            }
            else
            {
                this.label1.Text = "Неверный пароль!";
            }
            
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
