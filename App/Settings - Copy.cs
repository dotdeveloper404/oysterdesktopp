//namespace App
//{
//    using OysterVPNLibrary;
//    using OysterVPNModel;
//    //   using OysterVPNLibrary.OVPNService;
//    //using FastestVPNModel;
//    using System;
//    using System.Collections;
//    using System.Collections.Generic;
//    using System.Diagnostics;
//    using System.IO;
//    using System.Linq;
//    using System.Net.NetworkInformation;
//    using System.Runtime.Serialization;
//    using System.Runtime.Serialization.Formatters.Binary;
//    //using System.Windows.Forms;

//    [Serializable]
//    public class Settings : ISerializable
//    {
//        public static string ApiUrl = "https://api.staging.oystervpn.co/v1/";// "http://api.oystervpn.test/v1/";
//        public static bool skipAuthenticationCheck = false;
//        public static bool isDNSLeakProtectionEnabled = false;
//        public static List<NetworkInterfaceCard> NetworkCards = new List<NetworkInterfaceCard>();
//        public static bool appInitiated = false;
//        public static bool MainWindowVisible = false;
//        private static Product product = null;
//        private static string Name = "";
//        public static OysterVPNModel.Server PendingServerforConnection = null;
//        public static bool IntentionalDisconnect = false;
//        public static bool IntentionalConnect = false;
//        public static bool connecting = false;
//        public static bool disconnecting = false;
//        private static string Email;
//        private static string Password = "";
//        public static string SettingPath = "data.bin";
//        public static string openAppPath = Storage.UserDataFolder;
//        public static string DataPath = (Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\OysterVPN\data\");
//        public static string assetsPath = (Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\OysterVPN\assets\");
//        public static string openvpnLogPath = (Storage.UserDataFolder + @"\log.tmp");
//        public static string openvpnaccPath = (Storage.UserDataFolder + @"\acc.tmp");
//        private static OysterVPNModel.Server[] Servers = new OysterVPNModel.Server[0];
//        private static bool Remember = false;
//        public static string defaultProtocol = "IKEV2";
//        public static bool launchStartup = true;
//        public static bool internetKillSwitch = false;
//        public static bool redialAutomatically = true;
//        public static bool autoconnect = false;
//        public static bool disconnectonExit = false;
//        public static bool openvpnConnected = false;
//        public static string[] openvpnProtocols = new string[] { "TCP", "UDP" };
//        public static string connectedProtocol = "";
//        private static string ConnectionName = "OysterVPN";
//        public static string appName = "OysterVPN";
//        private static OysterVPNModel.Server Server = null;
//        public static bool connectionCompleted;
//        private static bool BackupConnection = false;
//        private static string BackupConnectionName = "OysterVPN-Backup";
//        private static int BackupConnectionSequence = 0;
//        private static string[] Protocols = new string[] { "PPTP", "L2TP", "IKEV2", "TCP", "UDP" };
//        public static DateTime? lastServerFetched = null;
//        private static bool loggedin = false;
//        public static string connectedServerIP;

//        public static bool SitesUseVpn = false;
//        public static bool SitesDontUseVpn = false;
//        public static string SplitTunnelSitesList;

//        public Settings()
//        {
//        }

//        public Settings(SerializationInfo info, StreamingContext context)
//        {
//            product = (Product)info.GetValue("static.product", typeof(Product));
//            Name = info.GetString("static.Name");
//            Email = info.GetString("static.Email");
//            Password = info.GetString("static.Password");
           
//            Servers=(OysterVPNModel.Server[])info.GetValue("static.Servers", typeof(OysterVPNModel.Server[]));
//            Remember = info.GetBoolean("static.Remember");
//            defaultProtocol = info.GetString("static.defaultProtocol");
//            launchStartup = info.GetBoolean("static.launchStartup");
//            internetKillSwitch = info.GetBoolean("static.internetKillSwitch");
//            Server = (OysterVPNModel.Server)info.GetValue("static.Server", typeof(OysterVPNModel.Server));
//            lastServerFetched = new DateTime?((DateTime)info.GetValue("static.lastServerFetched", typeof(DateTime)));
//            loggedin = info.GetBoolean("static.loggedin");
//            redialAutomatically = info.GetBoolean("static.redialAutomatically");
//            autoconnect = info.GetBoolean("static.autoconnect");
//            disconnectonExit = info.GetBoolean("static.disconnectonExit");
//            openvpnConnected = info.GetBoolean("static.openvpnConnected");
//            connectedServerIP = info.GetString("static.connectedServerIP");
//        }

