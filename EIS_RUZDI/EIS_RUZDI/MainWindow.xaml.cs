using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace EIS_RUZDI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<ViewerApplication> viewers = new List<ViewerApplication>();
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                RegistryKey webClientsRootKey;
                //on 64bit the browsers are in a different location
                webClientsRootKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
                if (webClientsRootKey == null)
                    webClientsRootKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
                
                if (webClientsRootKey != null)
                    foreach (var subKeyName in webClientsRootKey.GetSubKeyNames())
                        if (webClientsRootKey.OpenSubKey(subKeyName) != null)
                            if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell") != null)
                                if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open") != null)
                                    if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command") != null)
                                    {
                                        string commandLineUri = (string)webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue(null);
                                        if (string.IsNullOrEmpty(commandLineUri))
                                            continue;
                                        commandLineUri = commandLineUri.Trim("\"".ToCharArray());
                                        ViewerApplication viewer = new ViewerApplication();
                                        viewer.Executable = commandLineUri;
                                        viewer.Name = (string)webClientsRootKey.OpenSubKey(subKeyName).GetValue(null);
                                        viewers.Add(viewer);
                                    }
            }
            this.listView.ItemsSource = viewers;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 certificate = null;
            //manually chose the certificate in the store
            X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(store.Certificates, null, null, X509SelectionFlag.SingleSelection);

            if (sel.Count > 0)
            {
                certificate = sel[0];
                //System.Diagnostics.Process.Start("https://rosreestr.eisnot.ru/?auth_code=" + certificate.SerialNumber);
                //System.Diagnostics.Process.Start("https://rz.eisnot.ru/?auth_code=" + certificate.SerialNumber);
                Process.Start(((sender as Control).Tag as ViewerApplication).Executable, @"https://rz.eisnot.ru/?auth_code=" + certificate.SerialNumber);
            }
        }
    }

    public class ViewerApplication
    {
        public string Name { get; set; }
        public string Executable { get; set; }
        public Icon Icon
        {
            get { return System.Drawing.Icon.ExtractAssociatedIcon(this.Executable); }
        }
        public ImageSource ImageSource
        {
            get
            {
                ImageSource imageSource;
                using (Bitmap bmp = Icon.ToBitmap())
                {
                    var stream = new MemoryStream();
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    imageSource = BitmapFrame.Create(stream);
                }
                return imageSource;
            }
        }
    }
}