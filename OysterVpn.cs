using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using DotRas;

using System.IO; 
using OysterVPNModel;

using System.Net.NetworkInformation;
using System.Diagnostics;
using Microsoft.Win32;
using OysterVPNLibrary;
using App;
using log4net;
using System.Management;

namespace OysterVPN
{
    class OysterVpn
    {
        public static RasPhoneBook rasPhoneBook = new RasPhoneBook();
        public static EventHandler OpenVPNLogs_Tick;
        public static List<RasEntry> excluderasEntries = new List<RasEntry>();

        private static bool isKill;

        public static bool connect(string defaultProcotol, string ca, string tls, string dns)
        {

            bool isConnect = false;

            switch (defaultProcotol)
            {
                case "L2TP":
                case "PPTP":
                case "IKEV2":
                    openRasPhoneBook();
                 isConnect=initiateRasEntry(defaultProcotol);
                    new RasDialer
                    {
                        EntryName = Settings.getConnectionName(),
                        PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User),
                        Credentials = new NetworkCredential(Settings.getEmail(), Settings.getPassword())
                       
                    }.DialAsync();
                    break;

                case "TCP":
                case "UDP":
                     IsLogFileExist();
                 isConnect= ConfigOpenVpn(defaultProcotol, ca, tls, dns);
                    break;
            }

            return isConnect;
        }

        public static void openRasPhoneBook()
        {
            rasPhoneBook.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
        }

        private static bool initiateRasEntry(string protocol)
        {
            if ((from item in rasPhoneBook.Entries
                 where item.Name.Contains(Settings.appName)
                 select item).Count<RasEntry>() > 0)
            {
                removeEntries();
            }

          //  Settings.fetchSettings();

            Server server = Settings.getServer();

            IDictionary<string, RasVpnStrategy> dictionary = new Dictionary<string, RasVpnStrategy>
            {
                ["PPTP"] = RasVpnStrategy.PptpOnly,
                ["L2TP"] = RasVpnStrategy.L2tpOnly,
                ["IKEV2"] = RasVpnStrategy.IkeV2Only
            };
            IDictionary<string, string> serverProtocol = new Dictionary<string, string>
            {
                ["PPTP"] = "PPTP",
                ["L2TP"] = "L2TP",
                ["IKEV2"] = "IKEv2"
            };
            string[] source = new string[] { "L2TP", "IKEV2" };
            RasEntry entry = RasEntry.CreateVpnEntry(Settings.getConnectionName(), server == null ? "ost101.serverintoshell.com" : server.Dns, dictionary[protocol], RasDevice.GetDevices().First<RasDevice>(item => item.Name.Contains(serverProtocol[protocol])));
            if (source.Contains<string>(protocol))
            {
                entry.Options.UsePreSharedKey = true;
            }
            entry.Options.IPv6RemoteDefaultGateway = false;

            entry.NetworkProtocols.IPv6 = false;


            if (protocol == "L2TP")
            {
           
                entry.Options.RequirePap  = true;
                entry.Options.RequireMSChap2 = true;
                entry.Options.RequireEncryptedPassword = false;

            }
    
            
                 entry.Options.RequireEncryptedPassword = false;
                 entry.Options.RequireDataEncryption = false;
    
 
            // for split tunneling
            //  entry.Options.RemoteDefaultGateway = false;
          
            try
            {
                rasPhoneBook.Entries.Add(entry);
                entry.UpdateCredentials(RasPreSharedKey.Client, "psk123");
                return true;
            }
            catch (Exception exception)
            {
                ///logException(exception);
                initiateRasEntry(protocol);
            }
            if (source.Contains<string>(protocol))
            {
                entry.UpdateCredentials(RasPreSharedKey.Client, null);// server.ipsec.Trim());
            }

            return false;
        }

        public static DispatcherTimer OpenVpnLogs { get; }


