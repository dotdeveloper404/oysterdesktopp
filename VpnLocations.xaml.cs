using App;
using log4net;
using Newtonsoft.Json.Linq;
using OysterVPNLibrary;
using OysterVPNModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace OysterVPN
{
    /// <summary>
    /// Interaction logic for VpnLocations.xaml
    /// </summary>
    public partial class VpnLocations : Window
    {


        public VpnLocations()
        {
            InitializeComponent();

            try
            {
                listViewSmart.Items.Clear();

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
                        Login login = new Login();
                        login.ShowDialog();
                    }

                    dynamic _servers = JObject.Parse(JsonServers);

                    List<Server> servers = new List<Server>();

                    List<double> distances = new List<double>();

                    foreach (var item in _servers.data)
                    {
                        double distance = SmartConnectDistance.distance(Settings.getCurrentLocation().Latitude, Settings.getCurrentLocation().Longitude, Convert.ToDouble(item.latitude), Convert.ToDouble(item.longitude), 'M');
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

                if (recomended_tab_item.IsSelected)
                    CollectionViewSource.GetDefaultView(listViewRecomend.ItemsSource).Refresh();

                if (all_location_tab_item.IsSelected)
                    CollectionViewSource.GetDefaultView(listViewAllLocations.ItemsSource).Refresh();
            }
        }

        private void Window_Activateddd(object sender, EventArgs e)
        {
            try
            {
                listViewSmart.Items.Clear();

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
                        Login login = new Login();
                        login.ShowDialog();
                    }

                    dynamic _servers = JObject.Parse(JsonServers);

                    List<Server> servers = new List<Server>();

                    List<double> distances = new List<double>();

                    foreach (var item in _servers.data)
                    {
                        double distance = SmartConnectDistance.distance(Settings.getCurrentLocation().Latitude, Settings.getCurrentLocation().Longitude, Convert.ToDouble(item.latitude), Convert.ToDouble(item.longitude), 'M');
                        distances.Add(distance);

                        servers.Add(new Server()
                        {
                            Id = item.server_id,
                            Name = item.name,
                            Flag = item.flag,
                            City = item.city.name,
                            Country = item.country.name,
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

                //var notificationManager = new NotificationManager();

                //notificationManager.Show(new NotificationContent
                //{
                //    Title = "Error Occured",
                //    Message = ex.Message,
                //    Type = NotificationType.Error
                //});

            }
        }

        //private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var item = sender as ListViewItem;
        //    if (item != null && item.IsSelected)
        //    {
        //        Settings.setServer((Server)item.DataContext);

        //        foreach (Window _window in Application.Current.Windows)
        //        {
        //            if (_window.Name == "Home_Window")
        //            {
        //                ((HomeWindow)_window).countryFlagImage.Source = new BitmapImage(new Uri(Settings.getServer().Flag));
        //                ((HomeWindow)_window).currentLocation.Content = Settings.getServer().Name;
        //            }
        //        }

        //    }
        //}

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = sender as ListViewItem;
                if (item != null && item.IsSelected)
                {
                    Settings.setServer((Server)item.DataContext);

                    foreach (Window _window in Application.Current.Windows)
                    {
                        if (_window.Name == "Home_Window")
                        {
                            ((HomeWindow)_window).countryFlagImage.Source = new BitmapImage(new Uri(Settings.getServer().Flag));
                            ((HomeWindow)_window).currentLocation.Content = Settings.getServer().Name;
                        }
                    }

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

        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Hide();
        }

        private void listViewRecomend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void server_list_tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void listViewRecomend_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
