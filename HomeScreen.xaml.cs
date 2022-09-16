using App;
using NativeMessaging;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json.Linq;

namespace OysterVPN
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {

        //for already activate window connect
        private static int counter = 0;


        static Host Host;

        NotifyIcon notifyIcon;


        //private static HomeScreen instance;

        //public static HomeScreen Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new HomeScreen();
        //        }
        //        return instance;
        //    }
        //}



        public HomeScreen()
        {

            InitializeComponent();

            //fetch data.bin settings
            Settings.fetchSettings();


            MinimizeToTray.Enable(this);

            Closing += OnClosing;

            if (Settings.getinternetKillSwitch() == true)
            {
                IsKillSwitch.IsChecked = true;
            }

            if (Settings.getServer() != null)
            {
                countryFlagImage.Source = new BitmapImage(new Uri(Settings.getServer().Flag));
                currentLocation.Content = Settings.getServer().Name;
            }
            else
            {
                countryFlagImage.Source = new BitmapImage(new Uri("https://api.staging.oystervpn.co/flags/fr.png"));
                currentLocation.Content = "France";
            }
             

            #region vpn extension

           // if (ExtensionUserInfo.IsConnectVpnExtension == true)
           // {

           //     btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

           //     this.WindowState = WindowState.Minimized;

           //     //    ExtensionUserInfo.DisConnectedChanged += delegate (object sender, EventArgs e) { CheckConnectionChrome(sender, e, BtnDis); };

           //// ExtensionUserInfo.DisConnectedChanged += CheckConnectionChrome;

           // }


            //if (ExtensionUserInfo.IsDisConnectVpnExtension == true)
            //{
            //    btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
            //}


            #endregion


        }

    

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (System.Windows.MessageBox.Show(this, "Are you sure want to quit?", "Confirm", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                cancelEventArgs.Cancel = true;

            }
            Environment.Exit(Environment.ExitCode);

        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    break;
                case WindowState.Minimized:
                    this.notifyIcon = new NotifyIcon();
                    notifyIcon.BalloonTipText = "This is WPF app";
                    notifyIcon.Icon = new System.Drawing.Icon("Todolist.ico");
                    notifyIcon.Visible = true;
                    notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
                    this.notifyIcon.ShowBalloonTip(1000);
                    this.Hide();
                    break;
                case WindowState.Normal:
                    break;
            }
        }

        private void OnNotifyIconDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();
            WindowState = WindowState.Normal;
        }



        private void Connect_Button(object sender, RoutedEventArgs e)
        {

        }

        private void Location_Button(object sender, RoutedEventArgs e)
        {
            VpnLocations location = new VpnLocations();

            location.ShowDialog();

            //if (IsWindowOpen<Window>("VpnLocations"))
            //{
            //  //  location.ShowDialog();
            //    // MyWindowName is open

            //}
            //else
            //{
            //    //    current_location.Content = "Germany";

            //    location.ShowDialog();
            //}

        }
        //public static bool IsWindowOpen<T>(string name = "") where T : Window
        //{
        //    return string.IsNullOrEmpty(name)
        //       ? Application.Current.Windows.OfType<T>().Any()
        //       : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        //}

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

       

            LoadingBar.Visibility = Visibility.Visible;
            connect_label.Content = "Connecting...";

            //if internet kill switch is enable 
            if (Settings.getinternetKillSwitch())
            {
                OysterVpn.DisableKillSwitch();
            }
            //   bool isFetch = Settings.fetchSettings();
            string defaultProtocol = Settings.defaultProtocol;

            switch (defaultProtocol)
            {
                case "L2TP":
                    OysterVpn.connect("L2TP", "", "", "");
                    break;
                case "PPTP":
                    OysterVpn.connect("PPTP", "", "", "");
                    break;
                case "IKEV2":
                    OysterVpn.connect("IKEV2", "", "", "");
                    break;

                case "TCP":
                    //OysterVpn.connect("TCP", "ca1.ca", "tcp.tls", "us-cf-ovtcp-01.jumptoserver.com 4443");
                    OysterVpn.connect("TCP", "ca1.ca", "tcp.tls", Settings.getServer() == null ? "ost104.serverintoshell.com 4443" : Settings.getServer().Ip + " " + Settings.getServer().Port);
                    break;
                case "UDP":
                    //OysterVpn.connect("UDP", "ca2.ca", "tcp.tls", "us-cf-ovudp-01.jumptoserver.com 4443");
                    OysterVpn.connect("UDP", "ca2.ca", "udp.tls", Settings.getServer() == null ? "ost104.serverintoshell.com 4443" : Settings.getServer().Ip + " " + Settings.getServer().Port);
                    break;
            }


            var notificationManager = new NotificationManager();

            notificationManager.Show(new NotificationContent
            {
                Title = "notification",
                Message = "Connected To The Oyster VPN Server",
                Type = NotificationType.Success
            });


            btnConnect.Visibility = Visibility.Hidden;
            btnDisConnect.Visibility = Visibility.Visible;


            connect_label.Content = "Connected";
            LoadingBar.Visibility = Visibility.Hidden;

            //send signal to extension for connect
            //if (ExtensionUserInfo.IsConnectVpnExtension == true)
            //{
            //    JObject data = new JObject();
            //    data.Add("disConnectVpn", false);
            //    data.Add("connectVpn", true);
            //    Host.SendMessage(data);

            //    //prevent for further connect 
            //    ExtensionUserInfo.IsDisConnectVpnExtension = false;
            //    ExtensionUserInfo.IsConnectVpnExtension = true;
            //}

        }

        private void btnDisConnect_Click(object sender, RoutedEventArgs e)
        {
        
       


            LoadingBar.Visibility = Visibility.Visible;
            connect_label.Content = "Disconnecting...";

            #region check kill switch
            CheckInternetKillSwitch();
            #endregion

            OysterVpn.disconnect();

            btnDisConnect.Visibility = Visibility.Hidden;
            btnConnect.Visibility = Visibility.Visible;
            connect_label.Content = "Disconnected";
            LoadingBar.Visibility = Visibility.Hidden;

            //send signal to extension for disconnect

            //if (ExtensionUserInfo.IsDisConnectVpnExtension == true)
            //{
              
            //    JObject data = new JObject();
            //    data.Add("disConnectVpn", true);
            //    data.Add("connectVpn", false);
            //    Host.SendMessage(data);

            //    ExtensionUserInfo.IsDisConnectVpnExtension = false;
            //    ExtensionUserInfo.IsConnectVpnExtension = false;
            //}


        }

        public void CheckInternetKillSwitch()
        {
            if (IsKillSwitch.IsChecked)
            {
                Settings.setinternetKillSwitch(true);
                //  OysterVpn.ProtectDNSLeak();
                OysterVpn.KillSwitchWindow();
            }
            else
            {
                Settings.setinternetKillSwitch(false);
                //   OysterVpn.UnprotectDNSLeak();
                OysterVpn.DisableKillSwitch();
            }
        }

        private void MenuItem_VPNLocation_Click(object sender, RoutedEventArgs e)
        {
            VpnLocations vp = new VpnLocations();
            vp.ShowDialog();

        }

        private void MenuItem_SpeedTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_HelpSupportClick(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Options_Click(object sender, RoutedEventArgs e)
        {
            Options option = new Options();

          
            option.Show();

        }

        private void MenuItem_ContactSupport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_IpAddressChecker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_DnsLeakTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_AboutOysterVpn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Quit_Click(object sender, RoutedEventArgs e)
        {

            OysterVpn.disconnect();
            Environment.Exit(Environment.ExitCode);
        }

        private void IsKillSwitch_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(Settings.getinternetKillSwitch().ToString());
            if (IsKillSwitch.IsChecked)
            {
                Settings.setinternetKillSwitch(true);
                //    OysterVpn.ProtectDNSLeak();
                //   OysterVpn.KillSwitchWindow();
            }
            else
            {
                Settings.setinternetKillSwitch(false);
                //    OysterVpn.UnprotectDNSLeak();
                //  OysterVpn.DisableKillSwitch();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Enable "minimize to tray" behavior for this Window
            MinimizeToTray.Enable(this);
        }


        private void Window_Activated(object sender, EventArgs e)
        {

            // System.Windows.Forms.MessageBox.Show(ExtensionUserInfo.IsConnectVpnExtension.ToString());
          //  connect_label.Content = ExtensionUserInfo.IsConnectVpnExtension.ToString();
            if (ExtensionUserInfo.IsDisConnectVpnExtension == true )
            {
                btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

                //prevent for further disconnect 
               ExtensionUserInfo.IsDisConnectVpnExtension = false;
                ExtensionUserInfo.IsConnectVpnExtension = false;
                counter = 0;
            }
           
           else if(ExtensionUserInfo.IsConnectVpnExtension==true)//  && counter == 0)
            {
                btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

                //prevent for further connect 
                ExtensionUserInfo.IsDisConnectVpnExtension = false;
                ExtensionUserInfo.IsConnectVpnExtension = true;
                counter = 1;
            }

            // Title = "Active Window";
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
                //Title = "Inactive Window";
        }


        public void CheckConnectionChrome(object sender, EventArgs e)
        {


            if (ExtensionUserInfo.IsDisConnectVpnExtension == true)
            {
                //var window = new Window();
                //var stackPanel = new StackPanel { Orientation = System.Windows.Controls.Orientation.Vertical };
                //stackPanel.Children.Add(new System.Windows.Controls.Label { Content = "Label" });
                //stackPanel.Children.Add(new System.Windows.Controls.Button { Content = "Button" });
                //window.Content = stackPanel;

                //   System.Windows.Forms.MessageBox.Show(label.ToString());
               // System.Windows.Forms.MessageBox.Show("test12321");

                // LoadingBar.Visibility = Visibility.Visible;
                //  connect_label.Content = "Disconnecting...";

                #region check kill switch
                // CheckInternetKillSwitch();
                #endregion

              //  OysterVpn.disconnect();

              // btnDisConnect.Visibility = Visibility.Hidden;
              //  btnConnect.Visibility = Visibility.Visible;
              //  connect_label.Content = "Disconnected";
              //  LoadingBar.Visibility = Visibility.Hidden;


                // btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

            }
            //Thread backgroundThread = new Thread(new ThreadStart(call));
            //backgroundThread.Priority = ThreadPriority.Highest;
            //backgroundThread.Start();


        }


            private void call()
        {
            try
            {
              //  btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));


             if (ExtensionUserInfo.IsDisConnectVpnExtension == true)
                {    
                 

                    //   btn.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

                    //System.Windows.Forms.MessageBox.Show("test12321");
                    //System.Windows.Forms.MessageBox.Show(connect_label.ToString());
                    //System.Windows.Forms.MessageBox.Show(btnConnect.ToString());
                    //System.Windows.Forms.MessageBox.Show(btnConnect.Visibility.ToString());
                   // connect_label.Content = "Disconnecting...";
                    System.Windows.Forms.MessageBox.Show(btnDisConnect.Visibility.ToString());
                    System.Windows.Forms.MessageBox.Show("working");
                    //home.btnDisConnect.Visibility = Visibility.Hidden;
                    //home.btnConnect.Visibility = Visibility.Visible;
                    //  connect_label.Content = "Disconnected";
                    ///  HomeScreen d = new HomeScreen();



                       // btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

                    //MainWindow w = new MainWindow();
                    //w.Show();

                    //LoadingBar.Visibility = Visibility.Visible;
                 

                    //#region check kill switch
                    //CheckInternetKillSwitch();
                    //#endregion

                    //OysterVpn.disconnect();

                    //btnDisConnect.Visibility = Visibility.Hidden;
                    //btnConnect.Visibility = Visibility.Visible;
                    //connect_label.Content = "Disconnected";
                    //LoadingBar.Visibility = Visibility.Hidden;



                    //    btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));


                    //Task.Delay(2000).ContinueWith(t =>
                    //{



                    //    //System.Windows.Forms.MessageBox.Show(ExtensionUserInfo.IsDisConnectVpnExtension.ToString());
                    //    //System.Windows.Forms.MessageBox.Show(connect_label.Content.ToString());
                    //    //connect_label.Content = "Disconnecting...";

                    //    //this.btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

                    //});

                    //  connect_label.Content = "Disconnecting...";
                    //   btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));



                    //  OysterVpn.disconnect();

                    //    System.Windows.Forms.MessageBox.Show(ExtensionUserInfo.IsDisConnectVpnExtension.ToString());


                    //LoadingBar.Visibility = Visibility.Visible;
                    //connect_label.Content = "Disconnecting...";


                    //btnDisConnect.Visibility = Visibility.Hidden;
                    //btnConnect.Visibility = Visibility.Visible;
                    //connect_label.Content = "Disconnected";
                    //LoadingBar.Visibility = Visibility.Hidden;

                    //  System.Windows.Forms.MessageBox.Show(ExtensionUserInfo.IsDisConnectVpnExtension.ToString());
                    // btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                    //   btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                }
            }
            catch (Exception ex)
            {
                Log.LogMessage(ex.Message.ToString());
                throw ex;
            }

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void SplitTunnel_Click(object sender, RoutedEventArgs e)
        {
            SplitTunnel sp = new SplitTunnel();
            sp.Show();

        }
    }
}
