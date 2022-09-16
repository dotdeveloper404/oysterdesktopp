using App;
using log4net;
using Notifications.Wpf;
using OysterVPNLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace OysterVPN
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public ForgotPassword()
        {
            InitializeComponent();

        }


        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtEmailAddress.Text == "")
                {
                    txtValidation.Text = "Enter Valid Email Address";
                    return;
                }

                LoginLoadingBar.Visibility = Visibility.Visible;
                HttpClient client = new HttpClient();
                NameValueCollection collection = new NameValueCollection();
                collection.Add("email", txtEmailAddress.Text);

                var data = client.PostData(Settings.ApiUrl + "password/forgot-password", collection);

                //  bool? Result = new MessageBoxCustom("A temporary password has been sent to your email address...", MessageType.Success, MessageButtons.Ok).ShowDialog();
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Success",
                    Message = "A temporary password has been sent to your email address.",
                    Type = NotificationType.Success
                });
                this.Close();
            
            }
            catch(Exception ex)
            {
                txtValidation.Text = "Error Occured,Contact Support";

                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);

                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Error Occured",
                    Message = ex.Message,
                    Type = NotificationType.Error
                });

                //ForgetLoadingGrid.Visibility = Visibility.Hidden;
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

        private void txtEmailAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
