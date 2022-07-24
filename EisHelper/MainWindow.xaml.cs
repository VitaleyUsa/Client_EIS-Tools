using Microsoft.Win32;

using MySqlConnector;

using NetFwTypeLib;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EisHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields

        private static readonly string ConnectionTimeout = "5";
        private static readonly string Login = "root";

        private static readonly string TMPe = ""; // ввести пароль от бд енота
        private static readonly string TMPE = ""; // ввести пароль от бд клиента еис

        private static string backupPath = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine) + "\\Backup_EISSyncService";
        private static string cur_status = "Пустой ответ";
        private static string installLocation = "C:\\Program Files (x86)\\EIS\\";
        private static string Port = "21285";
        private static string Server = "127.0.0.1";
        private static string tmpEISfiles = Environment.GetEnvironmentVariable("AppData", EnvironmentVariableTarget.Machine);
        private static string upgradeCode = @"{02FFDDB1-4094-4434-879A-70AC8870FCCE}";

        #endregion Private Fields

        #region Public Constructors

        public MainWindow()
        {
            InitializeComponent();
            /*#if DEBUG
                        stackLogin.Visibility = Visibility.Collapsed;
                        MainGrid.Visibility = Visibility.Visible;
            #endif*/
            BackupSyncServiceLocation();
            FindEISLocation();
            CheckOutFilePartSize();
        }

        // Узнаем размер out-file-part
        private void CheckOutFilePartSize()
        {
            try { CheckOutFilePartSizeHelper(new FileInfo(installLocation + "DBData\\notary\\out_file_part.ibd")); }
            catch
            {
                cb_file_out_part.IsEnabled = false;
                tb_file_out_part.Text = "Внимание! Таблица out-file-part не найдена";
            }
        }

        private void CheckOutFilePartSizeHelper(FileInfo fs)
        {
            long filesizeInMegaBytes = fs.Length >> 20;

            if (filesizeInMegaBytes > 0)
            {
                cb_file_out_part.IsEnabled = true;
                tb_file_out_part.Text = "Размер таблицы out-file-part = " + filesizeInMegaBytes + " MB. Вы хотите очистить ее?";
            }
            else
            {
                cb_file_out_part.IsEnabled = false;
                tb_file_out_part.Text = "Размер таблицы в пределах нормы. Очистка не требуется";
            }
        }

        #endregion Public Constructors

        #region Public Methods

        // Копирование директорий
        private static void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(System.IO.Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        // Запись ресурсов в файлы
        private void WriteResourceToFile(string resourceName, string fileName)
        {
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        // Поиск месторасположения ЕИС
        private static void FindEISLocation()
        {
            StringBuilder sbProductCode = new StringBuilder(39);
            StringBuilder sbInstallLocation = new StringBuilder();

            for (int iProductIndex = 0; ; iProductIndex++)
            {
                int iRes = NativeMethods.MsiEnumRelatedProducts(upgradeCode, 0, iProductIndex, sbProductCode);
                if (iRes != NativeMethods.NoError)
                {
                    // NativeMethods.ErrorNoMoreItems=259
                    break;
                }
                string productCode = sbProductCode.ToString();

                int status = GetProperty(productCode, "InstallLocation", sbInstallLocation);
                installLocation = sbInstallLocation.ToString();
            }
        }

        private static int GetProperty(string productCode, string propertyName, StringBuilder sbBuffer)
        {
            int len = sbBuffer.Capacity;
            sbBuffer.Length = 0;
            int status = NativeMethods.MsiGetProductInfo(productCode,
                                                          propertyName,
                                                          sbBuffer, ref len);
            if (status == NativeMethods.ErrorMoreData)
            {
                len++;
                sbBuffer.EnsureCapacity(len);
                status = NativeMethods.MsiGetProductInfo(productCode, propertyName, sbBuffer, ref len);
            }
            if ((status == NativeMethods.ErrorUnknownProduct ||
                 status == NativeMethods.ErrorUnknownProperty)
                && (String.Compare(propertyName, "ProductVersion", StringComparison.Ordinal) == 0 ||
                    String.Compare(propertyName, "ProductName", StringComparison.Ordinal) == 0))
            {
                // try to get version manually
                StringBuilder sbKeyName = new StringBuilder();
                sbKeyName.Append("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Installer\\UserData\\S-1-5-18\\Products\\");
                Guid guid = new Guid(productCode);
                byte[] buidAsBytes = guid.ToByteArray();
                foreach (byte b in buidAsBytes)
                {
                    int by = ((b & 0xf) << 4) + ((b & 0xf0) >> 4);  // swap hex digits in the byte
                    sbKeyName.AppendFormat("{0:X2}", by);
                }
                sbKeyName.Append("\\InstallProperties");
                RegistryKey key = Registry.LocalMachine.OpenSubKey(sbKeyName.ToString());
                if (key != null)
                {
                    string valueName = "DisplayName";
                    if (String.Compare(propertyName, "ProductVersion", StringComparison.Ordinal) == 0)
                        valueName = "DisplayVersion";
                    string val = key.GetValue(valueName) as string;
                    if (!String.IsNullOrEmpty(val))
                    {
                        sbBuffer.Length = 0;
                        sbBuffer.Append(val);
                        status = NativeMethods.NoError;
                    }
                }
            }

            return status;
        }

        private static void RunCmd(string command)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            //cmd.StartInfo.RedirectStandardInput = true;
            //cmd.StartInfo.RedirectStandardOutput = true;
            //cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Arguments = "/K " + command;
            cmd.Start();
            cmd.StandardInput.Close();
            cmd.WaitForExit(5000);
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        private static extern bool ZeroMemory(IntPtr Destination, int Length);

        // Поиск месторасположения бэкапа ЕИС
        private bool BackupSyncServiceLocation()
        {
            if (File.Exists(backupPath + "\\Archive\\ArchiveData")
                && File.Exists(backupPath + "\\BadBackup\\LastFile")
                && File.Exists(backupPath + "\\Stamp\\Stamp"))
            {
                cb_registration.IsEnabled = true;
                tb_registration.Text = "Найдена резервная копия регистрационных данных";
                return true;
            }
            else
            {
                backupPath = System.IO.Path.GetTempPath();
                if (File.Exists(backupPath + "\\Backup_EISSyncService" + "\\Archive\\ArchiveData")
                    && File.Exists(backupPath + "\\Backup_EISSyncService" + "\\BadBackup\\LastFile")
                    && File.Exists(backupPath + "\\Backup_EISSyncService" + "\\Stamp\\Stamp"))
                {
                    cb_registration.IsEnabled = true;
                    tb_registration.Text = "Найдена резервная копия регистрационных данных";
                    return true;
                }
                else
                {
                    cb_registration.IsEnabled = false;
                    tb_registration.Text = "Резервная копия регистрационных данных не найдена";
                }
            }

            return false;
        }

        private void credentials_Checked(object sender, RoutedEventArgs e)
        {
            credentials_input.Visibility = Visibility.Visible;
        }

        private void credentials_Unchecked(object sender, RoutedEventArgs e)
        {
            credentials_input.Visibility = Visibility.Collapsed;
        }

        // Существование службы
        private bool DoesServiceExist(string serviceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(serviceName));
        }

        // Очистка всех регистрационных данных
        private async Task MariaDB_flush_credentials(string connString)
        {
            string message = "Вы точно уверены, что хотите полностью сбросить регистрацию? Вам придется заново подавать заявку на регистрацию!";
            string caption = "Внимание!";

            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var conn = new MySqlConnection(connString))
                {
                    try
                    {
                        await conn.OpenAsync();

                        using (var cmd = new MySqlCommand())
                        {
                            cmd.Connection = conn;

                            // Чистим таблицу с учетными данными
                            cmd.CommandText = "USE account; DELETE FROM local_user_role; DELETE FROM local_role; DELETE FROM local_user;";
                            await cmd.ExecuteNonQueryAsync();

                            if (DoesServiceExist("EISSyncService"))
                            {
                                string successMsg = "Регистрация была полностью удалена";
                                ServiceController service = new ServiceController("EISSyncService");

                                if ((service.Status.Equals(ServiceControllerStatus.Running)) || (service.Status.Equals(ServiceControllerStatus.StartPending)))
                                    service.Stop();

                                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));

                                try
                                {
                                    string eisHelper_backup = "SyncService\\EisHelper_Backup_after_flush";
                                    Directory.CreateDirectory(installLocation + eisHelper_backup);

                                    if (Directory.Exists(installLocation + "SyncService\\Archive"))
                                    {
                                        Copy(installLocation + "SyncService\\Archive", installLocation + eisHelper_backup + "\\Archive");
                                        File.Delete(installLocation + "SyncService\\Archive\\ArchiveData");
                                    }

                                    if (Directory.Exists(installLocation + "SyncService\\BadBackup"))
                                    {
                                        Copy(installLocation + "SyncService\\BadBackup", installLocation + eisHelper_backup + "\\BadBackup");
                                        File.Delete(installLocation + "SyncService\\BadBackup\\LastFile");
                                    }
                                }
                                catch { successMsg = "Невозможно полностью удалить регистрацию"; }

                                service.Start();
                                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));

                                MessageBox.Show(successMsg);
                            }
                            else MessageBox.Show("Служба EISSyncService не найдена");

                            await conn.CloseAsync();
                        }
                    }
                    catch (Exception exc) { await conn.CloseAsync(); MessageBox.Show(exc.Message + "\nYour host: " + Server + "\nPort: " + Port); }
                }
            }
            else cb_registration_drop.IsChecked = false;
        }

        // Сброс ошибки 500 в профиле
        private async Task MariaDB_flush_profile(string connString)
        {
            using (var conn = new MySqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;

                        // Чистим таблицу с учетными данными
                        cmd.CommandText = "USE notary; DELETE FROM dc_assistant; DELETE FROM dc_notary; DELETE FROM dc_user;";
                        await cmd.ExecuteNonQueryAsync();
                        await conn.CloseAsync();
                    }
                }
                catch (Exception exc) { await conn.CloseAsync(); MessageBox.Show(exc.Message + "\nYour host: " + Server + "\nPort: " + Port); }
            }
        }

        // Получение логина / пароля пользователя
        private async Task MariaDB_get_credentials(string connString)
        {
            using (var conn = new MySqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;

                        string isExists = "";
                        cmd.CommandText = "USE account; SELECT Login FROM local_user";
                        using (var reader = await cmd.ExecuteReaderAsync())
                            while (await reader.ReadAsync())
                                isExists += (reader.GetString(0));

                        if (String.IsNullOrEmpty(isExists))
                        {
                            cb_credentials.IsEnabled = false;
                            tb_credentials.Visibility = Visibility.Visible;
                            credentials_input.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(credentials_name.Text))
                            {
                                credentials_name.Text += isExists;
                            }
                            else
                            {
                                cmd.CommandText = "USE account; UPDATE local_user SET local_user.Login='" + credentials_name.Text + "' WHERE local_user.id = 1;";
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }

                        cmd.CommandText = "USE account; UPDATE local_user SET local_user.PasswordHash='$2a$11$6fUVCjqXnc/QkoGJkeNe1OfGn0ekgIAb2cCEnXDDF4LBIbwq9iwl6' WHERE local_user.id = 1;";
                        await cmd.ExecuteNonQueryAsync();

                        MessageBox.Show("Имя пользователя и пароль были сброшены!");

                        await conn.CloseAsync();
                    }
                }
                catch (Exception exc) { await conn.CloseAsync(); MessageBox.Show(exc.Message + "\nYour host: " + Server + "\nPort: " + Port); }
            }
        }

        // Сброс пароля на статотчет
        private async Task Mysql_flush_statpass(string connString)
        {
            using (var conn = new MySqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;

                        // Чистим таблицу с учетными данными
                        cmd.CommandText = "SELECT schema_name FROM information_schema.schemata WHERE schema_name NOT IN ('performance_schema', 'mysql', 'information_schema', 'test');";
                        string dbName = Convert.ToString(cmd.ExecuteScalar());

                        cmd.CommandText = "USE " + dbName + "; UPDATE Resources SET Resources.Value='' WHERE Resources.Section='DBProperties' AND Resources.Key LIKE '%AccessCode%';";
                        await cmd.ExecuteNonQueryAsync();
                        await conn.CloseAsync();
                    }
                    MessageBox.Show("Пароли для статотчета сброшены");
                }
                catch (Exception exc) { await conn.CloseAsync(); MessageBox.Show(exc.Message + "\nYour host: " + Server + "\nPort: " + Port); }
                cb_enot_statpass.IsChecked = false;
            }
        }

        // Чекбокс - ЕИС - нажата
        private void RadioButton_eis_Checked(object sender, RoutedEventArgs e)
        {
            if (EIS != null)
            {
                Port = "21285";
                var result = txt_Server.Tag.ToString().Substring(txt_Server.Tag.ToString().LastIndexOf(':') + 1);
                result = txt_Server.Tag.ToString().Remove(txt_Server.Tag.ToString().IndexOf(result));
                txt_Server.Tag = result + Port;

                cb_registration_drop.Visibility = Visibility.Visible;
                EIS.Visibility = Visibility.Visible;
                Enot.Visibility = Visibility.Collapsed;
            }
        }

        // Чекбокс - Енот - нажата
        private void RadioButton_enot_Checked(object sender, RoutedEventArgs e)
        {
            if (EIS != null)
            {
                Port = "3306";
                var result = txt_Server.Tag.ToString().Substring(txt_Server.Tag.ToString().LastIndexOf(':') + 1);
                result = txt_Server.Tag.ToString().Remove(txt_Server.Tag.ToString().IndexOf(result));
                txt_Server.Tag = result + Port;

                cb_registration_drop.Visibility = Visibility.Collapsed;
                EIS.Visibility = Visibility.Collapsed;
                Enot.Visibility = Visibility.Visible;
            }
        }

        private async void start_Click(object sender, RoutedEventArgs e)
        {
            List<RadioButton> radioButtons = EnotEis.Children.OfType<RadioButton>().ToList();
            RadioButton rbTarget = radioButtons
                  .Where(r => r.IsChecked == true)
                  .Single();
            Server = txt_Server.Text;
            if (String.IsNullOrEmpty(Server)) { Server = "localhost"; }

            if (!String.IsNullOrEmpty(txt_Server.Text))
            {
                if (txt_Server.Text.Contains(":"))
                {
                    var result = txt_Server.Text.Substring(txt_Server.Text.LastIndexOf(':') + 1);
                    Server = txt_Server.Text.Remove(txt_Server.Text.IndexOf(result));
                    Server = Server.Remove(Server.IndexOf(":"));
                }
            }

            string connString = @"Server=" + Server + ";Port=" + Port + ";ConnectionTimeout=" + ConnectionTimeout + ";Pooling=false;User ID=" + Login + ";Password=";

            if (rbTarget == rb_EIS)
            {
                //GCHandle gCHandle = GCHandle.Alloc(TMPE, GCHandleType.Pinned);

                connString += TMPE;
                // Сброс пароля пользователя и логина
                if (cb_credentials.IsChecked == true)
                {
                    await MariaDB_get_credentials(connString);

                    cb_credentials.IsChecked = false;
                    credentials_input.Visibility = Visibility.Visible;
                }

                // ФЦИИТ Обнуление регистрации
                if (cb_registration_drop.IsChecked == true)
                {
                    await MariaDB_flush_credentials(connString);

                    cb_registration_drop.IsChecked = false;
                }

                // Очистка таблицы out_file_part
                if (cb_file_out_part.IsChecked == true)
                {
                    if (DoesServiceExist("EisDB"))
                    {
                        ServiceController service = new ServiceController("EisDB");
                        if ((service.Status.Equals(ServiceControllerStatus.Running)) || (service.Status.Equals(ServiceControllerStatus.StartPending)))
                            service.Stop();

                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));

                        string successMsg = "Таблица out_file_part была очищена!";
                        try
                        {
                            File.Delete(installLocation + "\\DBDATA\\Notary\\out_file_part.frm");
                            File.Delete(installLocation + "\\DBDATA\\Notary\\out_file_part.ibd");

                            var resourceName = "EisHelper.out_file_part.frm";
                            WriteResourceToFile(resourceName, installLocation + "\\DBData\\out_file_part.frm");

                            resourceName = "EisHelper.out_file_part.ibd";
                            WriteResourceToFile(resourceName, installLocation + "\\DBData\\out_file_part.ibd");
                        }
                        catch { successMsg = "Не получилось очистить таблицу out_file_part"; }

                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));

                        MessageBox.Show(successMsg);
                    }
                    else MessageBox.Show("Служба EisDB не найдена");

                    cb_file_out_part.IsChecked = false;
                }

                // Восстановление регистрации
                if (cb_registration.IsChecked == true)
                {
                    if (DoesServiceExist("EISSyncService"))
                    {
                        string successMsg = "Регистрация восстановлена!";
                        ServiceController service = new ServiceController("EISSyncService");

                        if ((service.Status.Equals(ServiceControllerStatus.Running)) || (service.Status.Equals(ServiceControllerStatus.StartPending)))
                            service.Stop();

                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));

                        try
                        {
                            if (Directory.Exists(installLocation + "SyncService\\EisHelper_Backup"))
                                Directory.Delete(installLocation + "SyncService\\EisHelper_Backup", true);

                            Directory.CreateDirectory(installLocation + "SyncService\\EisHelper_Backup");
                            if (Directory.Exists(installLocation + "SyncService\\Archive"))
                                Directory.Move(installLocation + "SyncService\\Archive", installLocation + "SyncService\\EisHelper_Backup\\Archive");
                            if (Directory.Exists(installLocation + "SyncService\\BadBackup"))
                                Directory.Move(installLocation + "SyncService\\BadBackup", installLocation + "SyncService\\EisHelper_Backup\\BadBackup");
                            if (Directory.Exists(installLocation + "SyncService\\Stamp"))
                                Directory.Move(installLocation + "SyncService\\Stamp", installLocation + "SyncService\\EisHelper_Backup\\Stamp");

                            Copy(backupPath + "\\Archive", installLocation + "SyncService\\Archive");
                            Copy(backupPath + "\\BadBackup", installLocation + "SyncService\\BadBackup");
                            Copy(backupPath + "\\Stamp", installLocation + "SyncService\\Stamp");
                        }
                        catch { successMsg = "Невозможно восстановить регистрацию"; }

                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));

                        MessageBox.Show(successMsg);
                    }
                    else MessageBox.Show("Служба EISSyncService не найдена");

                    cb_registration.IsChecked = false;
                }

                // Фикс для запуска клиента ЕИС
                if (cb_global_fix.IsChecked == true)
                {
                    try
                    {
                        cur_status = "'Удаление папки ЕИСКлиент'";
                        if (Directory.Exists(tmpEISfiles + "\\EISClient"))
                            Directory.Delete(tmpEISfiles + "\\EISClient", true);
                        cur_status = "'Удаление папки ФНП'";
                        if (Directory.Exists(tmpEISfiles + "\\FNP"))
                            Directory.Delete(tmpEISfiles + "\\FNP", true);

                        cur_status = "'Запуск службы базы данных ЕИС'";
                        if (DoesServiceExist("EisDB"))
                        {
                            ServiceController service = new ServiceController("EisDB");
                            if ((service.Status.Equals(ServiceControllerStatus.Stopped)) || (service.Status.Equals(ServiceControllerStatus.StopPending)))
                                service.Start();
                            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
                        }

                        cur_status = "'Запуск службы синхронизации ЕИС'";
                        if (DoesServiceExist("EISSyncService"))
                        {
                            ServiceController service2 = new ServiceController("EISSyncService");
                            if ((service2.Status.Equals(ServiceControllerStatus.Stopped)) || (service2.Status.Equals(ServiceControllerStatus.StopPending)))
                                service2.Start();
                            service2.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
                        }

                        cur_status = "'Настройка службы синхронизации'";
                        if (DoesServiceExist("EISSyncService"))
                            try { RunCmd("sc config Everything start= delayed-auto"); RunCmd("sc failure EISSyncService reset= 900 actions= restart/10000/restart/60000"); }
                            catch { Console.WriteLine("Возможно, что произошла проблема при настройке службы синхронизации"); }

                        cur_status = "'Настройка службы MariaDB'";
                        if (DoesServiceExist("EisDB"))
                            try { RunCmd("sc failure EISDB reset= 900 actions= restart/10000/restart/60000"); }
                            catch { Console.WriteLine("Возможно, что произошла проблема при настройке службы MariaDB"); }

                        cur_status = "'Удаление правил брандмауэра'";
                        INetFwPolicy2 policy2 = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                        try
                        {
                            policy2.Rules.Remove("MySQL");
                            policy2.Rules.Remove("EnotNew");
                            policy2.Rules.Remove("EIS");

                            policy2.Rules.Remove("MySQL");
                            policy2.Rules.Remove("EnotNew");
                            policy2.Rules.Remove("EIS");
                        }
                        catch (Exception) { }

                        cur_status = "'Добавление правил брандмауэра'";
                        INetFwRule rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwRule"));
                        rule.Name = "EIS";

                        rule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                        rule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                        rule.LocalPorts = "3306,7446,21285";
                        rule.RemotePorts = "3306,7446,21285";
                        rule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                        rule.Enabled = true;

                        try { policy2.Rules.Add(rule); }
                        catch (Exception) { }

                        INetFwPolicy2 policy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                        rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwRule"));
                        rule.Name = "EIS";

                        rule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                        rule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                        rule.LocalPorts = "3306,7446,21285";
                        rule.RemotePorts = "3306,7446,21285";
                        rule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                        rule.Enabled = true;
                        try { policy.Rules.Add(rule); }
                        catch (Exception) { }

                        MessageBox.Show("Фикс применен успешно");
                    }
                    catch { MessageBox.Show("Произошла ошибка на этапе: " + cur_status); }
                }

                if (cb_jpo_500.IsChecked == true)
                {
                    if (DoesServiceExist("EISSyncService"))
                    {
                        string successMsg = "Применение исправления ошибки профиля завершено";

                        Process.Start("net", "stop EISSyncService").WaitForExit();

                        if (DoesServiceExist("EisDB"))
                        {
                            MainGrid.Visibility = Visibility.Collapsed;
                            stackStatus.Visibility = Visibility.Visible;

                            Process.Start("net", "start EisDB").WaitForExit();

                            await MariaDB_flush_profile(connString);
                        }
                        else MessageBox.Show("Служба EisDB не найдена");

                        Process.Start("net", "start EISSyncService").WaitForExit();
                        await Task.Delay(5000);
                        Process.Start("net", "stop EISSyncService").WaitForExit();
                        await Task.Delay(5000);
                        Process.Start("net", "start EISSyncService").WaitForExit();

                        MessageBox.Show(successMsg);
                    }
                    else MessageBox.Show("Служба EISSyncService не найдена");

                    cb_jpo_500.IsChecked = false;
                    MainGrid.Visibility = Visibility.Visible;
                    stackStatus.Visibility = Visibility.Collapsed;
                }

                //ZeroMemory(gCHandle.AddrOfPinnedObject(), TMPE.Length * 2);
                //gCHandle.Free();
            }
            else
            // ЕНОТ
            {
                connString += TMPe;
                if (cb_enot_statpass.IsChecked == true)
                {
                    await Mysql_flush_statpass(connString);
                }
            }
        }

        #endregion Private Methods

        #region Internal Classes

        internal static class NativeMethods
        {
            #region Internal Fields

            internal const int ErrorMoreData = 234;
            internal const int ErrorNoMoreItems = 259;
            internal const int ErrorUnknownProduct = 1605;
            internal const int ErrorUnknownProperty = 1608;
            internal const int MaxGuidChars = 38;
            internal const int NoError = 0;

            #endregion Internal Fields

            #region Internal Methods

            [DllImport("msi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int MsiEnumRelatedProducts(string lpUpgradeCode, int dwReserved,
                int iProductIndex, //The zero-based index into the registered products.
                StringBuilder lpProductBuf); // A buffer to receive the product code GUID.

            // This buffer must be 39 characters long. The first 38 characters are for the GUID, and
            // the last character is for the terminating null character.

            [DllImport("msi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern Int32 MsiGetProductInfo(string product, string property,
                StringBuilder valueBuf, ref Int32 cchValueBuf);

            #endregion Internal Methods
        }

        #endregion Internal Classes

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pbPassword.Password == "еис" || pbPassword.Password == "tbc" || pbPassword.Password == "eis")
            {
                stackLogin.Visibility = Visibility.Collapsed;
                MainGrid.Visibility = Visibility.Visible;
            }
            else
            {
                pbPassword.Password = "";
                tbPassword.Text = "Пароль не подходит, попробуйте еще раз!";
            }
        }
    }
}