//        public static void activateBackupConnection()
//        {
//            BackupConnection = true;
//            BackupConnectionSequence++;
//        }

//        public static void addNetworkCard(NetworkInterfaceCard card)
//        {
//            NetworkCards.Add(card);
//        }

//        //public static void allowfastestvpn()
//        //{
//        //    try
//        //    {
//        //        client.allowfastestvpn();
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        client.allowfastestvpn();
//        //    }
//        //}

//        private static bool sssBinaryDeSerialize()
//        {
        
//            using(StreamReader reader = new StreamReader(Storage.UserDataFolder + @"\" + SettingPath))
//            {
//                BinaryFormatter formater = new BinaryFormatter();
//                try
//                {
//                    Settings settings = (Settings)formater.Deserialize(reader.BaseStream);
//                }
//                catch(SerializationException ex)
//                {
//                    throw new SerializationException(((object)ex).ToString() + "\n" + ex.Source);

//                }
//            }

//            return true;

//        }



//        private static bool BinaryDeSerialize()
//        {
//            FileStream serializationStream = new FileStream(Storage.UserDataFolder + @"\" + SettingPath, FileMode.Open, FileAccess.Read);

//            BinaryFormatter formatter = new BinaryFormatter();
//            try
//            {
//            Settings settings = (Settings)formatter.Deserialize(serializationStream);
//            }
//            catch (SerializationException exception)
//            {
//              //  vpn.logException(exception);
//                serializationStream.Dispose();
//             //   File.Delete(getsettingFilePath());
//                return false;
//            }
//            serializationStream.Dispose();
//            return true;
//        }

//        private static void BinarySerialize()
//        {
//            try
//            {
//                Settings graph = new Settings();
//                FileStream serializationStream = new FileStream(Storage.UserDataFolder + @"\" + SettingPath, FileMode.Create);
//                new BinaryFormatter().Serialize(serializationStream, graph);
//                serializationStream.Dispose();
//            }
//            catch 
//            {
//            }
//        }

//        //[AsyncStateMachine(typeof(<checkupdates>d__43)), DebuggerStepThrough]
//        //public static Task checkupdates(Version curVersion)
//        //{
//        //    <checkupdates>d__43 stateMachine = new <checkupdates>d__43 {
//        //        curVersion = curVersion,
//        //        <>t__builder = AsyncTaskMethodBuilder.Create(),
//        //        <>1__state = -1
//        //    };
//        //    stateMachine.<>t__builder.Start<<checkupdates>d__43>(ref stateMachine);
//        //    return stateMachine.<>t__builder.Task;
//        //}

//        //public static void closeInternet()
//        //{
//        //    try
//        //    {
//        //        client.closeInternet();
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        client.closeInternet();
//        //    }
//        //}

//        //public static void closetcpudp()
//        //{
//        //    try
//        //    {
//        //        client.closetcpudp();
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        client.closetcpudp();
//        //    }
//        //}

//        //[AsyncStateMachine(typeof(<fetchservers>d__77)), DebuggerStepThrough]
//        //public static Task<bool> fetchservers()
//        //{
//        //    <fetchservers>d__77 stateMachine = new <fetchservers>d__77 {
//        //        <>t__builder = AsyncTaskMethodBuilder<bool>.Create(),
//        //        <>1__state = -1
//        //    };
//        //    stateMachine.<>t__builder.Start<<fetchservers>d__77>(ref stateMachine);
//        //    return stateMachine.<>t__builder.Task;
//        //}

//        public static bool fetchSettings() =>
//            BinaryDeSerialize();

//        public static Server findServer(string ipAddress) =>
//            (from server in Servers
//             where server.Ip == ipAddress
//             orderby server.IsFavourited descending, server.Name
//             select server).First<OysterVPNModel.Server>();

//        public static Server findServer(string ipAddress, string protocol) =>
//            (from server in Servers
//             where (server.Ip == ipAddress) && (server.Protocol == protocol)
//             orderby server.IsFavourited descending, server.Name
//             select server).First<OysterVPNModel.Server>();

//        public static OysterVPNModel.Server firstServer(string protocol) =>
//            (from server in Servers
//             where server.Protocol == protocol
//             orderby server.IsFavourited descending, server.Name
//             select server).First<OysterVPNModel.Server>();

//        public static bool getautoconnect() =>
//            autoconnect;

//        public static string getconnectedServerIP() =>
//            connectedServerIP;

//        public static string getConnectionName()
//        {
//            if (BackupConnection)
//            {
//                return (BackupConnectionName + BackupConnectionSequence.ToString());
//            }
//            return ConnectionName;
//        }

