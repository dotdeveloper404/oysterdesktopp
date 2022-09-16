using App;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for SecureDevice.xaml
    /// </summary>
    public partial class SecureDevice : Window
    {
        public SecureDevice()
        {
            InitializeComponent();
        }

        private void btnSetup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string navigateUri = Settings.SiteUrl + "download/";

                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                logger.Error(ex.Message);
            }
        }
    }
}
