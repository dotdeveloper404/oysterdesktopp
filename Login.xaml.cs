using App;
using CountryData.Standard;
using log4net;
using NativeMessaging;
using Newtonsoft.Json.Linq;
using Notifications.Wpf;
using OysterVPNLibrary;
using OysterVPNLibrary.Responses;
using OysterVPNModel;
using System;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        //   static Host Host;

        public Login()
        {
            InitializeComponent();

            #region chrome extension connect
            if (ExtensionUserInfo.Email != null && ExtensionUserInfo.Password != null)
            {

                txtEmailAddress.Text = ExtensionUserInfo.Email;
                txtPassword.Password = ExtensionUserInfo.Password;

                btnLogin.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            }

            #endregion

            Closing += OnClosing;
         
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
            if (System.Windows.MessageBox.Show("Do you want to close this applicaion?",
        "Alert", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Environment.Exit(Environment.ExitCode);
            }
            else
            {
                cancelEventArgs.Cancel = true;
            }


        }




        private void Button_Close(object sender, RoutedEventArgs e)
        {
            // this.WindowState = WindowState.Minimized;
            //bool? Result = new MessageBoxCustom("Are you sure, You want to close  application ? ", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();
            //if (Result.Value == true)
            //{
            //    Environment.Exit(Environment.ExitCode);
            //}
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            //LoginLoadingBar.Visibility = Visibility.Visible;


            if (_Login())
            {
                try
                {

                    //HomeScreen home = new HomeScreen();
                    HomeWindow home = new HomeWindow();
                    //this.Close();
                    this.Hide();
                    this.Visibility = Visibility.Hidden;
                    LoginLoadingBar.Visibility = Visibility.Hidden;
                    home.ShowDialog();
                }
                catch (Exception ex)
                {
                    ////HomeScreen home = new HomeScreen();
                    //HomeWindow home = new HomeWindow();
                    ////this.Close();
                    //this.Hide();
                    //this.Visibility = Visibility.Hidden;
                    //LoginLoadingBar.Visibility = Visibility.Hidden;
                    //home.ShowDialog();
                }
            }


        }


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private  bool _Login()
        {

            try
            {

                if (txtEmailAddress.Text == "")
                {
                    txtValidation.Text = "Enter Email Address";
                    return false;
                }
                else if (txtPassword.Password == "")
                {
                    txtValidation.Text = "Enter Password";
                    return false;
                }

                LoginLoadingBar.Visibility = Visibility.Visible;

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
                        Iso =item.iso,
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

                LoginLoadingBar.Visibility = Visibility.Hidden;

                #region save data into auth file for open vpn 
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
                string[] auth = {txtEmailAddress.Text.ToString(),txtPassword.Password.ToString()};

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
                catch(Exception e)
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

               
                LoginLoadingBar.Visibility = Visibility.Hidden;

                txtValidation.Text = "Error Occured While Login,Contact Administrator";// ex.Message + " "+ ex.InnerException.Message + "Error Occured While Login,Contact Administrator";

                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);

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

            return false;

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();

            Signup s = new Signup();

            s.ShowDialog();

            this.Visibility = Visibility.Hidden;
        }

        

        private void btnForgot_Click(object sender, RoutedEventArgs e)
        {

            ForgotPassword forgot = new ForgotPassword();
            forgot.Owner = this;


            forgot.Left = forgot.Owner.Left < 500 ? forgot.Owner.Left + 400 : forgot.Owner.Left - 370; // +  (forgot.Owner.Width - forgot.ActualWidth) / 2;
            forgot.Top = forgot.Owner.Top;// + 20;

            forgot.Show();

            //  this.Hide();
            //   this.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
