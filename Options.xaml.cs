using App;
using log4net;
using Microsoft.Win32;
using OysterVPNLibrary;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OysterVPN
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {

            InitializeComponent();

            try
            {

                Settings.fetchSettings();

                // this.Left = this.Owner.Left + (this.Owner.Width - this.ActualWidth) / 2;
                // this.Top = this.Owner.Top + 20;
                var protocol = Settings.getProtocol().Length == 0  ? Settings.defaultProtocol : Settings.getProtocol();

                switch (protocol)
                {
                    case "L2TP":
                        protocol_l2tp.IsChecked = true;
                        break;
                    case "PPTP":
                        protocol_pptp.IsChecked = true;
                        break;
                    case "IKEV2":
                        protocol_ikev.IsChecked = true;
                        break;

                    case "TCP":
                        protocol_tcp.IsChecked = true;
                        break;
                    case "UDP":
                        protocol_udp.IsChecked = true;
                        break;
                }

                userEmail.Content = Settings.getEmail();
                userName.Content = Settings.getName();
                userPhone.Content = Settings.getPhone();

                checkBoxConnnectLastUsedLocation.IsChecked = Settings.getConnectLastUsedServer();
                checkBoxLaunchStartup.IsChecked = Settings.getlaunchStartup();
                checkBoxVpnMinmized.IsChecked = Settings.getStartVpnMinimize();

                apppVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                //try
                //{
                //    apppVersion.Content= ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                //}
                //catch (InvalidDeploymentException)
                //{
                //    apppVersion.Content = "not installed";
                //}
            }
            catch(Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }

        }


       

        public void protocol_automatic_Checked(object sender, RoutedEventArgs e)
        { 
            protocol_l2tp.IsChecked = false;
            protocol_udp.IsChecked = false;
            protocol_tcp.IsChecked = false;
            protocol_ikev.IsChecked = false;
            protocol_pptp.IsChecked = false;

            Settings.setProtocol("IKEV2");

        }

        private void protocol_udp_Checked(object sender, RoutedEventArgs e)
        {
            protocol_l2tp.IsChecked = false;
            protocol_tcp.IsChecked = false;
            protocol_pptp.IsChecked = false;
            protocol_ikev.IsChecked = false;
            protocol_automatic.IsChecked = false;

            Settings.setProtocol("UDP");
        }

        private void protocol_tcp_Checked(object sender, RoutedEventArgs e)
        {
            protocol_l2tp.IsChecked = false;
            protocol_udp.IsChecked = false;
            protocol_ikev.IsChecked = false;
            protocol_pptp.IsChecked = false;
            protocol_automatic.IsChecked = false;

            Settings.setProtocol("TCP");
        }

        private void protocol_ikev_Checked(object sender, RoutedEventArgs e)
        {
            protocol_l2tp.IsChecked = false;
            protocol_udp.IsChecked = false;
            protocol_tcp.IsChecked = false;
            protocol_pptp.IsChecked = false;
            protocol_automatic.IsChecked = false;
            Settings.setProtocol("IKEV2");
        }

        private void protocol_l2tp_Checked(object sender, RoutedEventArgs e)
        {
            protocol_ikev.IsChecked = false;
            protocol_udp.IsChecked = false;
            protocol_tcp.IsChecked = false;
            protocol_automatic.IsChecked = false;
            protocol_pptp.IsChecked = false;
            Settings.setProtocol("L2TP");
        }

        private void protocol_pptp_Checked(object sender, RoutedEventArgs e)
        {
            protocol_ikev.IsChecked = false;
            protocol_udp.IsChecked = false;
            protocol_tcp.IsChecked = false;
            protocol_automatic.IsChecked = false;
            protocol_l2tp.IsChecked = false;
            Settings.setProtocol("PPTP");
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            var data = client.PostData(Settings.ApiUrl + "logout",null,Settings.AuthToken);

            Settings.setToken("");

            this.Hide();
            CloseAllWindows();
            //Window mainWindow = Application.Current.MainWindow;
            //mainWindow.Close();
            Login login = new Login();
            login.ShowDialog();
            this.Close();


        }

        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Hide();
        }

        private void server_list_tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void checkBoxLaunchStartup_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void checkBoxVpnMinmized_Checked(object sender, RoutedEventArgs e)
        {
        
        }

        private void checkBoxConnnectLastUsedLocation_Checked(object sender, RoutedEventArgs e)
        {
          
        }

        private void checkBoxLaunchStartup_Click(object sender, RoutedEventArgs e)
        {

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
           ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (checkBoxLaunchStartup.IsChecked.Value==true)
            {
                registryKey.SetValue("OysterVPN", System.Windows.Forms.Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue("OysterVPN");
            }


            Settings.setlaunchStartup(checkBoxLaunchStartup.IsChecked.Value);

        }

        private void checkBoxVpnMinmized_Click(object sender, RoutedEventArgs e)
        {
            Settings.setStartVpnMinimize(checkBoxVpnMinmized.IsChecked.Value);
        }

        private void checkBoxConnnectLastUsedLocation_Click(object sender, RoutedEventArgs e)
        {
            Settings.fetchSettings();

            Settings.setConnectLastUsedServer(checkBoxConnnectLastUsedLocation.IsChecked.Value);
            var server = Settings.getServer();
            Settings.setLastUsedServer(server);
           // var d = Settings.getLastServerUsed();
       //     System.Windows.Forms.MessageBox.Show("Test");

        }
    }
}
