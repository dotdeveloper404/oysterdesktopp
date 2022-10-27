using App;
using CountryData.Standard;
using log4net;
using Microsoft.Win32;
using NativeMessaging;
using NetFwTypeLib;
using Newtonsoft.Json.Linq;
using OysterVPNLibrary;
using OysterVPNModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace OysterVPN
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
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
        private static bool IsConnecting = false;

        private Stopwatch _stopWatch;
        private Timer  _timer;
        private const string _startTimeDisplay = "";

        static Host Host;

        System.Windows.Forms.NotifyIcon notifyIcon;

        private System.Windows.Forms.NotifyIcon m_notifyIcon;

        public void HomeOld()
        {
            InitializeComponent();

            try
            {
                listViewSmart.Items.Clear();

                MinimizeToTray.Enable(this);

                Closing += OnClosing;

                //if only internet available
                if (NetworkInterface.GetIsNetworkAvailable())
                {

                    Settings.fetchSettings();

                    #region Recent Location

                    List<Server> recentServer = new List<Server>();
                    if (Settings.getLastServerUsed() != null)
                    {
                        recentServer.Add(new Server()
                        {
                            Id = Settings.getLastServerUsed().Id,
                            Name = Settings.getLastServerUsed().Name,
                            Flag = Settings.getLastServerUsed().Flag,
                            Dns = Settings.getLastServerUsed().Dns,
                            Ip = Settings.getLastServerUsed().Ip,
                            Port = Settings.getLastServerUsed().Port,
                            DnsWithPort = Settings.getLastServerUsed().Dns + " " + Settings.getLastServerUsed().Port,
                            IsFavourited = Settings.getLastServerUsed().IsFavourited,
                            Continent = Settings.getLastServerUsed().Continent,
                        });

                        listViewRecent.ItemsSource = recentServer.ToList();


                    }

                    #endregion

                    //   List<Server> list = (List<Server>)listViewRecomend.ItemsSource;
                    //  LoginResponse response = new LoginResponse();

                    HttpClient client = new HttpClient();
                    Uri uri = new Uri(Settings.ApiUrl + "servers?orderby=id&orderdir=DESC");
                    var JsonServers = client.GetData(uri, Settings.AuthToken);
                    if (JsonServers == "401")
                    {
                        CloseAllWindows();
                        LoginForm login = new LoginForm();
                        login.ShowDialog();
                    }

                    dynamic _servers = JObject.Parse(JsonServers);

                    List<Server> servers = new List<Server>();

                    List<double> distances = new List<double>();
                    var currentLocation = Settings.getCurrentLocation();
                    double latitude = 0;
                    double longitude = 0;
                    if (Settings.getCurrentLocation() != null)
                    {
                        latitude = Settings.getCurrentLocation().Latitude;
                        longitude = Settings.getCurrentLocation().Longitude;
                    } 
                    foreach (var item in _servers.data)
                    {
                        double distance = SmartConnectDistance.distance(latitude, longitude, Convert.ToDouble(item.latitude), Convert.ToDouble(item.longitude), 'M');
                        distances.Add(distance);

                        servers.Add(new Server()
                        {
                            Id = item.server_id,
                            Name = item.name,
                            Flag = item.flag,
                            City = item.city != null ? item.city.name : "",
                            Country = item.country != null ? item.country.name : "",
                            Dns = item.dns,
                            Ip = item.ip_address,
                            Port = item.port,
                            DnsWithPort = item.dns + " " + item.port,
                            IsFavourited = item.is_favourited,
                            Continent = item.continent,
                            Latitude = Convert.ToDouble(item.latitude),
                            Longitude = Convert.ToDouble(item.longitude),
                            Distance = distance,
                            Iso = item.iso
                        });

                    }

                    Settings.setServers(servers.ToArray());

                    var serversList = Settings.getServers();

                    listViewRecomend.ItemsSource = serversList.ToList();

                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewRecomend.ItemsSource);
                    view.Filter = UserFilter;


                    listViewAllLocations.ItemsSource = serversList.ToList();

                    CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(listViewAllLocations.ItemsSource);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Continent");
                    view2.GroupDescriptions.Add(groupDescription);

                    view2.Filter = UserFilter;

                    var shortestDistance = distances.Min();
                    var getShortestValue = servers.Where(x => x.Distance == shortestDistance).FirstOrDefault();
                    listViewSmart.Items.Add(getShortestValue);


                    #region news

                    Uri _uri = new Uri(Settings.ApiUrl + "news/device/windows");
                    var JsonNews = client.GetData(_uri, Settings.AuthToken);
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
                        LoginForm login = new LoginForm();
                        login.ShowDialog();
                    }

                    if (jsonUpdate != "")
                    {
                        dynamic _update = JObject.Parse(jsonUpdate);
                        Settings.IsUpdateAvailable = _update.data.is_update_available_windows;
                        if (Settings.IsUpdateAvailable)
                        {
                            updatePanel.Visibility = Visibility.Visible;
                            //Canvas.SetZIndex(updatePanel, 1);
                        }
                        else
                        {
                            updatePanel.Visibility = Visibility.Hidden;
                            //Canvas.SetZIndex(updatePanel, -1);
                        }
                    }
                    #endregion

                }
                else
                {
                    List<Server> servers = new List<Server>();

                    List<double> distances = new List<double>();
                    Settings.fetchSettings();
                    var s = Settings.getServer();
                    var serversList = Settings.getServers().ToList();

                    foreach (var item in serversList)
                    {
                        double distance = SmartConnectDistance.distance(Settings.getCurrentLocation().Latitude, Settings.getCurrentLocation().Longitude, Convert.ToDouble(item.Latitude), Convert.ToDouble(item.Longitude), 'M');
                        distances.Add(distance);

                        servers.Add(new Server()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Flag = item.flag,
                            City = item.City,
                            Country = item.Country,
                            Dns = item.Dns,
                            Ip = item.Ip,
                            Port = item.Port,
                            DnsWithPort = item.Dns + " " + item.Port,
                            IsFavourited = item.IsFavourited,
                            Continent = item.Continent,
                            Latitude = Convert.ToDouble(item.Latitude),
                            Longitude = Convert.ToDouble(item.Longitude),
                            Distance = distance,
                            Iso = item.Iso
                        });

                    }



                    listViewRecomend.ItemsSource = serversList.ToList();

                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewRecomend.ItemsSource);
                    view.Filter = UserFilter;

                    listViewAllLocations.ItemsSource = serversList.ToList();

                    CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(listViewAllLocations.ItemsSource);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Continent");
                    view2.GroupDescriptions.Add(groupDescription);

                    view2.Filter = UserFilter;


                    var shortestDistance = distances.Min();
                    var getShortestValue = servers.Where(x => x.Distance == shortestDistance).FirstOrDefault();
                    listViewSmart.Items.Add(getShortestValue);
                }
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }

        }

        public Home()
        {

            InitializeComponent();

            timer.Text = _startTimeDisplay;

            _stopWatch = new Stopwatch();
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimerElapse;

            CallData();
        }

        private void OnTimerElapse(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                timer.Text = _stopWatch.Elapsed.ToString(@"hh\:mm\:ss");
            });
        }
 

        private void CallData()
        {
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

                    #region get servers
                    Uri uri = new Uri(Settings.ApiUrl + "servers?orderby=id&orderdir=DESC");
                    var JsonServers = client.GetData(uri, Settings.AuthToken);
                    if (JsonServers == "401")
                    {
                        CloseAllWindows();
                        LoginForm login = new LoginForm();
                        login.ShowDialog();
                    }

                    dynamic _servers = JObject.Parse(JsonServers);

                    List<Server> servers = new List<Server>();

                    List<double> distances = new List<double>();

                    double latitude = 0;
                    double longitude = 0;
                    if (Settings.getCurrentLocation() != null)
                    {
                        latitude = Settings.getCurrentLocation().Latitude;
                        longitude = Settings.getCurrentLocation().Longitude;
                    }
                    foreach (var item in _servers.data)
                    {
                        double distance = SmartConnectDistance.distance(latitude, longitude, Convert.ToDouble(item.latitude), Convert.ToDouble(item.longitude), 'M');
                        distances.Add(distance);

                        servers.Add(new Server()
                        {
                            Id = item.server_id,
                            Name = item.name,
                            Flag = item.flag,
                            City = item.city != null ? item.city.name : "",
                            Country = item.country != null ? item.country.name : "",
                            Dns = item.dns,
                            Ip = item.ip_address,
                            Port = item.port,
                            DnsWithPort = item.dns + " " + item.port,
                            IsFavourited = item.is_favourited,
                            Continent = item.continent,
                            Latitude = Convert.ToDouble(item.latitude),
                            Longitude = Convert.ToDouble(item.longitude),
                            Distance = distance,
                            Iso = item.iso
                        });

                    }

                    Settings.setServers(servers.ToArray());

                    var serversList = Settings.getServers();

                    listViewRecomend.ItemsSource = serversList.ToList();

                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewRecomend.ItemsSource);
                    view.Filter = UserFilter;


                    listViewAllLocations.ItemsSource = serversList.ToList();

                    CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(listViewAllLocations.ItemsSource);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Continent");
                    view2.GroupDescriptions.Add(groupDescription);

                    view2.Filter = UserFilter;

                    var shortestDistance = distances.Min();
                    var getShortestValue = servers.Where(x => x.Distance == shortestDistance).FirstOrDefault();
                    listViewSmart.Items.Add(getShortestValue);

                    #endregion

                    #region Recent Location

                    List<Server> recentServer = new List<Server>();
                    if (Settings.getLastServerUsed() != null)
                    {
                        recentServer.Add(new Server()
                        {
                            Id = Settings.getLastServerUsed().Id,
                            Name = Settings.getLastServerUsed().Name,
                            Flag = Settings.getLastServerUsed().Flag,
                            Dns = Settings.getLastServerUsed().Dns,
                            Ip = Settings.getLastServerUsed().Ip,
                            Port = Settings.getLastServerUsed().Port,
                            DnsWithPort = Settings.getLastServerUsed().Dns + " " + Settings.getLastServerUsed().Port,
                            IsFavourited = Settings.getLastServerUsed().IsFavourited,
                            Continent = Settings.getLastServerUsed().Continent,
                        });

                        listViewRecent.ItemsSource = recentServer.ToList();

                    }

                    #endregion

                    currentLocationBlur.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                    currentLocation.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                    labelIpAddress.Text = Settings.getCurrentLocation().Ip;

                    connectionPanel.Visibility = Visibility.Hidden;

                    string avatar = Settings.getAvatar().Replace("data:image/png;base64,","");

                    byte[] binaryData = Convert.FromBase64String(avatar);

                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = new MemoryStream(binaryData);
                    bi.EndInit();

                    UserAvatar.Source = bi;

                    //  Canvas.SetZIndex(connectionPanel, -1);

                    if (Settings.getServer() != null)
                    {
                        countryFlagImage.ImageSource = new BitmapImage(new Uri(Settings.getServer().Flag));
                        currentLocation.Text = Settings.getServer().Name;

                        countryFlagImageBlur.ImageSource = new BitmapImage(new Uri(Settings.getServer().Flag));
                        currentLocationBlur.Text = Settings.getServer().Name;
                    }
                    else if (Settings.getConnectLastUsedServer())
                    {
                        //  Settings.fetchSettings();

                       // var d = Settings.getLastServerUsed();
                        countryFlagImageBlur.ImageSource = new BitmapImage(new Uri(Settings.getLastServerUsed().Flag));
                        countryFlagImage.ImageSource = new BitmapImage(new Uri(Settings.getLastServerUsed().Flag));
                        currentLocation.Text = Settings.getLastServerUsed().Name;
                        currentLocationBlur.Text = Settings.getLastServerUsed().Name;
                    }
                    else
                    {
                        currentLocation.Text = "Select Server";
                        currentLocationBlur.Text = "Select Server";
                    }

                    #region news

                    Uri news_uri = new Uri(Settings.ApiUrl + "news/device/windows");
                    var JsonNews = client.GetData(news_uri, Settings.AuthToken);

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


                    if (jsonUpdate != "")
                    {
                        dynamic _update = JObject.Parse(jsonUpdate);
                        Settings.IsUpdateAvailable = _update.data.is_update_available_windows;
                        if (Settings.IsUpdateAvailable)
                        {
                            updatePanel.Visibility = Visibility.Visible;
                            //Canvas.SetZIndex(updatePanel, 1);
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
                                connectionPanel.Visibility = Visibility.Hidden;
                                //   Canvas.SetZIndex(connectionPanel, -1);
                            });

                            IsVpnAvailable = true;

                            this.Dispatcher.Invoke(() =>
                            {

                                #region visibility

                                gridLocationBlur.Visibility = Visibility.Collapsed;
                                oysterBlueLogoBlur.Visibility = Visibility.Collapsed;

                                oysterBlueLogo.Visibility = Visibility.Visible;
                                gridLocation.Visibility = Visibility.Visible;


                                this.btnConnect.Visibility = Visibility.Hidden;
                                this.btnDisconnect.Visibility = Visibility.Visible;
                                #endregion


                                timer.Visibility = Visibility.Visible;
                                _stopWatch.Start();
                                _timer.Start();
                             

                                txtConnect.Text = "Connected";

                                greenDot.Visibility = Visibility.Visible;
                                greytDot.Visibility = Visibility.Hidden;

                                btnConnect.IsEnabled = true;

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
                    //   Canvas.SetZIndex(connectionPanel, 1);
                    connectionPanel.Visibility = Visibility.Visible;
                    //btnConnect.IsEnabled = false;

                    string avatar = Settings.getAvatar().Replace("data:image/png;base64,", "");

                    byte[] binaryData = Convert.FromBase64String(avatar);

                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = new MemoryStream(binaryData);
                    bi.EndInit();

                    UserAvatar.Source = bi;

                    List<Server> servers = new List<Server>();

                    List<double> distances = new List<double>();
                    var serversList = Settings.getServers().ToList();

                    foreach (var item in serversList)
                    {
                        double distance = SmartConnectDistance.distance(Settings.getCurrentLocation().Latitude, Settings.getCurrentLocation().Longitude, Convert.ToDouble(item.Latitude), Convert.ToDouble(item.Longitude), 'M');
                        distances.Add(distance);

                        servers.Add(new Server()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Flag = item.flag,
                            City = item.City,
                            Country = item.Country,
                            Dns = item.Dns,
                            Ip = item.Ip,
                            Port = item.Port,
                            DnsWithPort = item.Dns + " " + item.Port,
                            IsFavourited = item.IsFavourited,
                            Continent = item.Continent,
                            Latitude = Convert.ToDouble(item.Latitude),
                            Longitude = Convert.ToDouble(item.Longitude),
                            Distance = distance,
                            Iso = item.Iso
                        });

                    }


                    var asdsa = Settings.getServer().Flag;
                    if (Settings.getServer() != null)
                    {
                        
                        countryFlagImage.ImageSource = new BitmapImage(new Uri(Settings.getServer().Flag));
                        currentLocation.Text = Settings.getServer().Name;

                        countryFlagImageBlur.ImageSource = new BitmapImage(new Uri(Settings.getServer().Flag));
                        currentLocationBlur.Text = Settings.getServer().Name;
                    }
                    else if (Settings.getConnectLastUsedServer())
                    {
                        //  Settings.fetchSettings();

                      
                        countryFlagImageBlur.ImageSource = new BitmapImage(new Uri(Settings.getLastServerUsed().Flag));
                        countryFlagImage.ImageSource = new BitmapImage(new Uri(Settings.getLastServerUsed().Flag));
                        currentLocation.Text = Settings.getLastServerUsed().Name;
                        currentLocationBlur.Text = Settings.getLastServerUsed().Name;
                    }
                    else
                    {
                        currentLocation.Text = "Select Server";
                        currentLocationBlur.Text = "Select Server";
                    }

                    listViewRecomend.ItemsSource = servers.ToList(); //serversList.ToList();

                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewRecomend.ItemsSource);
                    view.Filter = UserFilter;

                    listViewAllLocations.ItemsSource = servers.ToList(); //serversList.ToList();

                    CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(listViewAllLocations.ItemsSource);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Continent");
                    view2.GroupDescriptions.Add(groupDescription);

                    view2.Filter = UserFilter;

                    var shortestDistance = distances.Min();
                    var getShortestValue = servers.Where(x => x.Distance == shortestDistance).FirstOrDefault();
                    listViewSmart.Items.Add(getShortestValue);
                }

            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void btnLogout(object sender, RoutedEventArgs e)
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
                    LoginForm login = new LoginForm();
                    login.ShowDialog();
                    this.Close();
                }
                else
                {
                }
            }
            catch (Exception ex)
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


        private void btnSetting(object sender, RoutedEventArgs e)
        {

        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            BG.Opacity = 1;
        }


        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            BG.Opacity = 0.3;
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                //tt_contacts.Visibility = Visibility.Collapsed;
                tt_messages.Visibility = Visibility.Collapsed;
                tt_maps.Visibility = Visibility.Collapsed;
                tt_settings.Visibility = Visibility.Collapsed;
                tt_signout.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                //tt_contacts.Visibility = Visibility.Visible;
                tt_messages.Visibility = Visibility.Visible;
                tt_maps.Visibility = Visibility.Visible;
                tt_settings.Visibility = Visibility.Visible;
                tt_signout.Visibility = Visibility.Visible;
            }
        }


        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(SearchTermTextBox.Text))
                return true;
            else
                return ((item as Server).Name.IndexOf(SearchTermTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }


        private void SearchTermTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (recomended_tab_item != null)
                    if (recomended_tab_item.IsSelected)
                        CollectionViewSource.GetDefaultView(listViewRecomend.ItemsSource).Refresh();

                if (all_location_tab_item != null)
                    if (all_location_tab_item.IsSelected)
                        CollectionViewSource.GetDefaultView(listViewAllLocations.ItemsSource).Refresh();
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (IsConnected)
                {
                    System.Windows.Forms.MessageBox.Show("You are already connected to server, disconnect first then select");
                    return;
                }

                var item = sender as ListViewItem;
                if (item != null && item.IsSelected)
                {

                    Settings.setServer((Server)item.DataContext);
                    Settings.fetchSettings();
                 // var d = Settings.getServer().Flag;
                    countryFlagImage.ImageSource = new BitmapImage(new Uri(Settings.getServer().Flag));
                    currentLocation.Text = Settings.getServer().Name; // Settings.getServer().City + " - " +  Settings.getServer().Country;


                    countryFlagImageBlur.ImageSource = new BitmapImage(new Uri(Settings.getServer().Flag));
                    currentLocationBlur.Text = Settings.getServer().Name; // Settings.getServer().City + " - " +  Settings.getServer().Country;

                    //foreach (Window _window in Application.Current.Windows)
                    //{
                    //    if (_window.Name == "Home_Window")
                    //    {
                    //        ((HomeWindow)_window).countryFlagImage.Source = new BitmapImage(new Uri(Settings.getServer().Flag));
                    //        ((HomeWindow)_window).currentLocation.Content = Settings.getServer().Name;
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void btnFavourite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;

                HttpClient client = new HttpClient();
                NameValueCollection collection = new NameValueCollection();
                collection.Add("server_id", button.Content.ToString());

                //  collection.Add("user_id",Settings.getUserId().ToString());

                var data = client.PostData(Settings.ApiUrl + "servers/favorites", collection, Settings.AuthToken);
                if (data == "401")
                {
                    CloseAllWindows();
                    Login login = new Login();
                    login.ShowDialog();
                }

                dynamic _data = JObject.Parse(data);



                if (_data.data.is_favourited == true)
                {
                    button.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "assets/star-fill.png")));
                }
                else
                {
                    button.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "assets/star-empty.png")));
                }

            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }



        //private void listViewRecomend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void server_list_tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void listViewRecomend_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if(IsConnecting)
                {
                    btnDisconnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                     return;
                }
                //string directory = Path.GetDirectoryName(Storage.UserDataFolder);
                //System.Windows.Forms.MessageBox.Show(directory);

                Settings.fetchSettings();
                if (Settings.getServer() == null)
                {
                    System.Windows.Forms.MessageBox.Show("Select Server Location");
                    return;
                }

                if (connectionPanel.IsVisible)
                {
                    System.Windows.Forms.MessageBox.Show("Internet Connection Not Available");
                    return;
                }
                gridLocationBlur.Visibility = Visibility.Collapsed;
                oysterBlueLogoBlur.Visibility = Visibility.Collapsed;

                oysterBlueLogo.Visibility = Visibility.Visible;
                gridLocation.Visibility = Visibility.Visible;

                // btnConnect.IsEnabled = false;

                btnConnect.Content = "Connecting...";
                txtConnect.Text = "Connecting...";

                IsConnecting = true;

                // btnConnect.IsEnabled = false;

                //check if it is in disconnecting state
                //if (AlreadyConnected)
                //{
                //    btnDisconnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                //    return;
                //}
                //this.btnConnect.Visibility = Visibility.Hidden;
                //this.btnDisconnect.Visibility = Visibility.Visible;

                #region Connection 

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
                    case "WIREGUARD":
                        IsConnected = OysterVpn.connect("WIREGUARD", "", "", "");
                        break;
                }

                currentLocationBlur.Text = Settings.getServer().City + ", " + Settings.getServer().Country;
                currentLocationBlur.Text = Settings.getServer().City + ", " + Settings.getServer().Country;

                labelIpAddress.Text = Settings.getServer().Ip;

                Settings.setLastUsedServer(Settings.getServer());

                #endregion

                AlreadyConnected = true;

                //RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Google\Chrome\NativeMessagingHosts\com.oystertech.vpn");

                ////storing the values  
                //key.SetValue("IsConnected", "1");
                //key.Close();


                if (IsConnected)
                {
                    OysterVpn.ProtectDNSLeak();

                    IsConnecting = false;
                }

                NetworkChange.NetworkAddressChanged +=
                NetworkAddressChanged;

            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }

            //txtConnect.Text = "Connected";
            //btnConnect.Content = "CONNECT";
            //greytDot.Visibility = Visibility.Collapsed;
            //greenDot.Visibility = Visibility.Visible;

            //btnConnect.Visibility = Visibility.Collapsed;
            //btnDisconnect.Visibility = Visibility.Visible;


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

           
            #region Check if vpn connected or not  
            if (IsAvailableNetworkActive())
            {
                if (IsConnected)
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                        var isVpnConnected = interfaces.Where((x => x.NetworkInterfaceType.ToString() == "Ppp" || x.Description.Contains("TAP-Windows Adapter") && x.OperationalStatus == OperationalStatus.Up)).FirstOrDefault();
                        //if(isVpnConnected.OperationalStatus != OperationalStatus.Up)
                        if (isVpnConnected == null)
                        {
                            IsVpnAvailable = false;
                            this.Dispatcher.Invoke(() =>
                            {

                                btnConnect.IsEnabled = true;

                                this.btnDisconnect.Content = "DISCONNECT";
                                txtConnect.Text = "Not Connected";

                                btnDisconnect.Visibility = Visibility.Hidden;
                                btnConnect.Visibility = Visibility.Visible;

                                greytDot.Visibility = Visibility.Visible;
                                greenDot.Visibility = Visibility.Collapsed;

                                currentLocationBlur.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                                currentLocation.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                                labelIpAddress.Text = Settings.getCurrentLocation().Ip;

                                #region timer
                                //_stopWatch.Stop();
                                //_timer.Stop();
                                //_stopWatch.Reset();
                                #endregion

                                ExtensionUserInfo.IsDisConnectVpnExtension = false;
                                ExtensionUserInfo.IsConnectVpnExtension = false;

                            });
                        }
                        else
                        {

                            this.Dispatcher.Invoke(() =>
                            {
                                connectionPanel.Visibility = Visibility.Hidden;
                                //Canvas.SetZIndex(connectionPanel, -1);
                            });

                            IsVpnAvailable = true;

                            this.Dispatcher.Invoke(() =>
                            {

                                #region visibility

                                this.btnConnect.Visibility = Visibility.Hidden;
                                this.btnDisconnect.Visibility = Visibility.Visible;

                                greytDot.Visibility = Visibility.Collapsed;
                                greenDot.Visibility = Visibility.Visible;
                                #endregion

                                txtConnect.Text = "Connected";
                                btnConnect.Content = "CONNECT";

                                #region timer
                                timer.Visibility = Visibility.Visible;
                                _stopWatch.Start();
                                _timer.Start();
                                #endregion

                                btnConnect.IsEnabled = true;

                                AlreadyConnected = true;

                                IsConnected = true;

                                IsDisconnected = false;

                            });
                        }
                    }
                }
            }

            #endregion

        }

        private void ConnectionPanelButton_Click(object sender, RoutedEventArgs e)
        {
            connectionPanel.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

        private void closePanel(object sender, RoutedEventArgs e)
        {
            updatePanel.Visibility = Visibility.Hidden;
        }

        private void newsLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string navigateUri = newsLink.NavigateUri.ToString();
                Process.Start(new ProcessStartInfo(navigateUri));

            }
            catch (Exception ex)
            {
                e.Handled = true;
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Logout();
        }

        private void Signout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Logout();
        }

        private void Logout()
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
                    LoginForm login = new LoginForm();
                    login.ShowDialog();
                    this.Close();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void imageSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SettingsScreen settings = new SettingsScreen();
            settings.ShowDialog();
        }

        private async void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {


            txtConnect.Text = "Disconnecting";
            this.btnDisconnect.Content = "DISCONNECTING";

            try
            {

                IsConnected = false;

                await Task.Run(() =>

                IsDisconnected = OysterVpn.disconnect()

                );

                IsConnected = false;

                OysterVpn.UnprotectDNSLeak();

                #region visibility

                btnDisconnect.Visibility = Visibility.Hidden;
                btnConnect.Visibility = Visibility.Visible;

                greytDot.Visibility = Visibility.Visible;
                greenDot.Visibility = Visibility.Collapsed;

                btnDisconnect.Visibility = Visibility.Collapsed;
                btnConnect.Visibility = Visibility.Visible;

                #endregion

                txtConnect.Text = "Not Connected";
                this.btnDisconnect.Content = "DISCONNECT";
                currentLocationBlur.Text = Settings.getCurrentLocation().City + ", " + Settings.getCurrentLocation().Country;
                labelIpAddress.Text = Settings.getCurrentLocation().Ip;

                #region timer
                //_stopWatch.Stop();
                //_timer.Stop();
                _stopWatch.Reset();
                timer.Visibility = Visibility.Collapsed;

                #endregion

                ExtensionUserInfo.IsDisConnectVpnExtension = false;
                ExtensionUserInfo.IsConnectVpnExtension = false;
            }
            catch (Exception ex)
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

            // setting cancel to true will cancel the close request
            // so the application is not closed
            cancelEventArgs.Cancel = true;
            // this.Hide();
            this.WindowState = WindowState.Minimized;
            //base.OnClosing(cancelEventArgs);
           

            //if (System.Windows.MessageBox.Show("Do you want to close this application?",
            //"Alert", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    Environment.Exit(Environment.ExitCode);
            //}
            //else
            //{
            //    cancelEventArgs.Cancel = true;
            //}


        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    break;
                case WindowState.Minimized:
                    this.notifyIcon = new System.Windows.Forms.NotifyIcon();
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

        // Declare a method to handle NetworkAvailabilityChanged events.
        private void NetworkAvailabilityChanged(
        object sender, NetworkAvailabilityEventArgs e)
        {
            // Report whether the network is now available or unavailable.
            if (e.IsAvailable)
            {
                this.Dispatcher.Invoke(() =>
                {
                    updatePanel.Visibility = Visibility.Visible;
                    connectionPanel.Visibility = Visibility.Hidden;
                });

                IsNetworkAvailable = true;
                Console.WriteLine("Network Available");



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
                    updatePanel.Visibility = Visibility.Hidden;
                    connectionPanel.Visibility = Visibility.Visible;
                });

                Console.WriteLine("Network Unavailable");

                if (Settings.getinternetKillSwitch() && IsConnected)
                {

                    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                    INetFwRule firewallRule = firewallPolicy.Rules.OfType<INetFwRule>().Where(x => x.Name == "OysterVPN Block Internet").FirstOrDefault();

                    if (firewallRule == null)
                    {
                        INetFwRule _firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                        _firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                        _firewallRule.Description = "Used to block all internet access.";
                        _firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                        _firewallRule.Enabled = true;
                        _firewallRule.InterfaceTypes = "All";
                        _firewallRule.Name = "OysterVPN Block Internet";

                        firewallPolicy.Rules.Add(_firewallRule);
                    }


                    IsBlockedFirewall = true;

                    //this.Dispatcher.Invoke(() =>
                    //{

                    //    IsDisconnected = true;
                    //    IsConnected = false;

                    //    txtConnect.Text = "Connecting";
                    //    this.btnDisconnect.Content = "CONNECTING";
                    //    greytDot.Visibility = Visibility.Visible;
                    //    greenDot.Visibility = Visibility.Collapsed;

                    //    timer.Visibility = Visibility.Collapsed;
                    //    _stopWatch.Reset();
                    //    //_timer.Start();

                    //    AlreadyConnected = false;

                    //});

                }


                this.Dispatcher.Invoke(() =>
                {

                    IsDisconnected = true;
                    IsConnected = false;

                    txtConnect.Text = "Connecting";
                    this.btnDisconnect.Content = "CONNECTING";
                    greytDot.Visibility = Visibility.Visible;
                    greenDot.Visibility = Visibility.Collapsed;

                    timer.Visibility = Visibility.Collapsed;
                    _stopWatch.Reset();
                    //_timer.Start();

                    AlreadyConnected = false;

                });


                IsNetworkAvailable = false;
                IsConnected = false;
            }
        }



        #region MENU

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
            option.Left = option.Owner.Left < 500 ? option.Owner.Left + 60 : option.Owner.Left - 90;  //+  (option.Owner.Width - option.Width) / 2;
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

        private void MenuItem_Quit_Click(object sender, RoutedEventArgs e)
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
                OysterVpn.disconnect();
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
        private void IsKillSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (IsKillSwitch.IsChecked)
            {
                Settings.setinternetKillSwitch(true);

                //  OysterVpn.ProtectDNSLeak();
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
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SecureDevice secure = new SecureDevice();
            secure.Owner = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            secure.Left = secure.Owner.Left < 500 ? secure.Owner.Left + 400 : secure.Owner.Left - 390;
            secure.Top = secure.Owner.Top;
            secure.Show();
        }

        #endregion

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SecureDevice secure = new SecureDevice();
            secure.Owner = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            secure.Left = secure.Owner.Left < 500 ? secure.Owner.Left + 400 : secure.Owner.Left - 390;
            secure.Top = secure.Owner.Top;
            secure.Show();
        }

        private void ListViewItem_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SupportUrl + "?utm_source=app&utm_medium=windows&utm_campaign=contact";
                Process.Start(new ProcessStartInfo(navigateUri));
            }

            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SecureDevice secure = new SecureDevice();
            secure.Owner = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            secure.Left = secure.Owner.Left < 500 ? secure.Owner.Left + 400 : secure.Owner.Left - 390;
            secure.Top = secure.Owner.Top;
            secure.Show();
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SupportUrl + "?utm_source=app&utm_medium=windows&utm_campaign=contact";
                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void ListViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SupportUrl + "?utm_source=app&utm_medium=windows&utm_campaign=contact";
                Process.Start(new ProcessStartInfo(navigateUri));
            }

            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            SecureDevice secure = new SecureDevice();
            secure.Owner = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            secure.Left = secure.Owner.Left < 500 ? secure.Owner.Left + 400 : secure.Owner.Left - 390;
            secure.Top = secure.Owner.Top;
            secure.Show();
        }

        //private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //  //  SettingsMenuItem.IsSubmenuOpen = true;

        //}

        //private void SettingsMenuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //  //  SettingsMenuItem.IsSubmenuOpen = true;
        //}
    }
}