        private static bool ConfigOpenVpn(string protocol, string ca, string tls, string dns)
        {
            try
            {

              

                string projectDirectory = Environment.CurrentDirectory;
           

                Process process = new Process();

                ProcessStartInfo startInfo = new ProcessStartInfo();

                ////    startInfo.Verb = "runas";
                startInfo.UseShellExecute = false;

                startInfo.FileName = "" + projectDirectory + "\\openvpn.exe";

                //for exclude
                StringBuilder sitesList = new StringBuilder();
                if (Settings.getSitesDontUseVpn())
                {
                    if (Settings.getSitesList() != null)
                    {
                        foreach (var item in Settings.getSitesList())
                        {
                            if (item != "")
                                sitesList.Append("--route " + item.Trim() + " 255.255.255.255 net_gateway ");
                        }
                        sitesList.Append("--redirect-gateway def1 ");
                    }
                }
                else if (Settings.getSitesUseVpn())
                {
                    if (Settings.getSitesList() != null)
                    {
                        sitesList.Append("--route-nopull ");

                        foreach (var item in Settings.getSitesList())
                        {
                            if (item != "")
                                sitesList.Append("--route " + item.Trim() + " 255.255.255.255 ");
                        }
                    }
                }

               string directory = Path.GetDirectoryName(Storage.UserDataFolder);//(SettingPath); //(Storage.UserDataFolder + @"\" + SettingPath);

                startInfo.Arguments = "--client " + sitesList + "--remote " + dns + " --proto " + protocol.ToLower() + " --comp-lzo --mssfix --persist-key --persist-tun --dev tun --auth SHA256 --auth-user-pass \"" + directory + "\\auth.conf\" --tls-client --ca \"" + projectDirectory + "\\assets\\data\\oyster\\" + ca + "\" --key-direction 1 --tls-auth \"" + projectDirectory + "\\assets\\data\\oyster\\" + tls + "\" --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --cipher AES-256-CBC --ping-timer-rem";

                //startInfo.Arguments = "--client " + sitesList + "--remote " + dns + " --proto " + protocol.ToLower() + " --comp-lzo --mssfix --persist-key --persist-tun --dev tun --auth SHA256 --auth-user-pass \"" + projectDirectory + "\\assets\\data\\oyster\\auth.txt\" --tls-client --ca \"" + projectDirectory + "\\assets\\data\\oyster\\" + ca + "\" --key-direction 1 --tls-auth \"" + projectDirectory + "\\assets\\data\\oyster\\" + tls + "\" --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --cipher AES-256-CBC --ping-timer-rem";
               
                startInfo.Verb = "runas";


                Settings.fetchSettings();
                //  System.Windows.Forms.MessageBox.Show(Settings.getEmail());
                //if(Settings.getEmail() == "")
                //{
                //    startInfo.CreateNoWindow = false;
                //}
                //else
                //{
                //   startInfo.CreateNoWindow = true;// false;
                // }

                startInfo.CreateNoWindow = true;

                process.StartInfo = startInfo;
                process.Start();

                return true;

            }
            catch (Exception exception)
            {
                logException(exception);
                return false;
            }
        }


