using App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SplitTunnel.xaml
    /// </summary>
    public partial class SplitTunnel : Window
    {
        public SplitTunnel()
        {
            InitializeComponent();

            if(Settings.getSitesList() != null)
            {
                foreach (var item in Settings.getSitesList())
                {
                    SitesList.Text += item + "\r\n";
                }
            }
          

            UseVpn.IsChecked = Settings.getSitesUseVpn();
            DontUseVpn.IsChecked = Settings.getSitesDontUseVpn();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            List<string> list = new List<string>(Regex.Split(SitesList.Text, Environment.NewLine));

            if (AllAppsUseVpn.IsChecked == true)
            {
                Settings.setAllSitesUseVpn(true);
            }
            else
            {
                Settings.setAllSitesUseVpn(false);
            }

            if (DontUseVpn.IsChecked==true)
            {
                Settings.setSitesDontUseVpn(true);
            }
            else
            {
                Settings.setSitesDontUseVpn(false);
            }

             if (UseVpn.IsChecked == true)
            {
                Settings.setSitesUseVpn(true);
            }
            else
            {
                Settings.setSitesUseVpn(false);
            }

              Settings.setSitesList(list.Select(x=>x.Trim()).ToList());

            this.Close();

           

        }
    }
}
