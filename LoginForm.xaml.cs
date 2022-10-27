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
using System.Threading;
using System.Windows.Threading;
using System.Web.Caching;

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

        private async  void btnLogin(object sender, RoutedEventArgs e)
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
          //  await Task.Delay(5000);
            //this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            //{
            //    loader.Visibility = Visibility.Visible;
            //    Thread.Sleep(1000);
            //}));
          
                    if (await _Login())
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
                    loader.Visibility = Visibility.Hidden;
              

        }


        private async Task<bool> _Login()
        {

            try
            {

                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                {
                    loader.Visibility = Visibility.Visible;
                }));

                HttpClient client = new HttpClient();
                NameValueCollection collection = new NameValueCollection();
                collection.Add("email", txtEmailAddress.Text);
                collection.Add("password", txtPassword.Password);

                var data = client.PostData(Settings.ApiUrl + "authenticate", collection);
                await Task.Delay(4000);

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
                response.userinfo.Avatar = _data.data.userinfo.avatar;

                #region save to settings

                Settings.setToken((string)_data.data.token);
                Settings.setName((string)_data.data.userinfo.fullname);
                Settings.setEmail((string)_data.data.userinfo.email);
                Settings.setPhone((string)_data.data.userinfo.phone_number == null ? "" : (string)_data.data.userinfo.phone_number);
                Settings.setAvatar((string)_data.data.userinfo.avatar);
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

                #region save data into auth file for open vpn 
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
                string[] auth = { txtEmailAddress.Text.ToString(), txtPassword.Password.ToString() };

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
               
                #endregion

                return true;
            }
            catch (Exception ex)
            {

                throw ex;

            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

            if (counter == 2)
            {
                Slide1.Visibility = Visibility.Hidden;
                Slide3.Visibility = Visibility.Hidden;
                Slide2.Visibility = Visibility.Visible;

                counter--;

            }
            else if (counter == 1)
            {
                Slide1.Visibility = Visibility.Visible;
                Slide2.Visibility = Visibility.Hidden;
                Slide3.Visibility = Visibility.Hidden;

                counter = 0;
                btnBack.IsEnabled = false;
                btnNext.IsEnabled = true;

            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (counter == 0)
            {

                Slide2.Visibility = Visibility.Visible;
                Slide3.Visibility = Visibility.Hidden;
                Slide1.Visibility = Visibility.Hidden;

                counter++;
            }
            else if (counter == 1)
            {
                Slide1.Visibility = Visibility.Hidden;
                Slide2.Visibility = Visibility.Hidden;
                Slide3.Visibility = Visibility.Visible;

                counter++;
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
