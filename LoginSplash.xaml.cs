using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoginSplash.xaml
    /// </summary>
    public partial class LoginSplash : Window
    {
        int counter = 1;

        public LoginSplash()
        {
            InitializeComponent();

            Closing += OnClosing;

        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {

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


        //private void btnSlide1_Click(object sender, RoutedEventArgs e)
        //{
        //    Slid2.Visibility = Visibility.Hidden;
        //    Slid3.Visibility = Visibility.Hidden;
        //    Slid4.Visibility = Visibility.Hidden;


        //    Slid1.Visibility = Visibility.Visible;
        //}

        //private void btnSlide2_Click(object sender, RoutedEventArgs e)
        //{
        //    Slid1.Visibility = Visibility.Hidden;
        //    Slid3.Visibility = Visibility.Hidden;
        //    Slid4.Visibility = Visibility.Hidden;


        //    Slid2.Visibility = Visibility.Visible;
        //}

        //private void btnSlide3_Click(object sender, RoutedEventArgs e)
        //{
        //    Slid1.Visibility = Visibility.Hidden;
        //    Slid2.Visibility = Visibility.Hidden;
        //    Slid4.Visibility = Visibility.Hidden;


        //    Slid3.Visibility = Visibility.Visible;
        //}

        //private void btnSlide4_Click(object sender, RoutedEventArgs e)
        //{
        //    Slid1.Visibility = Visibility.Hidden;
        //    Slid2.Visibility = Visibility.Hidden;
        //    Slid3.Visibility = Visibility.Hidden;

        //    Slid4.Visibility = Visibility.Visible;
           
        //}

        //private void GotoLogin_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Hide();
        //    this.Visibility = Visibility.Hidden;
        //    Login login = new Login();
        //    login.ShowDialog();
            
        //}

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

            //if (counter == 0)
            //{

            //    Slid2.Visibility = Visibility.Hidden;
            //    Slid3.Visibility = Visibility.Hidden;
            //    Slid4.Visibility = Visibility.Hidden;

            //    Slid1.Visibility = Visibility.Visible;
                
            //    counter++;
            //}
            //if (counter == 1)
            //{
            //    Slid1.Visibility = Visibility.Hidden;
            //    Slid3.Visibility = Visibility.Hidden;
            //    Slid4.Visibility = Visibility.Hidden;


            //    Slid2.Visibility = Visibility.Visible;

            //    counter++;

            //}
            //else if (counter == 2)
            //{
            //    Slid1.Visibility = Visibility.Hidden;
            //    Slid2.Visibility = Visibility.Hidden;
            //    Slid4.Visibility = Visibility.Hidden;


            //    Slid3.Visibility = Visibility.Visible;

            //    counter++;

            //}
            //else if (counter == 3)
            //{
            //    Slid1.Visibility = Visibility.Hidden;
            //    Slid2.Visibility = Visibility.Hidden;
            //    Slid3.Visibility = Visibility.Hidden;

            //    Slid4.Visibility = Visibility.Visible;

            //    counter++;

            //}
            //else
            //{

            //    counter = 1;
            //    this.Hide();
            //    this.Visibility = Visibility.Hidden;
            //    Login login = new Login();
            //    login.ShowDialog();
               

            //}

        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();
            this.Visibility = Visibility.Hidden;

            SignupForm s = new SignupForm();
            s.ShowDialog();
           
        }

        private void GotoLogin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();
            this.Visibility = Visibility.Hidden;

            LoginForm login = new LoginForm();
            login.ShowDialog();
        }
    }
}
