using System;
using App;
using CountryData.Standard;
using log4net;
using NativeMessaging;
using Newtonsoft.Json.Linq;
using Notifications.Wpf;
using OysterVPNLibrary;
using OysterVPNLibrary.Responses;
using OysterVPNModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;
using System.IO;
using System.Threading.Tasks;


namespace OysterVPN
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        int counter = 0;
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin(object sender, RoutedEventArgs e)
        {

            if (txtEmailAddress.Text == "")
            {
                txtValidation.Text = "Enter Email Address";
                return;
            }
            else if (txtPassword.Password == "")
            {
                txtValidation.Text = "Enter Password";
                return;
            }

            loader.Visibility = Visibility.Visible;

            await Task.Delay(5000);

            if (_Login())
            {

                try
                {

                    Home home = new Home();
                    //HomeWindow home = new HomeWindow();
                    //this.Close();
                    this.Hide();
                    this.Visibility = Visibility.Hidden;
                    loader.Visibility = Visibility.Collapsed;
                    home.ShowDialog();
                }
                catch (Exception ex)
                {
                    loader.Visibility = Visibility.Hidden;

                    txtValidation.Text = "Error Occured While Login,Contact Administrator";// ex.Message + " "+ ex.InnerException.Message + "Error Occured While Login,Contact Administrator";

                    ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                    logger.Error(ex.Message);
                }
            }
        }


        private bool _Login()
        {

            try
            {

                loader.Visibility = Visibility.Visible;

                HttpClient client = new HttpClient();
                NameValueCollection collection = new NameValueCollection();
                collection.Add("email", txtEmailAddress.Text);
                collection.Add("password", txtPassword.Password);

                var data = client.PostData(Settings.ApiUrl + "authenticate", collection);


                //  var deserial = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponse>(client.PostData(Settings.ApiUrl + "authenticate", collection));
                ///   JObject obj = JObject.Parse(client.PostData(Settings.ApiUrl + "authenticate", collection));
                //  var d = data.Replace("data", "");

                //   var ex = JsonConvert.DeserializeObject<LoginResponse>(data);

                dynamic _data = JObject.Parse(data);

                if (_data.data.token == "")
                {
                    txtValidation.Text = "Username Or Password Is Incorrect";
                    return false;
                }


                LoginResponse response = new LoginResponse();

                response.message = _data.message;
                response.token = _data.data.token;
                Settings.AuthToken = _data.data.token;
                Settings.IsUpdateAvailable = _data.data.is_update_available_windows;

                response.userinfo.Id = _data.data.userinfo.user_id;
                response.userinfo.FullName = _data.data.userinfo.fullname;
                response.userinfo.FirstName = _data.data.userinfo.first_name;
                response.userinfo.LastName = _data.data.userinfo.last_name;
                response.userinfo.Email = _data.data.userinfo.email;
                response.userinfo.Phone = _data.data.userinfo.phone_number;

                #region save to settings

                Settings.setToken((string)_data.data.token);
                Settings.setName((string)_data.data.userinfo.fullname);
                Settings.setEmail((string)_data.data.userinfo.email);
                Settings.setPhone((string)_data.data.userinfo.phone_number == null ? "" : (string)_data.data.userinfo.phone_number);
                Settings.setPassword(txtPassword.Password);
                Settings.setUserId((int)_data.data.userinfo.user_id);
                Settings.setLoggedin(true);
                Settings.setinternetKillSwitch(false); // by default set to false

                Uri uri = new Uri(Settings.ApiUrl + "servers?orderby=id&orderdir=DESC");
                var JsonServers = client.GetData(uri, response.token);
                dynamic _servers = JObject.Parse(JsonServers);

                response.servers = new List<Server>();

                foreach (var item in _servers.data)
                {
                    response.servers.Add(new Server()
                    {
                        Id = item.server_id,
                        Name = item.name,
                        Flag = item.flag,
                        City = item.city != null ? item.city.name : "",
                        Country = item.country != null ? item.country.name : "",
                        Iso = item.iso,
                        Dns = item.dns,
                        Ip = item.ip_address,
                        Port = item.port,
                        DnsWithPort = item.dns + " " + item.port,
                        IsFavourited = item.is_favourited,
                        Latitude = Convert.ToDouble(item.latitude),
                        Longitude = Convert.ToDouble(item.longitude),

                    });
                }

                Settings.setServers(response.servers.ToArray());

                #endregion

                loader.Visibility = Visibility.Collapsed;

                #region save data into auth file for open vpn 
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
                string[] auth = { txtEmailAddress.Text.ToString(), txtPassword.Password.ToString() };

                //txtValidation.Text = workingDirectory + " asdsa" + projectDirectory;
                //txtValidation.Text= Environment.CurrentDirectory + "\\assets\\data\\oyster\\data.bin";
                //System.Windows.Forms.MessageBox.Show(workingDirectory);
                //System.Windows.Forms.MessageBox.Show(projectDirectory);
                //string startupPath = System.IO.Directory.GetParent(@"./").FullName;
                //System.Windows.Forms.MessageBox.Show(startupPath.ToString());
                // Write array of strings to a file using WriteAllLines.  
                // If the file does not exists, it will create a new file.  
                // This method automatically opens the file, writes to it, and closes file  
                //string path = workingDirectory + "\\assets\\data\\oyster\\auth.txt";  //"D:\\VS-Repo\\Oyster\\assets\\data\\oyster\\auth.txt"; //projectDirectory + @"\assets\data\oyster\auth.txt\";
                //   System.Windows.Forms.MessageBox.Show(path.ToString());
                //if (File.Exists(path)) { 
                //    File.Delete(path); 
                //}
                //File.WriteAllLines(path, auth);

                string directory = Path.GetDirectoryName(Storage.UserDataFolder);//(SettingPath); //(Storage.UserDataFolder + @"\" + SettingPath);
                string path = directory + "\\auth.conf";



                try
                {

                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fs.SetLength(0);

                        using (StreamWriter sw = new StreamWriter(fs))
                        {

                            sw.WriteLine(txtEmailAddress.Text);
                            sw.WriteLine(txtPassword.Password);
                        }
                    }


                }
                catch (Exception e)
                {
                    txtValidation.Text = e.Message;
                }

                //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                //using (StreamReader sr = new StreamReader(fs))
                //{
                //    using (StreamWriter sw = new StreamWriter(path))
                //    {
                //        sw.WriteLine(txtEmailAddress.Text.ToString());
                //        sw.WriteLine(txtPassword.Password.ToString());
                //    }
                //}

                //loads all Country Data via the constructor (You can initialize this once as a singleton)  
                //var helper = new CountryHelper();


                //#region get location from ip

                //Uri urlLocation = new Uri("https://ipinfo.io/");
                //var locationData = client.GetData(urlLocation);
                //dynamic _locationData = JObject.Parse(locationData);

                //CurrentLocation location = new CurrentLocation();


                //location.Ip = _locationData.ip;
                //location.Host = _locationData.hostname;
                //location.City = _locationData.city;
                //location.Region = _locationData.region;
                //location.Country = _locationData.country;
                //location.Country = helper.GetCountry(location.Country).First().CountryName;

                //String loc = _locationData.loc;
                //var res = loc.Split(',');

                //location.Latitude = Convert.ToDouble(res[0]);
                //location.Longitude = Convert.ToDouble(res[1]);

                //Settings.setCurrentLocation(location);

                #endregion

                return true;
            }
            catch (Exception ex)
            {

                throw ex;


                // var notificationManager = new NotificationManager();

                //notificationManager.Show(new NotificationContent
                //{
                //    Title = "Error Occured",
                //    Message = ex.Message,
                //    Type = NotificationType.Error
                //});

                //LoginLoadingBar.Visibility = Visibility.Hidden;
                //using (SentrySdk.Init(o =>
                //{
                //    o.Dsn = "https://d30b3952e6364830a8692ae79d15abb9@o850652.ingest.sentry.io/5817628";
                //    // When configuring for the first time, to see what the SDK is doing:
                //    o.Debug = true;
                //    // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
                //    // We recommend adjusting this value in production.
                //    o.TracesSampleRate = 1.0;
                //}))
                //{
                //    // App code goes here. Dispose the SDK before exiting to flush events.

                //    SentrySdk.CaptureMessage(ex.Message);
                //}

            }


        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (counter == 4)
            {

                Slide2.Visibility = Visibility.Hidden;
                Slide4.Visibility = Visibility.Hidden;
                Slide1.Visibility = Visibility.Hidden;

                Slide3.Visibility = Visibility.Visible;

                counter--;
            }
            else if (counter == 3)
            {
                Slide1.Visibility = Visibility.Hidden;
                Slide3.Visibility = Visibility.Hidden;
                Slide4.Visibility = Visibility.Hidden;


                Slide2.Visibility = Visibility.Visible;

                counter--;

            }
            else if (counter == 2)
            {
                Slide1.Visibility = Visibility.Visible;
                Slide2.Visibility = Visibility.Hidden;
                Slide4.Visibility = Visibility.Hidden;


                Slide3.Visibility = Visibility.Hidden;

                counter--;

            }
            else
            {
                counter = 0;
                btnBack.IsEnabled = false;
                btnNext.IsEnabled = true;
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (counter == 0)
            {

                Slide2.Visibility = Visibility.Hidden;
                Slide3.Visibility = Visibility.Hidden;
                Slide4.Visibility = Visibility.Hidden;

                Slide1.Visibility = Visibility.Visible;

                counter++;
            }
            else if (counter == 1)
            {
                Slide1.Visibility = Visibility.Hidden;
                Slide3.Visibility = Visibility.Hidden;
                Slide4.Visibility = Visibility.Hidden;


                Slide2.Visibility = Visibility.Visible;

                counter++;

            }
            else if (counter == 2)
            {
                Slide1.Visibility = Visibility.Hidden;
                Slide2.Visibility = Visibility.Hidden;
                Slide4.Visibility = Visibility.Hidden;


                Slide3.Visibility = Visibility.Visible;

                counter++;

            }
            else if (counter == 3)
            {
                Slide1.Visibility = Visibility.Hidden;
                Slide2.Visibility = Visibility.Hidden;
                Slide3.Visibility = Visibility.Hidden;

                Slide4.Visibility = Visibility.Visible;

                counter++;

            }
            else
            {
                counter = 4;
                btnNext.IsEnabled = false;
                btnBack.IsEnabled = true;
            }
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Visibility = Visibility.Hidden;
            SignupForm s = new SignupForm();
            s.ShowDialog();
        }

        private void btnForgot_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Visibility = Visibility.Hidden;

            ForgotPasswordForm forgot = new ForgotPasswordForm();
            forgot.ShowDialog();
        }
    }
}
