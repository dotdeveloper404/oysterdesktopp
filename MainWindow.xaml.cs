using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OysterVPN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PPTP_Click(object sender, RoutedEventArgs e)
        {
            OysterVpn.connect("PPTP","","","");
            MessageBox.Show("Your've connected to server PPTP");
        }

        private void l2tp_Click(object sender, RoutedEventArgs e)
        {
            OysterVpn.connect("L2TP","","","");
            MessageBox.Show("Your've connected to server L2TP");
        }

        private void ikev2_Click(object sender, RoutedEventArgs e)
        {
            OysterVpn.connect("IKEV2","","","");
            MessageBox.Show("Your've connected to server IKEV2");
        }

        private void udp_Click(object sender, RoutedEventArgs e)
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            // startInfo.FileName = @"C:\Users\Administrator\source\repos\OysterVPN\openvpn.exe";// @"C:\Program Files\OpenVPN\bin\openvpn.exe";
            // startInfo.Arguments = "--config \"C:\\Program Files\\OpenVPN\\config\\udp.ovpn\"";

            startInfo.FileName = "" + projectDirectory + "\\openvpn.exe";
            startInfo.Arguments = "--config " + projectDirectory + "\\udp.ovpn";

            startInfo.Verb = "runas";
            process.StartInfo = startInfo;

            process.Start();
            MessageBox.Show("Your've connected to server UDP");
        }

        private void tcp_Click(object sender, RoutedEventArgs e)
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            //    OysterVpn.connect();

            Process process = new Process();


            ProcessStartInfo startInfo = new ProcessStartInfo();
            //string openvpnaccPath = @"C:\\Program Files\\OpenVPN\\config\\auth.txt\";

            string str = "ca1.ca";
            string str2 = "tcp.tls";
            string dns = "";//Settings.getServer().dns;
            string str4 = "";// Settings.getServer().port.ToString();
            string str5 = "1194";
            string protocol = "tcp";
            startInfo.FileName = "" + projectDirectory + "\\openvpn.exe";//@"C:\Users\Administrator\source\repos\OysterVPN\Resources\openvpn.exe";// @"C:\Program Files\OpenVPN\bin\openvpn.exe";
                                                                         //  startInfo.Arguments = "--config \"C:\\Program Files\\OpenVPN\\config\\tcp.ovpn\"";

            // startInfo.Arguments = ("--client --dev tun --remote " + dns + " --proto " + protocol + " --port " + str4 + " --lport " + str5 + " --comp-lzo --auth SHA256  --auth-user-pass \"" + openvpnaccPath + "\" --tls-client --key-direction 1 --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --ca \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str + "\" --tls-auth \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str2 + "\" --cipher AES-256-CBC --keepalive 10 240 --block-outside-dns --persist-tun --resolv-retry infinite --persist-key  --status \"" + Storage.UserDataFolder + "\\status.dat\" 1  ");

            //startInfo.Arguments = "--client --dev tun --ca \"C:\\Users\\Administrator\\source\\repos\\OysterVPN\\bin\\Debug\\Resources\\data\\ca1.ca --auth-user-pass \"C:\\Program Files\\OpenVPN\\config\\auth.txt\" --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --cipher AES-256-CBC --keepalive 10 240 --block-outside-dns --persist-tun --resolv-retry infinite --persist-key --comp-lzo --auth SHA256 --tls-auth \"C:\\Users\\Administrator\\source\\repos\\OysterVPN\\bin\\Debug\\Resources\\data\\tcp.tls\" ";
            startInfo.Arguments = "--config " + projectDirectory + "\\tcp.ovpn"; // "--config \"C:\\Program Files\\OpenVPN\\config\\tcp.ovpn\"";
            startInfo.Verb = "runas";
            startInfo.CreateNoWindow = true;

            process.StartInfo = startInfo;
            process.Start();


            //Settings.shell("\"" + AppDomain.CurrentDomain.BaseDirectory + "Resources\\openvpn\" " + ("--client --dev tun --remote " + dns + " --proto " + protocol.ToLower() + " --port " + str4 + " --lport " + str5 + " --comp-lzo --auth SHA256  --auth-user-pass \"" + openvpnaccPath + "\" --tls-client --key-direction 1 --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --ca \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str + "\" --tls-auth \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str2 + "\" --cipher AES-256-CBC --keepalive 10 240 --block-outside-dns --persist-tun --resolv-retry infinite --persist-key  --status \"" + Storage.UserDataFolder + "\\status.dat\" 1  "));


            //Settings.shell("\"" + AppDomain.CurrentDomain.BaseDirectory + "Resources\\openvpn\" " + ("--client --dev tun --remote " + dns + " --proto " + protocol + " --port " + str4 + " --lport " + str5 + " --comp-lzo --auth SHA256  --auth-user-pass \"" + openvpnaccPath + "\" --tls-client --key-direction 1 --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --ca \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str + "\" --tls-auth \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str2 + "\" --cipher AES-256-CBC --keepalive 10 240 --block-outside-dns --persist-tun --resolv-retry infinite --persist-key --log \"" + Settings.openvpnLogPath + "\" --status \"" + Storage.UserDataFolder + "\\status.dat\" 1  "));

            MessageBox.Show("You've connected to server TCP");
        }

        private void disconnet_Click(object sender, RoutedEventArgs e)
        {
            OysterVpn.disconnect();
        }
    }
}
