using App;
using log4net;
using Newtonsoft.Json.Linq;
using Notifications.Wpf;
using OysterVPNLibrary;
using OysterVPNLibrary.Responses;
using OysterVPNModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
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
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Window
    {
        public Signup()
        {
            InitializeComponent();

          Closing += OnClosing;

        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            //if (System.Windows.MessageBox.Show(this, "Are you sure want to quit?", "Confirm", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            //{
            //    cancelEventArgs.Cancel = true;

            //}

            //if (cancelEventArgs.Cancel == false)
            //{
            //    Environment.Exit(Environment.ExitCode);
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

            //bool? Result = new MessageBoxCustom("Are you sure, You want to close  application ? ", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            //if (Result.Value == true)
            //{
            //    Environment.Exit(Environment.ExitCode);
            //}


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
            if (txtEmailAddress.Text == "")
            {
                txtValidate.Text = "Enter Valid Email Address";
            }
            else if (txtPassword.Password == "") {
                txtValidate.Text = "Enter Password";
            }
            else if(txtPassword.Password != txtRePassword.Password)
            {
                txtValidate.Text = "Password Does not Match";
            }

            else
            {
                try
                {

                    SignupLoadingBar.Visibility = Visibility.Visible;
                  
                    HttpClient client = new HttpClient();
                    NameValueCollection collection = new NameValueCollection();
                    collection.Add("email", txtEmailAddress.Text);
                    collection.Add("password", txtPassword.Password);
                    collection.Add("confirm_password", txtRePassword.Password);
                    collection.Add("user_type", "client");
                    collection.Add("terms", "1");

                    var data = client.PostData(Settings.ApiUrl + "signup", collection);

                    var notificationManager = new NotificationManager();

                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Success",
                        Message = "Successfully Signup to Oyster VPN,Login to Continue...",
                        Type = NotificationType.Success
                    });


                   // bool? Result = new MessageBoxCustom("Your account has been created successfully", MessageType.Success, MessageButtons.Ok).ShowDialog();


                    this.Hide();
                    Login login = new Login();
                    login.ShowDialog();

                    #region save to settings

                    //dynamic _data = JObject.Parse(data);

                    //LoginResponse response = new LoginResponse();

                    //response.message = _data.message;
                    //response.token = _data.data.token;
                    //Settings.AuthToken = _data.data.token;

                    //response.userinfo.Id = _data.data.userinfo.user_id;
                    //response.userinfo.FullName = _data.data.userinfo.fullname;
                    //response.userinfo.FirstName = _data.data.userinfo.firstname;
                    //response.userinfo.LastName = _data.data.userinfo.lastname;
                    //response.userinfo.Email = _data.data.userinfo.email;
                    //response.userinfo.Phone = _data.data.userinfo.phone_number;


                    //Settings.setName((string)_data.data.userinfo.fullname);
                    //Settings.setEmail((string)_data.data.userinfo.email);
                    //Settings.setPhone((string)_data.data.userinfo.phone_number);
                    //Settings.setPassword(txtPassword.Password);
                    //Settings.setUserId((int)_data.data.userinfo.user_id);
                    //Settings.setLoggedin(true);


                    //Uri uri = new Uri(Settings.ApiUrl + "servers?orderby=id&orderdir=DESC");
                    //var JsonServers = client.GetData(uri, response.token);
                    //dynamic _servers = JObject.Parse(JsonServers);

                    //response.servers = new List<Server>();

                    //foreach (var item in _servers.data.data)
                    //{

                    //    response.servers.Add(new Server()
                    //    {
                    //        Id = item.server_id,
                    //        Name = item.name,
                    //        Flag = item.flag,
                    //        Dns = item.dns,
                    //        Ip = item.ip_address,
                    //        Port = item.port,
                    //        DnsWithPort = item.dns + " " + item.port,
                    //        IsFavourited = item.is_favourited,
                    //    });

                    //}

                    //Settings.setServers(response.servers.ToArray());



                    #endregion

                }
                catch (Exception ex)
                {

                    txtValidate.Text = "Error Occured While Signup,Contact Support";

                    ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                    logger.Error(ex.Message);

                    var notificationManager = new NotificationManager();

                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Error Occured While Signup,Contact Support",
                        Message = ex.Message,
                        Type = NotificationType.Error
                    });

                    //SignupLoadingBar.Visibility = Visibility.Hidden;
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
        }


       
    

    private void txtEmailAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();
            this.Visibility = Visibility.Hidden;
         
            Login l = new Login();
            l.ShowDialog();
         
        }

        private void txtEmailAddress_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void checkBoxTerms_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