        private static void ConfigOpenVpnOld(string protocol, string ca, string tls, string dns)
        {
            //  OpenVpnLogs.add_Tick(OpenVPNLogs_Tick);
            //   OpenVpnLogs.set_Interval(TimeSpan.FromSeconds(1.0));
            try
            {

                // This will get the current WORKING directory (i.e. \bin\Debug)
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

                //    OysterVpn.connect();

                Process process = new Process();


                ProcessStartInfo startInfo = new ProcessStartInfo();

                string ca1 = "ca1.ca";
                string ca2 = "ca2.ca";
                string tcp_tls = "tcp.tls";
                string udp_tls = "udp.tls";
                // string dns= "us-cf-ovtcp-01.jumptoserver.com 4443";

                string str4 = "4443";
                string str5 = "1194";


                // startInfo.Arguments = ("--client --dev tun --remote " + dns + " --proto " + protocol + " --port " + str4 + " --lport " + str5 + " --comp-lzo --auth SHA256  --auth-user-pass \"" + openvpnaccPath + "\" --tls-client --key-direction 1 --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --ca \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str + "\" --tls-auth \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str2 + "\" --cipher AES-256-CBC --keepalive 10 240 --block-outside-dns --persist-tun --resolv-retry infinite --persist-key  --status \"" + Storage.UserDataFolder + "\\status.dat\" 1  ");
                //startInfo.FileName = @"C:\D:\VS-Repo\OysterVPN\Resources\openvpn.exe";// @"C:\Program Files\OpenVPN\bin\openvpn.exe";
                //    startInfo.Arguments = "--client --dev tun --ca \"D:\\VS-Repo\\Resources\\data\\ca1.ca --auth-user-pass \"D:\\VS-Repo\\Resources\\data\\auth.txt\" --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --cipher AES-256-CBC --keepalive 10 240 --block-outside-dns --persist-tun --resolv-retry infinite --persist-key --comp-lzo --auth SHA256 --tls-auth \"D:\\VS-Repo\\Resources\\data\\tcp.tls\" ";


                ////    startInfo.Verb = "runas";
                startInfo.UseShellExecute = false;

                //process.StartInfo = startInfo;
                //process.Start();

                startInfo.FileName = "" + projectDirectory + "\\openvpn.exe";

                //   startInfo.Arguments ="--config " + projectDirectory + "\\"+protocol.ToLower()+".ovpn" ; // "--config \"C:\\Program Files\\OpenVPN\\config\\tcp.ovpn\"";
                //                startInfo.Arguments = "--client --remote " + dns + " --proto " + protocol.ToLower() + " --comp-lzo --mssfix --persist-key --persist-tun --dev tun --auth SHA256 --auth-user-pass \"" + projectDirectory + "\\assets\\data\\auth.txt\" --tls-client --ca \"" + projectDirectory + "\\assets\\data\\" + ca + "\" --key-direction 1 --tls-auth \"" + projectDirectory + "\\assets\\data\\" + tls + "\" --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --cipher AES-256-CBC --ping-timer-rem";

                //for exclude
                StringBuilder sitesList = new StringBuilder();
                if (Settings.getSitesDontUseVpn())
                {
                    if (Settings.getSitesList() != null)
                    {
                        foreach (var item in Settings.getSitesList())
                        {
                            if (item != "")
                                sitesList.Append("--route " + item.Trim() + " 255.255.255.255 net_gateway ");
                        }
                        sitesList.Append("--redirect-gateway def1 ");
                    }
                }
                else if (Settings.getSitesUseVpn())
                {
                    if (Settings.getSitesList() != null)
                    {
                        sitesList.Append("--route-nopull ");

                        foreach (var item in Settings.getSitesList())
                        {
                            if (item != "")
                                sitesList.Append("--route " + item.Trim() + " 255.255.255.255 ");
                        }
                    }
                }

                //startInfo.Arguments = "--client --route-nopull --route myexternalip.com 255.255.255.255 --remote " + dns + " --proto " + protocol.ToLower() + " --comp-lzo --mssfix --persist-key --persist-tun --dev tun --auth SHA256 --auth-user-pass \"" + projectDirectory + "\\assets\\data\\oyster\\auth.txt\" --tls-client --ca \"" + projectDirectory + "\\assets\\data\\oyster\\" + ca + "\" --key-direction 1 --tls-auth \"" + projectDirectory + "\\assets\\data\\oyster\\" + tls + "\" --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --cipher AES-256-CBC --ping-timer-rem";
                string directory = Path.GetDirectoryName(Storage.UserDataFolder);//(SettingPath); //(Storage.UserDataFolder + @"\" + SettingPath);

                 startInfo.Arguments = "--client "+sitesList+"--remote " + dns + " --proto " + protocol.ToLower() + " --comp-lzo --mssfix --persist-key --persist-tun --dev tun --auth SHA256 --auth-user-pass \"" + projectDirectory + "\\assets\\data\\oyster\\auth.txt\" --tls-client --ca \"" + projectDirectory + "\\assets\\data\\oyster\\" + ca + "\" --key-direction 1 --tls-auth \"" + projectDirectory + "\\assets\\data\\oyster\\" + tls + "\" --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --cipher AES-256-CBC --ping-timer-rem";

                // startInfo.Arguments = "--client --route myexternalip.com 255.255.255.255 net_gateway --redirect-gateway def1 --remote " + dns + " --proto " + protocol.ToLower() + " --comp-lzo --mssfix --persist-key --persist-tun --dev tun --auth SHA256 --auth-user-pass \"" + projectDirectory + "\\assets\\data\\oyster\\auth.txt\" --tls-client --ca \"" + projectDirectory + "\\assets\\data\\oyster\\" + ca + "\" --key-direction 1 --tls-auth \"" + projectDirectory + "\\assets\\data\\oyster\\" + tls + "\" --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --cipher AES-256-CBC --ping-timer-rem";

                //--route myexternalip.com 255.255.255.255 net_gateway --redirect-gateway def1
                //--route myexternalip.com 255.255.255.255 net_gateway --redirect-gateway def1
                //--route-nopull route myexternalip.com 255.255.255.255
                // startInfo.UseShellExecute = true;


                startInfo.Verb = "runas";
                startInfo.CreateNoWindow = true;

                process.StartInfo = startInfo;
                process.Start();

                //Settings.shell("\"" + AppDomain.CurrentDomain.BaseDirectory + "Resources\\openvpn\" " + ("--client --dev tun --remote " + dns + " --proto " + protocol.ToLower() + " --port " + str4 + " --lport " + str5 + " --comp-lzo --auth SHA256  --auth-user-pass \"" + Settings.openvpnaccPath + "\" --tls-client --key-direction 1 --tls-cipher TLS-DHE-RSA-WITH-AES-256-GCM-SHA384:TLS-DHE-RSA-WITH-AES-256-CBC-SHA256:TLS-DHE-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-DHE-RSA-WITH-AES-256-CBC-SHA:TLS-RSA-WITH-CAMELLIA-256-CBC-SHA:TLS-RSA-WITH-AES-256-CBC-SHA --ca \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str + "\" --tls-auth \"" + AppDomain.CurrentDomain.BaseDirectory + @"Resources\data\" + str2 + "\" --cipher AES-256-CBC --keepalive 10 240 --block-outside-dns --persist-tun --resolv-retry infinite --persist-key --log \"" + Settings.openvpnLogPath + "\" --status \"" + Storage.UserDataFolder + "\\status.dat\" 1  "));
                //   OpenVpnLogs.Start();
            }
            catch (Exception exception)
            {
                logException(exception);
            }
        }

