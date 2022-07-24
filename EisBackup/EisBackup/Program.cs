using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace EisBackup
{
    internal static class Program
    {
        public static string ConnectionString = String.Format("server={0};user={1};pwd={2};database={3};",
                                                                Properties.Settings.Default.Server,
                                                                Properties.Settings.Default.Login,
                                                                Properties.Settings.Default.Password.DecryptString(),
                                                                Properties.Settings.Default.Database);

        public static string DefaultFolder = @"C:\Distr\EIS-Backup";
        public static string TargetFile = "Не выбран";
        public static string SilentFilePath = "Не выбран";
        public static bool SilentExport = false;
        public static bool Inactive = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AuthorizationForm(args));
        }
    }

    #region Соленье / приправы

    public static class StringSecurityHelper
    {
        private static readonly byte[] entropy = Encoding.Unicode.GetBytes("1AQ'&mc %;lq@31*@!)@asd@.,,z2389");

        public static string DecryptString(this string encryptedData)
        {
            if (encryptedData == null)
            {
                return null;
            }

            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, DataProtectionScope.CurrentUser);

                return Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return null;
            }
        }

        public static string EncryptString(this string input)
        {
            if (input == null)
            {
                return null;
            }

            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(input), entropy, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedData);
        }
    }

    #endregion Соленье / приправы
}