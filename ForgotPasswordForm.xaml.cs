using App;
using log4net;
using Notifications.Wpf;
using OysterVPNLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// Interaction logic for ForgotPasswordForm.xaml
    /// </summary>
    public partial class ForgotPasswordForm : Window
    {

        int counter = 0;

        public ForgotPasswordForm()
        {
            InitializeComponent();
        }

        private void btnSignin_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Visibility = Visibility.Hidden;

            LoginForm l = new LoginForm();
            l.ShowDialog();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtEmailAddress.Text == "")
                {
                    txtValidation.Text = "Enter Valid Email Address";
                    return;
                }

                loader.Visibility = Visibility.Visible;
                HttpClient client = new HttpClient();
                NameValueCollection collection = new NameValueCollection();
                collection.Add("email", txtEmailAddress.Text);

                var data = client.PostData(Settings.ApiUrl + "password/forgot-password", collection);

                System.Windows.MessageBox.Show("A temporary password has been sent to your email address.",
                 "Success", MessageBoxButton.OK);

                this.Close();

                LoginForm login = new LoginForm();
                login.ShowDialog();

            }
            catch (Exception ex)
            {
                loader.Visibility = Visibility.Collapsed;
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
            //else
            //{
            //    counter = 0;
            //    btnBack.IsEnabled = false;
            //    btnNext.IsEnabled = true;
            //}
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
            //else
            //{
            //    counter = 2;
            //    btnNext.IsEnabled = false;
            //    btnBack.IsEnabled = true;
            //}
        }
    }
}