//        //public static string getServerAddress()
//        //{
//        //    "us-cf-ike-01.jumptoserver.com"
//        //}

//        public static bool getdisconnectonExit() =>
//            disconnectonExit;

//        public static string getEmail() =>
//            Security.Decrypt(Email);

//        public static bool getinternetKillSwitch() =>
//            internetKillSwitch;

//        public static bool getlaunchStartup() =>
//            launchStartup;

//        public static bool getLoggedin() =>
//            loggedin;

//        public static string getName() =>
//            Security.Decrypt(Name);

//        public static List<NetworkInterfaceCard> getNetworkCard() =>
//            NetworkCards;

//        public void GetObjectData(SerializationInfo info, StreamingContext context)
//        {
//            info.AddValue("static.product", product, typeof(Product));
//            info.AddValue("static.Name", Name, typeof(string));
//            info.AddValue("static.Email", Email, typeof(string));
//            info.AddValue("static.Password", Password, typeof(string));
//            info.AddValue("static.Servers", Servers, typeof(OysterVPNModel.Server[]));
//            info.AddValue("static.Remember", Remember, typeof(bool));
//            info.AddValue("static.defaultProtocol", defaultProtocol, typeof(string));
//            info.AddValue("static.launchStartup", launchStartup, typeof(bool));
//            info.AddValue("static.internetKillSwitch", internetKillSwitch, typeof(bool));
//            info.AddValue("static.Server", Server, typeof(OysterVPNModel.Server));
//            info.AddValue("static.lastServerFetched", lastServerFetched, typeof(DateTime));
//            info.AddValue("static.loggedin", loggedin, typeof(bool));
//            info.AddValue("static.redialAutomatically", redialAutomatically, typeof(bool));
//            info.AddValue("static.autoconnect", autoconnect, typeof(bool));
//            info.AddValue("static.disconnectonExit", disconnectonExit, typeof(bool));
//            info.AddValue("static.openvpnConnected", openvpnConnected, typeof(bool));
//            info.AddValue("static.connectedServerIP", connectedServerIP, typeof(string));
//        }

//        public static string getSitesList() => SplitTunnelSitesList;
//        public static bool getSitesDontUseVpn() => SitesDontUseVpn;
//        public static bool getSitesUseVpn() => SitesUseVpn;

//        public static bool getopenvpnConnected() =>
//            openvpnConnected;

//        public static string getPassword() =>
//            Security.Decrypt(Password);

//        public static Product getProduct() =>
//            product;

//        public static string[] getProtocols() =>
//            Protocols;

//        public static bool getredialAutomatically() =>
//            redialAutomatically;

//        public static bool getRemember() =>
//            Remember;

//        public static OysterVPNModel.Server getServer() =>
//            Server;

//        public static OysterVPNModel.Server[] getServers() =>
//            Servers;

//        public static OysterVPNModel.Server[] getServers(string protocol)
//        {
//            try
//            {
//                return (from server in Servers
//                        where server.Protocol == protocol
//                        orderby server.IsFavourited descending, server.Name
//                        select server).ToArray<OysterVPNModel.Server>();
//            }
//            catch
//            {
//                return new OysterVPNModel.Server[0];
//            }
//        }

//        public static string getsettingFilePath() =>
//            Storage.UserDataFolder + @"\" + SettingPath;

//        //[AsyncStateMachine(typeof(< init > d__76)), DebuggerStepThrough]
//        //public static Task<bool> init()
//        //{
//        //    < init > d__76 stateMachine = new < init > d__76 {
//        //        <> t__builder = AsyncTaskMethodBuilder<bool>.Create(),
//        //        <> 1__state = -1
//        //          };
//        //    stateMachine.<> t__builder.Start << init > d__76 > (ref stateMachine);
//        //    return stateMachine.<> t__builder.Task;
//        //}

//        //public static bool isallowfastestvpn()
//        //{
//        //    try
//        //    {
//        //        return client.isallowfastestvpn();
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        return client.isallowfastestvpn();
//        //    }
//        //}

//        //public static bool iscloseInternet()
//        //{
//        //    try
//        //    {
//        //        return client.iscloseInternet();
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        return client.iscloseInternet();
//        //    }
//        //}

//        public static bool isTapDriverInstalled()
//        {
//            openvpnCredentialscheck();
//            bool flag = false;
//            string id = "";
//            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
//            {
//                if (interface2.Description.Contains("TAP-Windows"))
//                {
//                    id = interface2.Id;
//                    flag = true;
//                    break;
//                }
//            }
//            if (flag)
//            {
//            }
//            return flag;
//        }

