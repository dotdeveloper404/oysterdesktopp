using System;
using System.Collections.Generic;
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
    }
}
