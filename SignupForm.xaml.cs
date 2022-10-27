using App;
using log4net;
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
    /// Interaction logic for SignupForm.xaml
    /// </summary>
    public partial class SignupForm : Window
    {

        int counter = 0;    
        public SignupForm()
        {
            InitializeComponent();
        }

        private async void btnSignup(object sender, RoutedEventArgs e)
        {

            if (txtEmailAddress.Text == "")
            {
                txtValidate.Text = "Enter Valid Email Address";
                return;
            }
            else if (txtPassword.Password == "")
            {
                txtValidate.Text = "Enter Password";
                return;
            }
            else if (txtPassword.Password != txtRePassword.Password)
            {
                txtValidate.Text = "Password Does not Match";
                return;
            }

            else
            {
                try
                {
                    loader.Visibility = Visibility.Visible;

                    await Task.Delay(5000);

                    HttpClient client = new HttpClient();
                    NameValueCollection collection = new NameValueCollection();
                    collection.Add("email", txtEmailAddress.Text);
                    collection.Add("password", txtPassword.Password);
                    collection.Add("confirm_password", txtRePassword.Password);
                    collection.Add("user_type", "client");
                    collection.Add("terms", "1");

                    var data = client.PostData(Settings.ApiUrl + "signup", collection);


                    if (System.Windows.MessageBox.Show("Your account has been created successfully",
                   "Success", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        loader.Visibility = Visibility.Collapsed;
                        this.Hide();
                        LoginForm login = new LoginForm();
                        login.ShowDialog();

                    }
                     
                    
                }
                catch (Exception ex)
                {
                    loader.Visibility = Visibility.Collapsed;
                    txtValidate.Text = "Error Occured While Signup,Contact Support";

                    ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                    logger.Error(ex.Message);
                }
            }
        }

        private void btnSignin_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Visibility = Visibility.Hidden;

            LoginForm l = new LoginForm();
            l.ShowDialog();
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


        //private void btnBack_Click(object sender, RoutedEventArgs e)
        //{
        //    if (counter == 4)
        //    {

        //        Slide2.Visibility = Visibility.Hidden;
        //        Slide4.Visibility = Visibility.Hidden;
        //        Slide1.Visibility = Visibility.Hidden;

        //        Slide3.Visibility = Visibility.Visible;

        //        counter--;
        //    }
        //    else if (counter == 3)
        //    {
        //        Slide1.Visibility = Visibility.Hidden;
        //        Slide3.Visibility = Visibility.Hidden;
        //        Slide4.Visibility = Visibility.Hidden;


        //        Slide2.Visibility = Visibility.Visible;

        //        counter--;

        //    }
        //    else if (counter == 2)
        //    {
        //        Slide1.Visibility = Visibility.Visible;
        //        Slide2.Visibility = Visibility.Hidden;
        //        Slide4.Visibility = Visibility.Hidden;


        //        Slide3.Visibility = Visibility.Hidden;

        //        counter--;

        //    }
        //    else
        //    {
        //        counter = 0;
        //        btnBack.IsEnabled = false;
        //        btnNext.IsEnabled = true;
        //    }
        //}

        //private void btnNext_Click(object sender, RoutedEventArgs e)
        //{
        //    if (counter == 0)
        //    {

        //        Slide2.Visibility = Visibility.Hidden;
        //        Slide3.Visibility = Visibility.Hidden;
        //        Slide4.Visibility = Visibility.Hidden;

        //        Slide1.Visibility = Visibility.Visible;

        //        counter++;
        //    }
        //    else if (counter == 1)
        //    {
        //        Slide1.Visibility = Visibility.Hidden;
        //        Slide3.Visibility = Visibility.Hidden;
        //        Slide4.Visibility = Visibility.Hidden;


        //        Slide2.Visibility = Visibility.Visible;

        //        counter++;

        //    }
        //    else if (counter == 2)
        //    {
        //        Slide1.Visibility = Visibility.Hidden;
        //        Slide2.Visibility = Visibility.Hidden;
        //        Slide4.Visibility = Visibility.Hidden;


        //        Slide3.Visibility = Visibility.Visible;

        //        counter++;

        //    }
        //    else if (counter == 3)
        //    {
        //        Slide1.Visibility = Visibility.Hidden;
        //        Slide2.Visibility = Visibility.Hidden;
        //        Slide3.Visibility = Visibility.Hidden;

        //        Slide4.Visibility = Visibility.Visible;

        //        counter++;

        //    }
        //    else
        //    {
        //        counter = 4;
        //        btnNext.IsEnabled = false;
        //        btnBack.IsEnabled = true;
        //    }
        //}
    }

    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));

        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }
}
