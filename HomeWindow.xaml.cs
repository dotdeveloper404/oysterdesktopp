using App;
using CountryData.Standard;
using log4net;
using Microsoft.Win32;
using NativeMessaging;
using NetFwTypeLib; // Located in FirewallAPI.dll
using Newtonsoft.Json.Linq;
using Notifications.Wpf;
using OysterVPNLibrary;
using OysterVPNModel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace OysterVPN
{
    /// <summary>   
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {

        //for already activate window connect
        private static int counter = 0;

        static private bool isKill;

        private bool vpnON = false;

        private static bool IsBlockedFirewall = false;
        private static bool IsNetworkAvailable = true;
        private static bool AlreadyConnected = false;
        private static bool IsConnected = false;
        private static bool IsDisconnected = false;
        private static bool IsVpnAvailable = false;


        static Host Host;

        NotifyIcon notifyIcon;

        private System.Windows.Forms.NotifyIcon m_notifyIcon;

        public HomeWindow()
        {
            InitializeComponent();

            try
            {
             
                //fetch data.bin settings
                Settings.fetchSettings();

                NetworkChange.NetworkAvailabilityChanged +=
                  NetworkAvailabilityChanged;

                if (Settings.getinternetKillSwitch() == true)
                {
                    IsKillSwitch.IsChecked = true;
                }

                MinimizeToTray.Enable(this);

                Closing += OnClosing;

                #region set current  location

                // labelLocation.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                // labelIpAddress.Text = Settings.getCurrentLocation().Ip;
                //loads all Country Data via the constructor (You can initialize this once as a singleton)  
                var helper = new CountryHelper();

                HttpClient client = new HttpClient();


                #endregion

                //if only internet available
                if (NetworkInterface.GetIsNetworkAvailable())
                {


                    #region get location from ip

                    Uri urlLocation = new Uri("https://ipinfo.io/");
                    var locationData = client.GetData(urlLocation);
                    if (locationData != null && locationData != "")
                    {
                        dynamic _locationData = JObject.Parse(locationData);

                        CurrentLocation location = new CurrentLocation();


                        location.Ip = _locationData.ip;
                        location.Host = _locationData.hostname;
                        location.City = _locationData.city;
                        location.Region = _locationData.region;
                        location.Country = _locationData.country;
                        location.Country = helper.GetCountry(location.Country).First().CountryName;

                        String loc = _locationData.loc;
                        var res = loc.Split(',');

                        location.Latitude = Convert.ToDouble(res[0]);
                        location.Longitude = Convert.ToDouble(res[1]);

                        Settings.setCurrentLocation(location);

                    }

                    #endregion

                    Settings.fetchSettings();

                    labelLocation.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                    labelIpAddress.Text = Settings.getCurrentLocation().Ip;


                    Canvas.SetZIndex(connectionPanel, -1);

                    if (Settings.getServer() != null)
                    {
                        countryFlagImage.Source = new BitmapImage(new Uri(Settings.getServer().Flag));
                        currentLocation.Content = Settings.getServer().Name;
                    }
                    else if (Settings.getConnectLastUsedServer())
                    {
                      //  Settings.fetchSettings();
                        
                        var d = Settings.getLastServerUsed();
                        countryFlagImage.Source = new BitmapImage(new Uri(Settings.getLastServerUsed().Flag));
                        currentLocation.Content = Settings.getLastServerUsed().Name;
                    }
                    else
                    {
                        //by default ny server select
                        //  Settings.setServer(Settings.getServers().Where(x => x.Dns == "us-ny-01.serverintoshell.com").FirstOrDefault());

                        //  countryFlagImage.Source = new BitmapImage(new Uri(Settings.getServer() == null ? Settings.ApiUrl + "/flags/us.png" : Settings.getServer().Flag));
                        currentLocation.Content = "Select Server"; //  Settings.getServer() == null ? "New York" : Settings.getServer().Name;
                    }

                    #region news

                    Uri uri = new Uri(Settings.ApiUrl + "news/device/windows");
                    var JsonNews = client.GetData(uri, Settings.AuthToken);
                    if (JsonNews == "401")
                    {
                        Login login = new Login();
                        login.ShowDialog();
                    }

                    if (JsonNews != "")
                    {
                        dynamic _news = JObject.Parse(JsonNews);

                        newsText.Text = _news.data[0].text;
                        newsLinkText.Text = _news.data[0].link_text;
                        newsLink.NavigateUri = _news.data[0].link;
                    }
                    #endregion

                    #region update available

                    Uri updateUri = new Uri(Settings.ApiUrl + "update_available");
                    var jsonUpdate = client.GetData(updateUri, Settings.AuthToken);

                    if (jsonUpdate == "401")
                    {
                        Login login = new Login();
                        login.ShowDialog();
                    }

                    if (jsonUpdate != "")
                    {
                        dynamic _update = JObject.Parse(jsonUpdate);
                        Settings.IsUpdateAvailable = _update.data.is_update_available_windows;
                        if (Settings.IsUpdateAvailable)
                        {
                            Canvas.SetZIndex(updatePanel, 1);
                        }
                    }
                    #endregion

                    #region  Check VPN Connected Already Or not

                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                        var isVpnConnected = interfaces.Where((x => x.NetworkInterfaceType.ToString() == "Ppp" || x.Description.Contains("TAP-Windows") && x.OperationalStatus == OperationalStatus.Up)).FirstOrDefault();
                        if (isVpnConnected != null)
                        {


                            this.Dispatcher.Invoke(() =>
                            {
                                Canvas.SetZIndex(connectionPanel, -1);
                            });

                            IsVpnAvailable = true;

                            this.Dispatcher.Invoke(() =>
                            {

                            #region visibility
                            this.btnConnectBlue.Visibility = Visibility.Hidden;
                            this.btnConnect.Visibility = Visibility.Hidden;
                            this.btnDisConnect.Visibility = Visibility.Visible;
                            #endregion

                                canvasOrange.Opacity = 0;
                                canvasGreen.Opacity = 1;
                                imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));
                                connect_label.Content = "CONNECTED";
                                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));

                                btnConnectBlue.IsEnabled = true;

                                AlreadyConnected = true;

                                IsConnected = true;

                                IsDisconnected = false;

                            });
                        }


                        #region  if user checked on connected last server used when opened app  from options 

                        //  Settings.fetchSettings();

                        if (!IsConnected && Settings.getConnectLastUsedServer())
                        {
                            var d = Settings.getLastServerUsed();
                            Settings.setServer(Settings.getLastServerUsed());
                            this.Dispatcher.Invoke(() =>
                            {
                                btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                            });
                        }

                        #endregion


                    }

                    #endregion



                }
                else
                {
                    Canvas.SetZIndex(connectionPanel, 1);

                    var se = Settings.getServer();

                    if (Settings.getServer() != null)
                    {
                        countryFlagImage.Source = new BitmapImage(new Uri(Settings.getServer().flag, UriKind.Relative));
                        currentLocation.Content = Settings.getServer().Name;
                    }
                    else if (Settings.getConnectLastUsedServer())
                    {
                        countryFlagImage.Source = new BitmapImage(new Uri("pack://application:,,,/assets/" + Settings.getLastServerUsed().Iso.ToLower().ToString() + ".png")); //new BitmapImage(new Uri(Settings.getLastServerUsed().flag, UriKind.Relative));
                        currentLocation.Content = Settings.getLastServerUsed().Name;
                    }
                    else
                    {
                        currentLocation.Content = "Select Server";
                    }

                    //  currentLocation.Content = "No Location Available";
                }
            }
            catch(Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void miQuit_Click()
        {
            // "On-Click" logic here
        }

        private void miShowDebug_Click()
        {
            // "On-Click" logic here
        }


        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {

            //bool? Result = new MessageBoxCustom("Are you sure, You want to close  application ? ", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            //if (Result.Value == true)
            //{
            //    Environment.Exit(Environment.ExitCode);
            //}
            //else
            //{
            //    cancelEventArgs.Cancel = true;
            //}


            if (System.Windows.MessageBox.Show("Do you want to close this application?",
            "Alert", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Environment.Exit(Environment.ExitCode);
            }
            else
            {
                cancelEventArgs.Cancel = true;
            }


        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    break;
                case WindowState.Minimized:
                    this.notifyIcon = new NotifyIcon();
                    notifyIcon.BalloonTipText = "Oyster VPN Application";
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




        private void Location_Button(object sender, RoutedEventArgs e)
        {
            VpnLocations vp = new VpnLocations();
            vp.Owner = this; //System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            vp.Left = vp.Owner.Left < 500 ? vp.Owner.Left + 400 : vp.Owner.Left - 380;  //+  (option.Owner.Width - option.Width) / 2;
            vp.Top = vp.Owner.Top;// + 20;
            vp.ShowDialog();

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Enable "minimize to tray" behavior for this Window
            MinimizeToTray.Enable(this);
        }
      //  private void Window_Activated(object sender, EventArgs e)
      //  {


            //if (ExtensionUserInfo.IsDisConnectVpnExtension == true)
            //{
            //    btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

            //    //prevent for further disconnect 
            //    ExtensionUserInfo.IsDisConnectVpnExtension = false;
            //    ExtensionUserInfo.IsConnectVpnExtension = false;
            //    counter = 0;
            //}

            //else if (ExtensionUserInfo.IsConnectVpnExtension == true)//  && counter == 0)
            //{
            //    btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

            //    //prevent for further connect 
            //    ExtensionUserInfo.IsDisConnectVpnExtension = false;
            //    ExtensionUserInfo.IsConnectVpnExtension = true;
            //    counter = 1;
            //}

            // Title = "Active Window";
       // }

    //    private void Window_Deactivated(object sender, EventArgs e)
     //   {
            //Title = "Inactive Window";
    //    }


        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        System.Windows.Threading.DispatcherTimer dispatcherTimerVPNAuto = new System.Windows.Threading.DispatcherTimer();

        private async  void btnDisConnect_Click(object sender, RoutedEventArgs e)
        {

            //DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(2));

            //canvasGreen.BeginAnimation(Canvas.OpacityProperty, ani);

            //DoubleAnimation ania = new DoubleAnimation(1, TimeSpan.FromSeconds(1));

            //canvasRed.BeginAnimation(Canvas.OpacityProperty, ania);

            try
            {
                this.canvasGreen.Opacity = 0;
                this.canvasOrange.Opacity = 1;
                this.connect_label.Content = "DISCONNECTING...";
                this.connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));

                imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));
                //imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));

                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Remove("OysterVPN Block Internet");

                // System.Windows.Forms.MessageBox.Show("Test");

                DoubleAnimation da = new DoubleAnimation();
                da.From = 0.3;
                da.To = 0;
                da.BeginTime = TimeSpan.FromSeconds(0);
                da.Duration = TimeSpan.FromSeconds(0.5);
                da.AutoReverse = true;
                da.RepeatBehavior = new RepeatBehavior(new TimeSpan(0, 0, 5));

                roundImage4.BeginAnimation(Image.OpacityProperty, da);
                roundImage3.BeginAnimation(Image.OpacityProperty, da);
                roundImage2.BeginAnimation(Image.OpacityProperty, da);
                roundImage1.BeginAnimation(Image.OpacityProperty, da);


                IsConnected = false;

                await Task.Run(() =>

                IsDisconnected = OysterVpn.disconnect());


                IsConnected = false;

                //var notificationManager = new NotificationManager();

                //notificationManager.Show(new NotificationContent
                //{
                //    Title = "notification",
                //    Message = "Disconnected To The Oyster VPN Server",
                //    Type = NotificationType.Error
                //});


                #region visibility

                btnDisConnect.Visibility = Visibility.Hidden;
                btnConnect.Visibility = Visibility.Visible;
                homeBlue.Visibility = Visibility.Hidden;
                homeRed.Visibility = Visibility.Visible;

                imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));
                #endregion

                canvasGreen.Opacity = 0;
                canvasOrange.Opacity = 0;
                canvasRed.Opacity = 1;
                connect_label.Content = "DISCONNECTED";
                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DE1818"));

                labelLocation.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                labelIpAddress.Text = Settings.getCurrentLocation().Ip;

                ExtensionUserInfo.IsDisConnectVpnExtension = false;
                ExtensionUserInfo.IsConnectVpnExtension = false;
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }

            //NetworkChange.NetworkAddressChanged +=
            //NetworkAddressChangedConnect;

        }

        private void btnConnectBlue_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                Settings.fetchSettings();

                if (Settings.getServer() == null)
                {
                    System.Windows.Forms.MessageBox.Show("Select Server Location");
                    return;
                }

                canvasOrange.Opacity = 1;
                connect_label.Content = "CONNECTING...";
                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));
                // imageBlueDisconnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));
                this.btnConnectBlue.Visibility = Visibility.Hidden;
                this.btnDisConnect.Visibility = Visibility.Visible;
                imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));

                //  imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));

                // btnConnectBlue.IsEnabled = false;

                DoubleAnimation da = new DoubleAnimation();
                da.From = 0.3;
                da.To = 0;
                da.BeginTime = TimeSpan.FromSeconds(0);
                da.Duration = TimeSpan.FromSeconds(0.5);
                da.AutoReverse = true;
                da.RepeatBehavior = new RepeatBehavior(new TimeSpan(0, 0, 3));

                roundImage4.BeginAnimation(Image.OpacityProperty, da);
                roundImage3.BeginAnimation(Image.OpacityProperty, da);
                roundImage2.BeginAnimation(Image.OpacityProperty, da);
                roundImage1.BeginAnimation(Image.OpacityProperty, da);

                #region Connection

               
                //   string defaultProtocol = Settings.defaultProtocol;
                var protocol = Settings.getProtocol().Length == 0 ? Settings.defaultProtocol : Settings.getProtocol();

                switch (protocol)
                {
                    case "L2TP":
                        IsConnected = OysterVpn.connect("L2TP", "", "", "");
                        break;
                    case "PPTP":
                        IsConnected = OysterVpn.connect("PPTP", "", "", "");
                        break;
                    case "IKEV2":
                        IsConnected = OysterVpn.connect("IKEV2", "", "", "");
                        break;

                    case "TCP":
                        IsConnected = OysterVpn.connect("TCP", "ca1.ca", "tcp.tls", Settings.getServer() == null ? "us-ny-01.serverintoshell.com 4443" : Settings.getServer().Dns + " " + Settings.getServer().Port);
                        break;
                    case "UDP":
                        IsConnected = OysterVpn.connect("UDP", "ca2.ca", "udp.tls", Settings.getServer() == null ? "us-ny-01.serverintoshell.com 4443" : Settings.getServer().Dns + " " + Settings.getServer().Port);
                        break;
                }


                Settings.setLastUsedServer(Settings.getServer());

                labelLocation.Text = Settings.getServer().City + ", " + Settings.getServer().Country;
                labelIpAddress.Text = Settings.getServer().Ip;


                AlreadyConnected = true;

                #endregion

                //NetworkChange.NetworkAddressChanged +=
                //NetworkAddressChangedConnect;

                //   if (Settings.getinternetKillSwitch())
                {

                    //  NetworkChange.NetworkAvailabilityChanged +=
                    //  NetworkAvailabilityChanged;
                    NetworkChange.NetworkAddressChanged +=
                    NetworkAddressChanged;
                }
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }

        }

        private void NetworkAddressChangedConnect(object sender, EventArgs e)
        {



            if (IsConnected)
            {
                IsDisconnected = false;
                this.Dispatcher.Invoke(() =>
                {



                    //var notificationManager = new NotificationManager();

                    //notificationManager.Show(new NotificationContent
                    //{
                    //    Title = "notification",
                    //    Message = "Connected To The Oyster VPN Server",
                    //    Type = NotificationType.Success
                    //});

                    #region visibility
                    this.btnConnectBlue.Visibility = Visibility.Hidden;
                    this.btnConnect.Visibility = Visibility.Hidden;
                    //  this.btnDisConnect.Visibility = Visibility.Visible;
                    #endregion

                    canvasOrange.Opacity = 0;
                    canvasGreen.Opacity = 1;
                    imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));
                    connect_label.Content = "CONNECTED";
                    connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));

                    btnConnectBlue.IsEnabled = true;
                    AlreadyConnected = true;

                });

            }

            //if (IsDisconnected)
            //{
            //    IsConnected = false;

            //    this.Dispatcher.Invoke(() =>
            //    {

            //        btnDisConnect.IsEnabled = false;


            //        var notificationManager = new NotificationManager();

            //        notificationManager.Show(new NotificationContent
            //        {
            //            Title = "notification",
            //            Message = "Disconnected To The Oyster VPN Server",
            //            Type = NotificationType.Error
            //        });

            //        imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));

            //        #region visibility

            //        btnDisConnect.Visibility = Visibility.Hidden;
            //        btnConnect.Visibility = Visibility.Visible;
            //        homeBlue.Visibility = Visibility.Hidden;
            //        homeRed.Visibility = Visibility.Visible;

            //        #endregion
            //        canvasGreen.Opacity = 0;
            //        canvasOrange.Opacity = 0;
            //        canvasRed.Opacity = 1;
            //        connect_label.Content = "DISCONNECTED";
            //        connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DE1818"));

            //        labelLocation.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
            //        labelIpAddress.Text = Settings.getCurrentLocation().Ip;

            //        ExtensionUserInfo.IsDisConnectVpnExtension = false;
            //        ExtensionUserInfo.IsConnectVpnExtension = false;

            //        btnDisConnect.IsEnabled = true;

            //    });

            //}

        }



        private void btnConnectBlue_ClickOld(object sender, RoutedEventArgs e)
        {

            //INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
            //  Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            //firewallPolicy.Rules.Remove("Block Internet");

            imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));
            canvasOrange.Opacity = 0;
            btnConnectBlue.IsEnabled = false;

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0.3;
            da.To = 0;
            da.BeginTime = TimeSpan.FromSeconds(0);
            da.Duration = TimeSpan.FromSeconds(0.5);
            da.AutoReverse = true;
            da.RepeatBehavior = new RepeatBehavior(new TimeSpan(0, 0, 3));

            roundImage4.BeginAnimation(Image.OpacityProperty, da);
            roundImage3.BeginAnimation(Image.OpacityProperty, da);
            roundImage2.BeginAnimation(Image.OpacityProperty, da);
            roundImage1.BeginAnimation(Image.OpacityProperty, da);

            connect_label.Content = "CONNECTING...";
            connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));


            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(2));

            canvasBlue.BeginAnimation(Canvas.OpacityProperty, ani);

            DoubleAnimation ania = new DoubleAnimation(1, TimeSpan.FromSeconds(1));

            canvasGreen.BeginAnimation(Canvas.OpacityProperty, ania);

            #region Connection 

            //   bool isFetch = Settings.fetchSettings();
            string defaultProtocol = Settings.defaultProtocol;
            bool isConnect = false;

            switch (defaultProtocol)
            {
                case "L2TP":
                    isConnect = OysterVpn.connect("L2TP", "", "", "");
                    break;
                case "PPTP":
                    isConnect = OysterVpn.connect("PPTP", "", "", "");
                    break;
                case "IKEV2":
                    isConnect = OysterVpn.connect("IKEV2", "", "", "");
                    break;

                case "TCP":
                    isConnect = OysterVpn.connect("TCP", "ca1.ca", "tcp.tls", Settings.getServer() == null ? "us-ny-01.serverintoshell.com 4443" : Settings.getServer().Dns + " " + Settings.getServer().Port);
                    break;
                case "UDP":
                    isConnect = OysterVpn.connect("UDP", "ca2.ca", "udp.tls", Settings.getServer() == null ? "us-ny-01.serverintoshell.com 4443" : Settings.getServer().Dns + " " + Settings.getServer().Port);
                    break;
            }

            Settings.setLastUsedServer(Settings.getServer());

            labelLocation.Text = Settings.getServer().City + ", " + Settings.getServer().Country;
            labelIpAddress.Text = Settings.getServer().Ip;

            #endregion

            btnConnectBlue.IsEnabled = true;

            //  if (isConnect)
            // {

            //var notificationManager = new NotificationManager();

            //notificationManager.Show(new NotificationContent
            //{
            //    Title = "notification",
            //    Message = "Connected To The Oyster VPN Server",
            //    Type = NotificationType.Success
            //});

            #region visibility
            btnConnectBlue.Visibility = Visibility.Hidden;
            btnConnect.Visibility = Visibility.Hidden;
            //  homeBlue.Visibility = Visibility.Hidden;
            // homeRed.Visibility = Visibility.Hidden;
            // homeGreen.Visibility = Visibility.Visible;
            btnDisConnect.Visibility = Visibility.Visible;
            #endregion

            connect_label.Content = "CONNECTED";
            connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));
            //}
            //else
            //{
            //    canvasOrange.Opacity = 1;
            //    connect_label.Content = "CONNECTING...";
            //    connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));

            //    imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));
            //    btnConnectBlue.Visibility = Visibility.Hidden;
            //    btnConnect.Visibility = Visibility.Hidden;
            //    btnDisConnect.Visibility = Visibility.Visible;
            //}

            btnConnectBlue.IsEnabled = true;
            AlreadyConnected = true;

            //if internet kill switch is enable 
            //if (Settings.getinternetKillSwitch())
            //{

            //    //  Add the handlers to the NetworkChange events.
            //    // start listening for a network change

            //    NetworkChange.NetworkAvailabilityChanged +=
            //     NetworkAvailabilityChanged;
            //    NetworkChange.NetworkAddressChanged +=
            //    NetworkAddressChanged;


            //}
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                Settings.fetchSettings();

                // btnConnect.IsEnabled = false;

                connect_label.Content = "CONNECTING...";
                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));

                //INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                //  Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                //firewallPolicy.Rules.Remove("Block Internet");
                this.btnConnect.Visibility = Visibility.Hidden;
                this.btnConnectBlue.Visibility = Visibility.Hidden;
                this.btnDisConnect.Visibility = Visibility.Visible;
                imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));
                //   imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));
                canvasOrange.Opacity = 1;
                //  btnConnect.IsEnabled = false;
                DoubleAnimation da = new DoubleAnimation();
                da.From = 0.3;
                da.To = 0;
                da.BeginTime = TimeSpan.FromSeconds(0);
                da.Duration = TimeSpan.FromSeconds(0.5);
                da.AutoReverse = true;
                da.RepeatBehavior = new RepeatBehavior(new TimeSpan(0, 0, 3));

                roundImage4.BeginAnimation(Image.OpacityProperty, da);
                roundImage3.BeginAnimation(Image.OpacityProperty, da);
                roundImage2.BeginAnimation(Image.OpacityProperty, da);
                roundImage1.BeginAnimation(Image.OpacityProperty, da);

                //DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(2));

                //canvasRed.BeginAnimation(Canvas.OpacityProperty, ani);
                ////  imageRedDisconnected.BeginAnimation(ImageBrush.OpacityProperty, new DoubleAnimation(0, TimeSpan.FromSeconds(3)));

                //DoubleAnimation ania = new DoubleAnimation(1, TimeSpan.FromSeconds(1));

                //canvasGreen.BeginAnimation(Canvas.OpacityProperty, ania);

                ////  imageGreenConnected.BeginAnimation(ImageBrush.OpacityProperty, new DoubleAnimation(1, TimeSpan.FromSeconds(3)));

                #region Connection 

                //if internet kill switch is enable 
                //if (Settings.getinternetKillSwitch())
                //{
                //    OysterVpn.DisableKillSwitch();
                //}
                //   bool isFetch = Settings.fetchSettings();
             //   string defaultProtocol = Settings.defaultProtocol;
                var protocol = Settings.getProtocol().Length == 0 ? Settings.defaultProtocol : Settings.getProtocol();

                switch (protocol)
                {
                    case "L2TP":
                        IsConnected = OysterVpn.connect("L2TP", "", "", "");
                        break;
                    case "PPTP":
                        IsConnected = OysterVpn.connect("PPTP", "", "", "");
                        break;
                    case "IKEV2":
                        IsConnected = OysterVpn.connect("IKEV2", "", "", "");
                        break;

                    case "TCP":
                        IsConnected = OysterVpn.connect("TCP", "ca1.ca", "tcp.tls", Settings.getServer() == null ? "us-ny-01.serverintoshell.com 4443" : Settings.getServer().Dns + " " + Settings.getServer().Port);
                        break;
                    case "UDP":
                        IsConnected = OysterVpn.connect("UDP", "ca2.ca", "udp.tls", Settings.getServer() == null ? "us-ny-01.serverintoshell.com 4443" : Settings.getServer().Dns + " " + Settings.getServer().Port);
                        break;
                }

                labelLocation.Text = Settings.getServer().City + ", " + Settings.getServer().Country;
                labelIpAddress.Text = Settings.getServer().Ip;


                Settings.setLastUsedServer(Settings.getServer());

                #endregion

                //btnConnect.IsEnabled = true;

                //var notificationManager = new NotificationManager();

                //notificationManager.Show(new NotificationContent
                //{
                //    Title = "notification",
                //    Message = "Connected To The Oyster VPN Server",
                //    Type = NotificationType.Success
                //});


                //#region visibility
                //canvasGreen.Opacity = 100;
                //btnConnectBlue.Visibility = Visibility.Hidden;
                //btnConnect.Visibility = Visibility.Hidden;
                //homeBlue.Visibility = Visibility.Hidden;
                //// homeRed.Visibility = Visibility.Hidden;
                //homeGreen.Visibility = Visibility.Visible;
                //btnDisConnect.Visibility = Visibility.Visible;
                //#endregion

                //connect_label.Content = "CONNECTED";
                //connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));

                AlreadyConnected = true;

                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Google\Chrome\NativeMessagingHosts\com.oystertech.vpn");

                //storing the values  
                key.SetValue("IsConnected", "1");
                key.Close();

                // NetworkChange.NetworkAddressChanged +=
                //NetworkAddressChangedConnect;

                //if (Settings.getinternetKillSwitch())
                {

                    //    //  Add the handlers to the NetworkChange events.
                    //    // start listening for a network change
                    //  NetworkChange.NetworkAvailabilityChanged +=
                    //    NetworkAvailabilityChanged;
                    NetworkChange.NetworkAddressChanged +=
                    NetworkAddressChanged;
                }

            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }



        // Declare a method to handle NetworkAvailabilityChanged events.
        private void NetworkAvailabilityChanged(
        object sender, NetworkAvailabilityEventArgs e)
        {
            // Report whether the network is now available or unavailable.
            if (e.IsAvailable)
            {

                this.Dispatcher.Invoke(() =>
                {
                    Canvas.SetZIndex(updatePanel, 1);
                    Canvas.SetZIndex(connectionPanel, -1);
                });

                IsNetworkAvailable = true;
                Console.WriteLine("Network Available");

                //INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                //    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                //firewallPolicy.Rules.Remove("OysterVPN Block Internet");

                if (Settings.getinternetKillSwitch() && IsDisconnected)
                {

                    IsBlockedFirewall = false;
                    INetFwPolicy2 _firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    _firewallPolicy.Rules.Remove("OysterVPN Block Internet");

                    IsConnected = true;


                    this.Dispatcher.Invoke(() =>
                    {
                        btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                    });

                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {

                    Canvas.SetZIndex(updatePanel, -1);
                    Canvas.SetZIndex(connectionPanel, 1);

                });

                Console.WriteLine("Network Unavailable");

                if (Settings.getinternetKillSwitch() && IsConnected)
                {

                    INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FWRule"));
                    firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                    firewallRule.Description = "Used to block all internet access.";
                    firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                    firewallRule.Enabled = true;
                    firewallRule.InterfaceTypes = "All";
                    firewallRule.Name = "OysterVPN Block Internet";

                    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    firewallPolicy.Rules.Add(firewallRule);
                    IsBlockedFirewall = true;

                    this.Dispatcher.Invoke(() =>
                    {

                        IsDisconnected = true;
                        IsConnected = false;

                        connect_label.Content = "CONNECTING...";
                        connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9A000"));

                        AlreadyConnected = false;

                        canvasOrange.Opacity = 100;
                        imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));

                    });

                }
                IsNetworkAvailable = false;
                IsConnected = false;
            }
        }


        public static async Task<bool> IsConnectedToInternetAsync()
        {
            const int maxHops = 30;
            const string someFarAwayIpAddress = "8.8.8.8";

            // Keep pinging further along the line from here to google 
            // until we find a response that is from a routable address
            for (int ttl = 1; ttl <= maxHops; ttl++)
            {
                var options = new PingOptions(ttl, true);
                byte[] buffer = new byte[32];
                PingReply reply;
                try
                {
                    using (var pinger = new Ping())
                    {
                        reply = await pinger.SendPingAsync(someFarAwayIpAddress, 10000, buffer, options);
                    }
                }
                catch (PingException pingex)
                {
                    Debug.Print($"Ping exception (probably due to no network connection or recent change in network conditions), hence not connected to internet. Message: {pingex.Message}");
                    return false;
                }

                string address = reply.Address?.ToString() ?? null;
                Debug.Print($"Hop #{ttl} is {address}, {reply.Status}");

                if (reply.Status != IPStatus.TtlExpired && reply.Status != IPStatus.Success)
                {
                    Debug.Print($"Hop #{ttl} is {reply.Status}, hence we are not connected.");
                    return false;
                }

                if (IsRoutableAddress(reply.Address))
                {
                    Debug.Print("That's routable, so we must be connected to the internet.");
                    return true;
                }
            }

            return false;
        }

        private static bool IsRoutableAddress(IPAddress addr)
        {
            if (addr == null)
            {
                return false;
            }
            else if (addr.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return !addr.IsIPv6LinkLocal && !addr.IsIPv6SiteLocal;
            }
            else // IPv4
            {
                byte[] bytes = addr.GetAddressBytes();
                if (bytes[0] == 10)
                {   // Class A network
                    return false;
                }
                else if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
                {   // Class B network
                    return false;
                }
                else if (bytes[0] == 192 && bytes[1] == 168)
                {   // Class C network
                    return false;
                }
                else
                {   // None of the above, so must be routable
                    return true;
                }
            }
        }

        public static bool IsAvailableNetworkActive()
        {
            // only recognizes changes related to Internet adapters
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                // however, this will include all adapters -- filter by opstatus and activity
                NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                return (from face in interfaces
                        where face.OperationalStatus == OperationalStatus.Up && face.Speed > 0
                        where (face.NetworkInterfaceType != NetworkInterfaceType.Tunnel) && (face.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        select face.GetIPv4Statistics()).Any(statistics => (statistics.BytesReceived > 0) && (statistics.BytesSent > 0));
            }

            return false;
        }

        private void NetworkAddressChanged(object sender, EventArgs e)
        {

            #region KILL SWITCH ONLY

            //for kill swith purpose only
            if (Settings.getinternetKillSwitch())
            {

                //if (!IsAvailableNetworkActive())
                //{
                //    INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                //     Type.GetTypeFromProgID("HNetCfg.FWRule"));
                //    firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                //    firewallRule.Description = "Used to block all internet access.";
                //    firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                //    firewallRule.Enabled = true;
                //    firewallRule.InterfaceTypes = "All";
                //    firewallRule.Name = "OysterVPN Block Internet";

                //    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                //    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                //    firewallPolicy.Rules.Add(firewallRule);
                //    IsBlockedFirewall = true;

                //    this.Dispatcher.Invoke(() =>
                //    {

                //        IsDisconnected = false;
                //        IsConnected = false;

                //        connect_label.Content = "CONNECTING...";
                //        connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9A000"));

                //        AlreadyConnected = false;

                //        canvasOrange.Opacity = 100;
                //        imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));

                //    });


                //    //   btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

                //    //connect_label.Content = "CONNECTING...";
                //    //connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9A000"));

                //}

                //else
                //{
                //    if (!AlreadyConnected && IsNetworkAvailable && IsVpnAvailable)
                //    {
                //        IsBlockedFirewall = false;
                //        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                //        Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                //        firewallPolicy.Rules.Remove("OysterVPN Block Internet");

                //        IsConnected = true;


                //        this.Dispatcher.Invoke(() =>
                //        {
                //            btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                //        });

                //    }
                //}


                //////////////////////////////////////////////////////////////////////////////////////
                ////
                /////
                /////
                //else
                //{


                //    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                //     Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                //    firewallPolicy.Rules.Remove("Block Internet");

                //    string pattern = @"(ethernet|wi-?fi)";

                //    if (IsBlockedFirewall)
                //    {

                //        this.Dispatcher.Invoke(() =>
                //        {

                //            connect_label.Content = "CONNECTING...";
                //            connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9A000"));

                //        });

                //        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                //        {
                //            System.Windows.Forms.MessageBox.Show(nic.NetworkInterfaceType.ToString().ToLower());

                //            Match m = Regex.Match(nic.NetworkInterfaceType.ToString().ToLower(), pattern, RegexOptions.IgnoreCase);

                //                if (m.Success)
                //                // if (nic.NetworkInterfaceType.ToString().Contains("Ethernet") || nic.NetworkInterfaceType.ToString().Contains("Wi-?fi"))
                //                {

                //                this.Dispatcher.Invoke(() =>
                //                {
                //                    btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                //                });

                //                break;
                //                }
                //         }
                //    }

                //    IsBlockedFirewall = false;

                //}


                //try
                //{

                //    var checkInternetConnection = new Ping().Send("www.google.com").Status == IPStatus.Success;
                //   if (checkInternetConnection && IsNetworkAvailable && !AlreadyConnected)
                //    {

                //        this.Dispatcher.Invoke(() =>
                //        {
                //            btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                //        });
                //        //this.Dispatcher.Invoke(() =>
                //        //{

                //        //    connect_label.Content = "CONNECTED";
                //        //    connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));

                //        //});
                //    }
                //}
                //catch (Exception ex)
                //{
                //    AlreadyConnected = false;
                //    this.Dispatcher.Invoke(() =>
                //    {
                //        IsDisconnected = false;
                //        IsConnected = false;


                //        connect_label.Content = "CONNECTING...";
                //        connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));

                //        canvasOrange.Opacity = 100;
                //        imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));

                //    });
                //}
            }

            #endregion

            #region Check if vpn connected or not  
            if (IsAvailableNetworkActive())
            {
                if (IsConnected)
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                        var isVpnConnected = interfaces.Where((x => x.NetworkInterfaceType.ToString() == "Ppp" || x.Description.Contains("TAP-Windows Adapter") && x.OperationalStatus == OperationalStatus.Up)).FirstOrDefault();
                        if (isVpnConnected == null)
                        {
                            IsVpnAvailable = false;
                            this.Dispatcher.Invoke(() =>
                            {

                                btnDisConnect.Visibility = Visibility.Hidden;
                                btnConnect.Visibility = Visibility.Visible;
                                homeBlue.Visibility = Visibility.Hidden;
                                homeRed.Visibility = Visibility.Visible;

                                canvasGreen.Opacity = 0;
                                canvasOrange.Opacity = 0;
                                canvasRed.Opacity = 1;
                                connect_label.Content = "DISCONNECTED";
                                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DE1818"));

                                labelLocation.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                                labelIpAddress.Text = Settings.getCurrentLocation().Ip;

                                ExtensionUserInfo.IsDisConnectVpnExtension = false;
                                ExtensionUserInfo.IsConnectVpnExtension = false;

                                //IsDisconnected = false;
                                //IsConnected = false;

                                //connect_label.Content = "CONNECTING...";
                                //connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9A000"));

                                //AlreadyConnected = false;

                                //canvasOrange.Opacity = 100;
                                //imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));


                            });
                        }
                        else
                        {

                            this.Dispatcher.Invoke(() =>
                            {
                                Canvas.SetZIndex(connectionPanel, -1);
                            });

                            IsVpnAvailable = true;

                            this.Dispatcher.Invoke(() =>
                            {



                                //var notificationManager = new NotificationManager();

                                //notificationManager.Show(new NotificationContent
                                //{
                                //    Title = "notification",
                                //    Message = "Connected To The Oyster VPN Server",
                                //    Type = NotificationType.Success
                                //});

                                #region visibility
                                this.btnConnectBlue.Visibility = Visibility.Hidden;
                                this.btnConnect.Visibility = Visibility.Hidden;
                                this.btnDisConnect.Visibility = Visibility.Visible;
                                #endregion

                                canvasOrange.Opacity = 0;
                                canvasGreen.Opacity = 1;
                                imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connected-green.png"));
                                connect_label.Content = "CONNECTED";
                                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));

                                btnConnectBlue.IsEnabled = true;

                                AlreadyConnected = true;

                                IsConnected = true;

                                IsDisconnected = false;

                            });
                        }
                        //foreach (NetworkInterface Interface in interfaces)
                        //{
                        //    if (Interface.OperationalStatus == OperationalStatus.Up)
                        //    {
                        //        if ((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                        //        {
                        //            //  IPv4InterfaceStatistics statistics = Interface.GetIPv4Statistics();
                        //            //  MessageBox.Show(Interface.Name + " " + Interface.NetworkInterfaceType.ToString() + " " + Interface.Description);
                        //        }
                        //        else
                        //        {

                        //            this.Dispatcher.Invoke(() =>
                        //            {

                        //                IsDisconnected = false;
                        //                IsConnected = false;

                        //                connect_label.Content = "CONNECTING...";
                        //                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9A000"));

                        //                AlreadyConnected = false;

                        //                canvasOrange.Opacity = 100;
                        //                imageGreenConnected.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/button-connecting.png"));


                        //            });

                        //            break;

                        //            //.Show("VPN Connection is lost!");
                        //        }
                        //    }
                        //}
                    }
                     
                }

            }



            #endregion


        }



        // private static string NetworkInterfaceType = "";
        // Declare a method to handle NetworkAdressChanged events.
        private static void NetworkAddressChangedss(object sender, EventArgs e)
        {



            //  Console.WriteLine("Current IP Addresses:");
            // Iterate through the interfaces and display information.
            //foreach (NetworkInterface ni in  NetworkInterface.GetAllNetworkInterfaces())
            // {
            var adapters = NetworkInterface.GetAllNetworkInterfaces().Where(x => x.NetworkInterfaceType.ToString().Contains("Ethernet") || x.NetworkInterfaceType.ToString().Contains("Wifi")).ToList();

            //if (adapters.NetworkInterfaceType.ToString().Contains("Ethernet") || adapters.NetworkInterfaceType.ToString().Contains("Wifi"))
            if (adapters.Count() > 0)
            {


                //  INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                //Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                //  firewallPolicy.Rules.Remove("Block Internet");

                //string value = adapters.FirstOrDefault().NetworkInterfaceType.ToString();
                //NetworkInterfaceType = value;
                //ProcessStartInfo info = new ProcessStartInfo();
                //info.FileName = "cmd.exe";
                //info.Arguments = "/c netsh interface set interface \"" + value + "\" ENABLED";
                //info.WindowStyle = ProcessWindowStyle.Hidden;
                //Process.Start(info);



                //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                //    {

                //        string value = nic.Name;
                //        ProcessStartInfo info = new ProcessStartInfo();
                //        info.FileName = "cmd.exe";
                //        info.Arguments = "/c netsh interface set interface \"" + value + "\" ENABLED";
                //        info.WindowStyle = ProcessWindowStyle.Hidden;
                //        Process.Start(info);

                //        //connect_label.Content = "CONNECTED";
                //        //connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));

                //    }

            }
            else
            {



                //IPHostEntry host;
                //var vpn = NetworkInterface.GetAllNetworkInterfaces();
                //host = Dns.GetHostEntry(Dns.GetHostName()); //This gets what your current IP Address is
                //foreach (IPAddress ip in host.AddressList)
                //{
                //    if (ip.AddressFamily.ToString() == "InterNetwork")
                //    {
                //    }
                //}

                //connect_label.Content = "CONNECTING...";
                //connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9A000"));
                ////Kill all internet adapters

                // foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                //   {
                //var ad = NetworkInterface.GetAllNetworkInterfaces();
                //string value = NetworkInterfaceType;
                //        //nic.Name;
                //        // Process.Start("cmd.exe", "/c netsh interface set interface \"" + value + "\" DISABLED");
                //    ProcessStartInfo info = new ProcessStartInfo();
                //    info.FileName = "cmd.exe";
                //    info.Arguments = "/c netsh interface set interface \"" + value + "\" DISABLED";
                //    info.WindowStyle = ProcessWindowStyle.Hidden;
                //    Process.Start(info);
                //  }
            }

            //foreach (UnicastIPAddressInformation addr
            //in ni.GetIPProperties().UnicastAddresses)
            //{
            //    System.Windows.Forms.MessageBox.Show(addr.Address.ToString());
            //    //Console.WriteLine(" - {0} (lease expires {1})",
            //    //addr.Address, DateTime.Now +
            //    //new TimeSpan(0, 0, (int)addr.DhcpLeaseLifetime));
            //}
            //   }
        }

        private void dispatherTimerToConnectVPNAuto(object sender, EventArgs e)
        {

            if (IsBlockedFirewall)
            {
                connect_label.Content = "CONNECTING...";
                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType.ToString().Contains("Ethernet") || nic.NetworkInterfaceType.ToString().Contains("Wifi"))
                    {
                        btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                        dispatcherTimerVPNAuto.Stop();
                        break;
                    }
                }

            }

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //InternetKillSwitch.Start();

            if (IsBlockedFirewall)
            {
                connect_label.Content = "CONNECTING...";
                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType.ToString().Contains("Ethernet") || nic.NetworkInterfaceType.ToString().Contains("Wifi"))
                    {
                        btnConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                        dispatcherTimer.Stop();
                        break;
                    }
                }

            }
            //else
            //{
            //    connect_label.Content = "CONNECTED";
            //    connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));
            //}

            //        var checkInternetConnection = new Ping().Send("www.google.com").Status == IPStatus.Success;
            //        if (!checkInternetConnection)
            //        {
            //            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
            // Type.GetTypeFromProgID("HNetCfg.FWRule"));
            //            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            //            firewallRule.Description = "Used to block all internet access.";
            //            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            //            firewallRule.Enabled = true;
            //            firewallRule.InterfaceTypes = "All";
            //            firewallRule.Name = "Block Internet";

            //            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
            //Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            //            firewallPolicy.Rules.Add(firewallRule);

            //           // dispatcherTimer.Stop();
            //        }
            //        else
            //        {
            //            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
            //Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            //            firewallPolicy.Rules.Remove("Block Internet");

            //         //   dispatcherTimer.Stop();
            //        }


            //  checkVPN();
        }

        private void checkVPN()
        {


            var vpn = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => x.Name != null && x.Name == "OysterVPN");
            var _ip = vpn == null ? "0.0.0.0" : vpn.GetIPProperties().UnicastAddresses.First(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Address.ToString();

            var adapters = NetworkInterface.GetAllNetworkInterfaces();


            vpnON = false;
            IPHostEntry host;
            string localIP = "?"; ;
            host = Dns.GetHostEntry(Dns.GetHostName()); //This gets what your current IP Address is
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                    if (ip.ToString() == _ip)  //Returns true 
                    {
                        vpnON = true;

                        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                        {

                            string value = nic.Name;
                            ProcessStartInfo info = new ProcessStartInfo();
                            info.FileName = "cmd.exe";
                            info.Arguments = "/c netsh interface set interface \"" + value + "\" ENABLED";
                            info.WindowStyle = ProcessWindowStyle.Hidden;
                            Process.Start(info);
                        }

                    }
                    //if internet adapter enable
                    else if (_ip == "0.0.0.0")
                    {

                        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                        {
                            if (nic.NetworkInterfaceType.ToString().Contains("Ethernet") || nic.NetworkInterfaceType.ToString().Contains("Wifi"))
                            {

                                foreach (var adapter in adapters)
                                {
                                    string value = nic.Name;

                                    ProcessStartInfo info = new ProcessStartInfo();
                                    info.FileName = "cmd.exe";
                                    info.Arguments = "/c netsh interface set interface \"" + adapter.Name + "\" ENABLED";
                                    info.WindowStyle = ProcessWindowStyle.Hidden;
                                    Process.Start(info);

                                    connect_label.Content = "CONNECTED";
                                    connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B939"));


                                    vpnON = true;

                                }

                            }
                        }
                    }
                }
            }

            if (vpnON == false)
            {

                connect_label.Content = "CONNECTING...";
                connect_label.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC64715"));
                //Kill all internet adapters

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    string value = nic.Name;
                    // Process.Start("cmd.exe", "/c netsh interface set interface \"" + value + "\" DISABLED");
                    ProcessStartInfo info = new ProcessStartInfo();
                    info.FileName = "cmd.exe";
                    info.Arguments = "/c netsh interface set interface \"" + value + "\" DISABLED";
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(info);
                }
            }
        }

        private void MenuItem_VPNLocation_Click(object sender, RoutedEventArgs e)
        {

            VpnLocations vp = new VpnLocations();
            vp.Owner = this;// System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            vp.Left = vp.Owner.Left < 500 ? vp.Owner.Left + 400 : vp.Owner.Left - 380;  //+  (option.Owner.Width - option.Width) / 2;
            vp.Top = vp.Owner.Top;// + 20;
            vp.ShowDialog();

        }

        private void MenuItem_SpeedTest_Click(object sender, RoutedEventArgs e)
        {
            SpeedTest sp = new SpeedTest();
            sp.Show();
        }

        private void MenuItem_Options_Click(object sender, RoutedEventArgs e)
        {
            Options option = new Options();
            option.Owner = this; //System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            option.Left = option.Owner.Left < 500 ? option.Owner.Left + 400 : option.Owner.Left - 390;  //+  (option.Owner.Width - option.Width) / 2;
            option.Top = option.Owner.Top;// + 20;
            option.ShowDialog();
        }

        private void MenuItem_HelpSupportClick(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_ContactSupport_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string navigateUri = "http://support.oystervpn.co/?utm_source=app&utm_medium=windows&utm_campaign=support";

                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void MenuItem_IpAddressChecker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SiteUrl + "what-is-my-ip?utm_source=app&utm_medium=windows&utm_campaign=ip";

                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void MenuItem_DnsLeakTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SiteUrl + "dns-leak-test?utm_source=app&utm_medium=windows&utm_campaign=dns";

                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void MenuItem_AboutOysterVpn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SiteUrl + "about?utm_source=app&utm_medium=windows&utm_campaign=about";

                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private  void MenuItem_Quit_Click(object sender, RoutedEventArgs e)
        {

            //bool? Result = new MessageBoxCustom("Are you sure, You want to close  application ? ", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            //if (Result.Value == true)
            //{
            //    OysterVpn.disconnect();
            //    Environment.Exit(Environment.ExitCode);
            //}

            if (System.Windows.MessageBox.Show("Do you want to close this application?",
           "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

             //   await Task.Run(() => OysterVpn.disconnect());
            
              
                Environment.Exit(Environment.ExitCode);

            }


        }

        private void SplitTunnel_Click(object sender, RoutedEventArgs e)
        {
            SplitTunnel sp = new SplitTunnel();
            sp.Owner = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            sp.Left = sp.Owner.Left < 500 ? sp.Owner.Left + 400 : sp.Owner.Left - 370;  //+  (option.Owner.Width - option.Width) / 2;
            sp.Top = sp.Owner.Top;// + 20;
            sp.Show();
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

        private void IsKillSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (IsKillSwitch.IsChecked)
            {
                Settings.setinternetKillSwitch(true);

                //    OysterVpn.ProtectDNSLeak();
                //   OysterVpn.KillSwitchWindow();
            }
            else
            {
                Settings.setinternetKillSwitch(false);

                //remove firewall rule also
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Remove("OysterVPN Block Internet");
                IsBlockedFirewall = false;
                //    OysterVpn.UnprotectDNSLeak();
                //  OysterVpn.DisableKillSwitch();
            }
        }


        //private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        //{
        //   // Process.Start(new ProcessStartInfo(e.Uri.OriginalString) { UseShellExecute = true });
        //    Process.Start(new ProcessStartInfo(e.Uri.OriginalString));
        //    e.Handled = true;
        //}

        private void newsLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hl = (Hyperlink)sender;

            string navigateUri = hl.NavigateUri.ToString();

            Process.Start(new ProcessStartInfo(navigateUri));

            e.Handled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SecureDevice secure = new SecureDevice();
            secure.Owner = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            secure.Left = secure.Owner.Left < 500 ? secure.Owner.Left + 400 : secure.Owner.Left - 390;
            secure.Top = secure.Owner.Top;
            secure.Show();
        }

        private void MenuItem_Emailus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SiteUrl + "contact-us?utm_source=app&utm_medium=windows&utm_campaign=contact";

                Process.Start(new ProcessStartInfo(navigateUri));
            }

            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void newsLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string navigateUri = newsLink.NavigateUri.ToString();

                Process.Start(new ProcessStartInfo(navigateUri));

                e.Handled = true;
            }
            catch(Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            updatePanel.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SiteUrl + "download/windows-vpn/";

                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void ConnectionPanelButton_Click(object sender, RoutedEventArgs e)
        {
            connectionPanel.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Logout_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (System.Windows.MessageBox.Show("Do you want to logout?",
              "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    HttpClient client = new HttpClient();

                    var data = client.PostData(Settings.ApiUrl + "logout", null, Settings.AuthToken);

                    Settings.setToken("");

                    this.Hide();
                    CloseAllWindows();
                    //Window mainWindow = Application.Current.MainWindow;
                    //mainWindow.Close();
                    Login login = new Login();
                    login.ShowDialog();
                    this.Close();
                }
                else
                {
                }
            }
            catch(Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
          
        }

        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Hide();
        }


        private void MenuItem_PasswordGenerator_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SiteUrl + "password-generator/";

                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }
    }
}