        private static void IsLogFileExist()
        {
            try
            {
                if (System.IO.File.Exists(Settings.openvpnLogPath))
                {
                    System.IO.File.Delete(Settings.openvpnLogPath);
                }
            }
            catch (IOException exception)
            {
                logException(exception);
            }
        }

        public static void removeEntries()
        {
            removeEntries(false);
        }


        public static void removeEntries(bool disconnecting)
        {
            IEnumerable<RasEntry> rasEntries = from item in rasPhoneBook.Entries
                                               where item.Name.Contains(Settings.appName)
                                               select item;
            try
            {
                foreach (RasEntry entry in rasEntries)
                {
                    entry.Rename(entry.Name + "OLD");
                    if (!excluderasEntries.Contains(entry))
                    {
                        try
                        {
                            entry.Remove();
                            rasEntries = from item in rasPhoneBook.Entries
                                         where item.Name.Contains(Settings.appName)
                                         select item;
                        }
                        catch (RasException exception)
                        {
                            excluderasEntries.Add(entry);
                            // logException(exception);
                            if (IsConnected())
                            {
                                disconnect();
                            }
                            string str = Settings.getConnectionName();
                            if ((entry.Name == Settings.getConnectionName()) && !disconnecting)
                            {
                                Settings.activateBackupConnection();
                            }
                            removeEntries(disconnecting);
                        }
                        catch (Exception exception2)
                        {
                            //    logException(exception2);
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                //  logException(exception3);
            }
        }

        public static bool disconnect()
        {
             IEnumerable<RasEntry> enumerable = from item in rasPhoneBook.Entries
                                               where item.Name.Contains(Settings.appName)
                                               select item;
           
            DisconnectOpenVpn();

            foreach (RasConnection connection in RasConnection.GetActiveConnections())
            {
                if (connection.EntryName.Contains(Settings.appName))
                {
                    openRasPhoneBook();
                    connection.HangUp();
                    removeEntries();
                }
            }
       
            return true;
        }

        public static  void DisconnectOpenVpn()
        {
            try
            {

                Process.Start(new ProcessStartInfo()
                {
                    FileName = "taskKill",
                    Arguments = $"/f /im openvpn.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false
                }).WaitForExit();

            }
            catch (InvalidOperationException exception)
            {
                //   logException(exception);
            }
        }


        public static bool IsConnected()
        {
            bool flag = false;
            try
            {
                foreach (RasConnection connection in RasConnection.GetActiveConnections())
                {
                    if (connection.EntryName.Contains(Settings.appName) && (connection.GetConnectionStatus().ConnectionState == RasConnectionState.Connected))
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception exception)
            {
                //    logException(exception);
            }
            try
            {
                if (flag)
                {
                    return flag;
                }
                foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (interface2.Description.Contains("TAP-Windows") && (interface2.OperationalStatus == OperationalStatus.Up))
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception exception2)
            {
                //    logException(exception2);
            }
            return flag;
        }

        public static void ProtectDNSLeak()
        {
            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (interface2.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties iPProperties = interface2.GetIPProperties();
                    IPv4InterfaceProperties properties2 = interface2.GetIPProperties().GetIPv4Properties();
                    string[] strArray = new string[] { "10.8.8.8" };
                    if (((((properties2.Index <= 1) || interface2.Name.Contains(Settings.appName)) || interface2.Description.Contains("TAP-Windows")) || ((from item in iPProperties.DnsAddresses
                                                                                                                                                           where item.ToString() == "10.8.8.8"
                                                                                                                                                           select item).Count<IPAddress>() <= 0)) && (((properties2.Index > 1) && !interface2.Name.Contains(Settings.appName)) && !interface2.Description.Contains("TAP-Windows")))
                    {
                        IPAddressCollection dnsAddresses = iPProperties.DnsAddresses;
                        NetworkInterfaceCard card = new NetworkInterfaceCard
                        {
                            Name = interface2.Name,
                            Id = interface2.Id,
                            dns = iPProperties.DnsAddresses,
                            isDNSAuto = IsDNSAuto(interface2.Id)
                        };
                        Settings.addNetworkCard(card);
                        RemoveAllDNS(interface2.Name);
                        int num2 = 1;
                        foreach (string str in strArray)
                        {
                            AddDns(interface2.Name, str, num2.ToString());
                            num2++;
                        }
                    }
                }
            }
            Settings.isDNSLeakProtectionEnabled = true;
        }

        public static void UnprotectDNSLeak()
        {
            try
            {
                foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
                {
                    IPInterfaceProperties iPProperties = interface2.GetIPProperties();
                    IPv4InterfaceProperties properties2 = interface2.GetIPProperties().GetIPv4Properties();
                    string[] strArray = new string[] { "10.8.8.8" };
                    Server server = null;
                    Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\TCPIP6\Parameters", true).SetValue("DisabledComponents", 0xff, RegistryValueKind.DWord);
                    foreach (RasConnection connection in RasConnection.GetActiveConnections())
                    {
                        if (connection.EntryName == interface2.Name)
                        {
                            RasIPInfo projectionInfo = (RasIPInfo)connection.GetProjectionInfo(RasProjectionType.IP);
                            server = Settings.findServer(connection.GetConnectionStatus().RemoteEndPoint.ToString());
                        }
                    }
                    if ((((server == null) && (properties2.Index > 1)) && (!interface2.Name.Contains(Settings.appName) && !interface2.Description.Contains("TAP-Windows"))) && ((from item in iPProperties.DnsAddresses
                                                                                                                                                                                 where item.ToString() == "10.8.8.8"
                                                                                                                                                                                 select item).Count<IPAddress>() > 0))
                    {
                        RemoveAllDNS(interface2.Name);
                    }
                }
            }
            catch (Exception exception)
            {
                logException(exception);
            }
            try
            {
                List<NetworkInterfaceCard> list = Settings.getNetworkCard();
                foreach (NetworkInterfaceCard card in list)
                {
                    RemoveAllDNS(card.Name);
                    if (!card.isDNSAuto)
                    {
                        int num2 = 1;
                        foreach (IPAddress address in card.dns)
                        {
                            AddDns(card.Name, address.ToString(), num2.ToString());
                            num2++;
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                logException(exception2);
            }
        }



        private static bool IsDNSAuto(string NetworkAdapterGUID)
        {
            string str2 = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\" + NetworkAdapterGUID, "NameServer", null);
            return string.IsNullOrEmpty(str2);
        }

        private static void RemoveAllDNS(string name)
        {
            Settings.netsh(" interface ip delete dns name=\"" + name + "\" address=all");
        }


        private static void AddDns(string Name, string Dns, string Index)
        {
            Settings.netsh(" interface ip add dns name=\"" + Name + "\"  addr=" + Dns + " index=" + Index);
        }

        public static Server GetConnectedServer()
        {
            string ipAddress = null;
            Server server = new Server();
            try
            {
                foreach (RasConnection connection in RasConnection.GetActiveConnections())
                {
                    if (connection.EntryName.Contains(Settings.appName))
                    {
                        RasIPInfo projectionInfo = (RasIPInfo)connection.GetProjectionInfo(RasProjectionType.IP);
                        ipAddress = connection.GetConnectionStatus().RemoteEndPoint.ToString();
                    }
                }
                if (ipAddress == null)
                {
                    ipAddress = Settings.getconnectedServerIP();
                }
                server = Settings.findServer(ipAddress, Settings.defaultProtocol);
                Settings.setServer(server);
            }
            catch (Exception exception)
            {
                logException(exception);
            }
            return server;
        }

        public static void logException(Exception e)
        {
            ILog logger = log4net.LogManager.GetLogger("ErrorLog");
            logger.Error(e.Message);
            
        }


        public static void KillSwitchWindow()
        {
            // start listening for a network change
            isKill = false;
            List<string> adapaterList = new List<string>();
            adapaterList.Add("Ethernet");
            adapaterList.Add("Wifi");

            foreach (var item in adapaterList)
            {

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "netsh";
                info.Arguments = "interface set interface \"" + item + "\" DISABLED";
                info.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(info);
               // p.WaitForExit();
            }
            //  NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangeCallBack);
        }

        public static void AddressChangeCallBack(object sender,EventArgs e)
        {

            List<string> adapaterList = new List<string>();
            adapaterList.Add("Ethernet");
            adapaterList.Add("Wifi");
        
                
            foreach (var item in adapaterList)
            {

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "netsh";
                info.Arguments = "interface set interface \"" + item + "\" DISABLED";
                info.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(info);
                p.WaitForExit();
            }

            //foreach (NetworkInterface _interface in NetworkInterface.GetAllNetworkInterfaces())
            //{
            //    //ProcessStartInfo info = new ProcessStartInfo();
            //    //info.FileName = "netsh";
            //    //info.Arguments = "interface set interface \"" + interface2.Name + "\" DISABLED";
            //    //info.WindowStyle = ProcessWindowStyle.Hidden;
            //    //Process p = Process.Start(info);
            //    //p.WaitForExit();

            //   // IPv4InterfaceProperties ipv4= _interface.GetIPProperties().GetIPv4Properties();
            //   //if (((((ipv4.Index <= 1) || _interface.Name.Contains(Settings.appName)) || _interface.Description.Contains("TAP-Windows"))))
            //   // {
            //    if (_interface.OperationalStatus == OperationalStatus.Up)
            //    {
            //            isKill = true;

            //        ProcessStartInfo info = new ProcessStartInfo();
            //        info.FileName = "netsh";
            //        info.Arguments = "interface set interface \"" + _interface.Name + "\" DISABLED";
            //        info.WindowStyle = ProcessWindowStyle.Hidden;
            //        Process p = Process.Start(info);
            //        p.WaitForExit();

            //        //Process.Start("cmd.exe", "/c netsh interface set interface \"" + interface2.Name + "\" DISABLED");

            //        KillSwitchNow();
            //       }
            ////    }

            //}
        }


        private static void KillSwitchNow()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "ipconfig";
            info.Arguments = "/release";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(info);
            p.WaitForExit();
        }

        public static void DisableKillSwitch()
        {

            //ProcessStartInfo info = new ProcessStartInfo();
            //info.FileName = "netsh";
            //info.Arguments = "interface set interface Ethernet ENABLE";
            //info.WindowStyle = ProcessWindowStyle.Hidden;
            //Process p = Process.Start(info);
            //p.WaitForExit();

            List<string> adapaterList = new List<string>();
            adapaterList.Add("Ethernet");
            adapaterList.Add("Wifi");

            foreach (var item in adapaterList)
            {

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "netsh";
                info.Arguments = "interface set interface \"" + item + "\" ENABLE";
                info.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(info);
                p.WaitForExit();
            }


            //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            //{

            //    ProcessStartInfo info = new ProcessStartInfo();
            //    info.FileName = "netsh";
            //    info.Arguments = "interface set interface \"" + nic.Name + "\" ENABLE";
            //    info.WindowStyle = ProcessWindowStyle.Hidden;
            //    Process p = Process.Start(info);
            //    p.WaitForExit();

            //    //This should //enable the interfaces again but for some reason doesnt work.
            //}
            //MessageBox.Show("All network adapters have been re-enabled. InternetKillSwitch is OFF.");
        }


    }
}