//        //public static void killProcess(string processname)
//        //{
//        //    try
//        //    {
//        //        client.killProcess(processname);
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        client.killProcess(processname);
//        //    }
//        //}

//        public static void netsh(string command)
//        {
//            try
//            {
//                Process.Start("cmd", "/C" + command + "");

//                //client.netsh(command);
//            }
//            catch
//            {
//                Process.Start("cmd", "/C" + command + "");
//            }
//        }

//        //public static void openInternet()
//        //{
//        //    try
//        //    {
//        //        client.openInternet();
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        client.openInternet();
//        //    }
//        //}

//        //public static void opentcpudp()
//        //{
//        //    try
//        //    {
//        //        client.opentcpudp();
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        client.opentcpudp();
//        //    }
//        //}

//        //public static void openVPN()
//        //{
//        //    try
//        //    {
//        //        client.openVPN();
//        //    }
//        //    catch
//        //    {
//        //        client = new FVPNServiceClient();
//        //        openVPN();
//        //    }
//        //}

//        public static void openvpnCredentialscheck()
//        {
//            string[] contents = new string[] { getEmail(), getPassword() };
//            try
//            {
//                File.Delete(openvpnaccPath);
//                File.WriteAllLines(openvpnaccPath, contents);
//            }
//            catch
//            {
//            }
//        }

//        public static void PrepareTapDriver()
//        {
//           // MessageBox.Show("Tap Driver is not installed, click OK to install it.");
//            Process process1 = new Process();
//            ProcessStartInfo info1 = new ProcessStartInfo
//            {
//                FileName = AppDomain.CurrentDomain.BaseDirectory + @"\Resources\driver\tap.exe"
//            };
//            process1.StartInfo = info1;
//            Process process = process1;
//            process.Start();
//            process.WaitForExit();
//        }

//        private static void saveSettings()
//        {
//      BinarySerialize();
//        }

//        public static void setautoconnect(bool value)
//        {
//            autoconnect = value;
//            saveSettings();
//        }

//        public static void setconnectedServerIP(string ipaddress)
//        {
//            connectedServerIP = ipaddress;
//            saveSettings();
//        }

//        public static void setdisconnectonExit(bool value)
//        {
//            disconnectonExit = value;
//            saveSettings();
//        }

//        public static void setEmail(string Email)
//        {
//            Settings.Email = Security.Encrypt(Email);
//            saveSettings();
//        }

//        public static void setinternetKillSwitch(bool value)
//        {
//            internetKillSwitch = value;
//            saveSettings();
//        }

//        public static void setlaunchStartup(bool value)
//        {
//            launchStartup = value;
//            saveSettings();
//        }

//        public static void setLoggedin(bool loggedin)
//        {
//            Settings.loggedin = loggedin;
//            saveSettings();
//        }

//        public static void setName(string Name)
//        {
//            Settings.Name = Security.Encrypt(Name);
//            saveSettings();
//        }

//        public static void setopenvpnConnected(bool connect)
//        {
//            openvpnConnected = connect;
//            saveSettings();
//        }


//        public static void setSitesUseVpn(bool SitesUsevpn)
//        {
//            Settings.SitesUseVpn = SitesUsevpn;
//            saveSettings();
//        }
//        public static void setSitesDontUseVpn(bool SitesDontUsevpn)
//        {
//            Settings.SitesDontUseVpn = SitesDontUsevpn;
//            saveSettings();
//        }

//        public static void setSitesList(string sitesList)
//        {
//            Settings.SplitTunnelSitesList = sitesList;
//            saveSettings();
//        }



//        public static void setPassword(string Password)
//        {
//            Settings.Password = Security.Encrypt(Password);
//            saveSettings();
//        }

//        public static void setProduct(Product product)
//        {
//            Settings.product = product;
//            saveSettings();
//        }

//        public static void setProtocol(string Protocol)
//        {
//            defaultProtocol = Protocol;
//            saveSettings();
//        }

//        public static void setredialAutomatically(bool value)
//        {
//            redialAutomatically = value;
//            saveSettings();
//        }

//        public static void setRemember(bool remember)
//        {
//            Remember = remember;
//            saveSettings();
//        }

//        public static void setServer(OysterVPNModel.Server Server)
//        {
//            Settings.Server = Server;
//            saveSettings();
//        }

//        public static void setServerConnect()
//        {
//            Server.IsConnected = true;
//        }

//        public static void setServerDisconnect()
//        {
//            Server.IsConnected = false;
//        }

//        public static void setServers(OysterVPNModel.Server[] Servers)
//        {
//            Settings.Servers = Servers;
//            saveSettings();
//        }

      
//    }
//}

