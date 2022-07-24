using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace EisBackup
{
    public partial class MainForm : Form
    {
        public MainForm(string[] args)
        {

            InitializeComponent();

            string installPath = InstalledApplications.GetApplicationInstallPath("еНот");
            INIManager manager = new INIManager(installPath + "eNot.ini");
            string database = manager.GetPrivateString("DatabaseServer", "Database");
            string server = manager.GetPrivateString("DatabaseServer", "Address");

            // Реализация передачи параметров при фоновом выполнении скрипта
            if (args.Length > 0)
                if (args[0] == "/s")
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.ShowInTaskbar = false;
                    Program.SilentExport = true;
                    Program.SilentFilePath = args[1];
                    exportControl1.btExport_Click(null, null);
                }

            TextBoxWatermarkExtensionMethod.SetWatermark(settingsControl1.tbLogin, "Имя пользователя");
            TextBoxWatermarkExtensionMethod.SetWatermark(settingsControl1.tbPassword, "Пароль");
            TextBoxWatermarkExtensionMethod.SetWatermark(settingsControl1.tbDatabase, "База данных");
            TextBoxWatermarkExtensionMethod.SetWatermark(settingsControl1.tbServer, "Адрес сервера");

            settingsControl1.tbLogin.Text = "super_user";
            settingsControl1.tbPassword.Text = "haha_no_pass_for_u :p";
            
            if (database != null)
                settingsControl1.tbDatabase.Text = database;

            if (server != null)
                settingsControl1.tbServer.Text = server;

            Properties.Settings.Default.Login = "root";
            Properties.Settings.Default.Password = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAFwi8HU7vn0OgXhB+h4LJrQAAAAACAAAAAAAQZgAAAAEAACAAAAA25EbZWxFTlSr8Obq3sxGMunZLoklB2ohUqX8SKW6vaAAAAAAOgAAAAAIAACAAAAA3jRaCTerv0m8d5TmiuYhGBu9gXINgvoDBrdIVk5IOuyAAAADapspvmrLD1d/ZGpzYUD5jvH1bD8CKnutzVhYt9p0LVkAAAAA6YfjTab03l3Vgf7FF0xLi8EfxmFODAEadInbKAfrRNZ66f/vQIyC8hPYB76Ji3T92+1e8Az094vnA7xW+clCD";
            Properties.Settings.Default.Database = settingsControl1.tbDatabase.Text;
            Properties.Settings.Default.Server = settingsControl1.tbServer.Text;

            exportControl1.ButtonClick += new EventHandler(MakeButtonsInactive);
            importControl1.ButtonClick += new EventHandler(MakeButtonsInactive);
            settingsControl1.ButtonClick += new EventHandler(CheckCred);

            if (Properties.Settings.Default.Login == "" ||
                Properties.Settings.Default.Password == "" ||
                Properties.Settings.Default.Database == "" ||
                Properties.Settings.Default.Server == "")
            {
                btExport.Enabled = false;
                btImport.Enabled = false;
                btTasker.Enabled = false;
            }
        }

        private void CheckCred(object sender, EventArgs e)
        {
            if (settingsControl1.tbLogin.Text == "" &&
                settingsControl1.tbPassword.Text == "" &&
                settingsControl1.tbDatabase.Text == " " &&
                settingsControl1.tbServer.Text == "")
            {
                btExport.Enabled = false;
                btImport.Enabled = false;
                btTasker.Enabled = false;
            }
            else
            {
                btExport.Enabled = true;
                btImport.Enabled = true;
                btTasker.Enabled = true;
            }
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            settingsControl1.Hide();
            importControl1.Hide();
            exportControl1.Show();
            taskerControl1.Hide();
            aboutControl1.Hide();

            Program.TargetFile = "";
        }

        private void BtImport_Click(object sender, EventArgs e)
        {
            settingsControl1.Hide();
            importControl1.Show();
            exportControl1.Hide();
            taskerControl1.Hide();
            aboutControl1.Hide();

            importControl1.sourceObject.MyCurrentFile = "Не выбран";
        }

        private void BtSettings_Click(object sender, EventArgs e)
        {
            settingsControl1.Show();
            importControl1.Hide();
            exportControl1.Hide();
            taskerControl1.Hide();
            aboutControl1.Hide();
        }

        private void BtTasker_Click(object sender, EventArgs e)
        {
            settingsControl1.Hide();
            importControl1.Hide();
            exportControl1.Hide();
            taskerControl1.Show();
            aboutControl1.Hide();

            taskerControl1.GetTaskStatus();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //settingsControl1.tbLogin.Text = Properties.Settings.Default.Login;
            //settingsControl1.tbPassword.Text = Properties.Settings.Default.Password.DecryptString();
            settingsControl1.tbDatabase.Text = Properties.Settings.Default.Database;
            settingsControl1.tbServer.Text = Properties.Settings.Default.Server;
        }

        private void MakeButtonsInactive(object sender, EventArgs e)
        {
            if (Program.Inactive)
            {
                btSettings.Enabled = false;
                btExport.Enabled = false;
                btImport.Enabled = false;
                btTasker.Enabled = false;
                btAbout.Enabled = false;
            }
            else
            {
                btSettings.Enabled = true;
                btExport.Enabled = true;
                btImport.Enabled = true;
                btTasker.Enabled = true;
                btAbout.Enabled = true;
            }
        }

        private void BtAbout_Click(object sender, EventArgs e)
        {
            settingsControl1.Hide();
            importControl1.Hide();
            exportControl1.Hide();
            taskerControl1.Hide();
            aboutControl1.Show();
        }
    }

    //Класс для чтения/записи INI-файлов
    public class INIManager
    {
        //Конструктор, принимающий путь к INI-файлу
        public INIManager(string aPath)
        {
            path = aPath;
        }

        //Конструктор без аргументов (путь к INI-файлу нужно будет задать отдельно)
        public INIManager() : this("") { }

        //Возвращает значение из INI-файла (по указанным секции и ключу) 
        public string GetPrivateString(string aSection, string aKey)
        {
            //Для получения значения
            StringBuilder buffer = new StringBuilder(SIZE);

            //Получить значение в buffer
            GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

            //Вернуть полученное значение
            return buffer.ToString();
        }

        //Пишет значение в INI-файл (по указанным секции и ключу) 
        public void WritePrivateString(string aSection, string aKey, string aValue)
        {
            //Записать значение в INI-файл
            WritePrivateString(aSection, aKey, aValue, path);
        }

        //Возвращает или устанавливает путь к INI файлу
        public string Path { get { return path; } set { path = value; } }

        //Поля класса
        private const int SIZE = 1024; //Максимальный размер (для чтения значения из файла)
        private string path = null; //Для хранения пути к INI-файлу

        //Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        //Импорт функции WritePrivateProfileString (для записи значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);
    }

    public static class InstalledApplications
    {
        public static string GetApplicationInstallPath(string nameOfAppToFind)
        {
            string installedPath;
            string keyName;

            // search in: CurrentUser
            keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.CurrentUser, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }

            // search in: LocalMachine_32
            keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }

            // search in: LocalMachine_64
            keyName = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }

            return string.Empty;
        }

        private static string ExistsInSubKey(RegistryKey root, string subKeyName, string attributeName, string nameOfAppToFind)
        {
            RegistryKey subkey;
            string displayName;

            using (RegistryKey key = root.OpenSubKey(subKeyName))
            {
                if (key != null)
                {
                    foreach (string kn in key.GetSubKeyNames())
                    {
                        using (subkey = key.OpenSubKey(kn))
                        {
                            displayName = subkey.GetValue(attributeName) as string;
                            if (displayName != null && displayName.Contains(nameOfAppToFind) == true)
                            {
                                return subkey.GetValue("InstallLocation") as string;
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }

    public static class TextBoxWatermarkExtensionMethod
    {
        private const uint ECM_FIRST = 0x1500;
        private const uint EM_SETCUEBANNER = ECM_FIRST + 1;

        public static void SetWatermark(this TextBox textBox, string watermarkText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, watermarkText);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
    }
}