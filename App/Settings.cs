namespace App
{
    using OysterVPNLibrary;
    using OysterVPNModel;
    using OysterVPN;
    //   using OysterVPNLibrary.OVPNService;
    //using FastestVPNModel;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    //using System.Windows.Forms;

    [Serializable]
    public class Settings : ISerializable
    {
        public static string ApiUrl =  "https://api.oystervpn.com/v1/";
        public static string ApiAssetUrl =  "https://api.oystervpn.com/";
        public static string SiteUrl = "https://oystervpn.com/";
        public static string SupportUrl = "https://support.oystervpn.com/";
        public static bool skipAuthenticationCheck = false;
        public static bool isDNSLeakProtectionEnabled = false;
        public static List<NetworkInterfaceCard> NetworkCards = new List<NetworkInterfaceCard>();
        public static bool appInitiated = false;
        public static bool MainWindowVisible = false;
        private static Product product = null;
        private static string Name = "";
        public static OysterVPNModel.Server PendingServerforConnection = null;
        public static bool IntentionalDisconnect = false;
        public static bool IntentionalConnect = false;
        public static bool connecting = false;
        public static bool disconnecting = false;
        private static string Email;
        private static string Phone;
        private static string Token;
        private static string Password = "";
        private static int UserId = 0;
      //  private static string SettingPath = Environment.CurrentDirectory + "\\assets\\data\\oyster\\data.bin";
        public static string SettingPath = "data.bin";
        public static string openAppPath = Storage.UserDataFolder;
        public static string DataPath = (Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\OysterVPN\data\");
        public static string assetsPath = (Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\OysterVPN\assets\");
        public static string openvpnLogPath = (Storage.UserDataFolder + @"\log.tmp");
        public static string openvpnaccPath = (Storage.UserDataFolder + @"\acc.tmp");
        private static OysterVPNModel.Server[] Servers;// = new OysterVPNModel.Server[];
        private static bool Remember = false;
        public static string defaultProtocol = "IKEV2";
        public static string Protocol = "";
        public static string Avatar = "";
        public static bool launchStartup = false;
        public static bool StartVpnMinimize = false;
        public static bool ConnectLastUsedServer = false;
        public static bool internetKillSwitch = false;
        public static bool redialAutomatically = true;
        public static bool autoconnect = false;
        public static bool disconnectonExit = false;
        public static bool openvpnConnected = false;
        public static string[] openvpnProtocols = new string[] { "TCP", "UDP" };
        public static string connectedProtocol = "";
        private static string ConnectionName = "OysterVPN";
        public static string appName = "OysterVPN";
        private static OysterVPNModel.Server Server = null;
        private static OysterVPNModel.CurrentLocation CurrentLocation = null;
        private static OysterVPNModel.Server LastUsedServer = null;
        public static bool connectionCompleted;
        private static bool BackupConnection = false;
        private static string BackupConnectionName = "OysterVPN-Backup";
        private static int BackupConnectionSequence = 0;
        private static string[] Protocols = new string[] { "PPTP", "L2TP", "IKEV2", "TCP", "UDP" };
        public static DateTime? lastServerFetched = null;
        private static bool loggedin = false;
        public static string connectedServerIP;

        private static bool AllAppsUseVpn = true;
        private static bool SitesUseVpn = false;
        private static bool SitesDontUseVpn = false;
        private static List<string> SplitTunnelSitesList;

  

        public static string AuthToken { get; set; }
        public static bool IsUpdateAvailable { get; set; }

        public Settings()
        {
        }

        public Settings(SerializationInfo info, StreamingContext context)
        {
            //  product = (Product)info.GetValue("static.product", typeof(Product));
                Name = info.GetString("static.Name");
                Email = info.GetString("static.Email");
                Password = info.GetString("static.Password");
                Phone = info.GetString("static.Phone");
                Token = info.GetString("static.Token");
                UserId = info.GetInt32("static.UserId");
                Protocol = info.GetString("static.Protocol");
                Avatar = info.GetString("static.Avatar");
            //  Servers = (OysterVPNModel.Server[])info.GetValue("static.Servers", typeof(OysterVPNModel.Server[]));
            //Remember = info.GetBoolean("static.Remember");
            //defaultProtocol = info.GetString("static.defaultProtocol");
                StartVpnMinimize = info.GetBoolean("static.StartVpnMinimize");
                ConnectLastUsedServer = info.GetBoolean("static.ConnectLastUsedServer");
                launchStartup = info.GetBoolean("static.launchStartup");
                internetKillSwitch = info.GetBoolean("static.internetKillSwitch");
                SitesUseVpn = info.GetBoolean("static.SitesUseVpn");
                SitesDontUseVpn = info.GetBoolean("static.SitesDontUseVpn");
                AllAppsUseVpn = info.GetBoolean("static.AllAppsUseVpn");
                SplitTunnelSitesList = (List<string>)info.GetValue("static.SplitTunnelSitesList", typeof(List<string>));
                Server = (OysterVPNModel.Server)info.GetValue("static.Server", typeof(OysterVPNModel.Server));
                LastUsedServer = (OysterVPNModel.Server)info.GetValue("static.LastUsedServer", typeof(OysterVPNModel.Server));
                CurrentLocation = (OysterVPNModel.CurrentLocation)info.GetValue("static.CurrentLocation", typeof(OysterVPNModel.CurrentLocation));
                Servers = (Server[])info.GetValue("static.Servers", typeof(Array));  
            //lastServerFetched = new DateTime?((DateTime)info.GetValue("static.lastServerFetched", typeof(DateTime)));
                //loggedin = info.GetBoolean("static.loggedin");
                //redialAutomatically = info.GetBoolean("static.redialAutomatically");
                //autoconnect = info.GetBoolean("static.autoconnect");
                //disconnectonExit = info.GetBoolean("static.disconnectonExit");
                //openvpnConnected = info.GetBoolean("static.openvpnConnected");
                //connectedServerIP = info.GetString("static.connectedServerIP");

        }

        public static void activateBackupConnection()
        {
            BackupConnection = true;
            BackupConnectionSequence++;
        }

        public static void addNetworkCard(NetworkInterfaceCard card)
        {
            NetworkCards.Add(card);
        }

       
        //private static bool sssBinaryDeSerialize()
        //{

        //    using (StreamReader reader = new StreamReader(Storage.UserDataFolder + @"\" + SettingPath))
        //    {
        //        BinaryFormatter formater = new BinaryFormatter();
        //        try
        //        {
        //            Settings settings = (Settings)formater.Deserialize(reader.BaseStream);
        //        }
        //        catch (SerializationException ex)
        //        {
        //            throw new SerializationException(((object)ex).ToString() + "\n" + ex.Source);

        //        }
        //    }

        //    return true;

        //}


        private static bool BinaryDeSerialize()
        {
             

              string directory = Path.GetDirectoryName(Storage.UserDataFolder + @"\" + SettingPath);//(SettingPath); //(Storage.UserDataFolder + @"\" + SettingPath);
            string filePath = Path.Combine(directory);

            if (File.Exists(directory + @"\data.bin"))
            {

                FileStream serializationStream = new FileStream(Storage.UserDataFolder + @"\" + SettingPath, FileMode.Open, FileAccess.Read);//(SettingPath,FileMode.Open,FileAccess.Read);//(Storage.UserDataFolder + @"\" + SettingPath, FileMode.Open, FileAccess.Read);

             try
            {
               
                BinaryFormatter formatter = new BinaryFormatter();
                Settings settings = (Settings)formatter.Deserialize(serializationStream);
                serializationStream.Dispose();
            }
            catch (Exception exception)
            //  catch (SerializationException exception)
            {
                //  vpn.logException(exception);
                serializationStream.Dispose();
                File.Delete(getsettingFilePath());
                return false;
            }
          //  serializationStream.Dispose();
            return true;
            }

            return false;
        }

        private static void BinarySerialize()
        {
            try
            {
                Settings graph = new Settings();
                FileStream serializationStream = new FileStream(Storage.UserDataFolder + @"\" + SettingPath, FileMode.Create);//(SettingPath, FileMode.Create);//(Storage.UserDataFolder + @"\" + SettingPath, FileMode.Create);
                new BinaryFormatter().Serialize(serializationStream, graph);
                serializationStream.Dispose();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static bool fetchSettings() =>
            BinaryDeSerialize();

        public static Server findServer(string ipAddress) =>
            (from server in Servers
             where server.Ip == ipAddress
             orderby server.IsFavourited descending, server.Name
             select server).First<OysterVPNModel.Server>();

        public static Server findServer(string ipAddress, string protocol) =>
            (from server in Servers
             where (server.Ip == ipAddress) && (server.Protocol == protocol)
             orderby server.IsFavourited descending, server.Name
             select server).First<OysterVPNModel.Server>();

        public static OysterVPNModel.Server firstServer(string protocol) =>
            (from server in Servers
             where server.Protocol == protocol
             orderby server.IsFavourited descending, server.Name
             select server).First<OysterVPNModel.Server>();

        public static bool getautoconnect() =>
            autoconnect;

        public static string getconnectedServerIP() =>
            connectedServerIP;

        public static string getConnectionName()
        {
            if (BackupConnection)
            {
                return (BackupConnectionName + BackupConnectionSequence.ToString());
            }
            return ConnectionName;
        }

        //public static string getServerAddress()
        //{
        //    "us-sd-sd-01.sd.com"
        //}

        public static bool getdisconnectonExit() =>
            disconnectonExit;

        public static string getEmail() =>
            Security.Decrypt(Email);

        public static bool getinternetKillSwitch() =>
            internetKillSwitch;

        public static bool getlaunchStartup() =>
            launchStartup;

        public static bool getLoggedin() =>
            loggedin;

        public static string getName() =>
            Security.Decrypt(Name);

        public static string getPhone() =>
           Security.Decrypt(Phone);

        public static string getToken() =>
         Security.Decrypt(Token);

        public static List<NetworkInterfaceCard> getNetworkCard() =>
            NetworkCards;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
         //   info.AddValue("static.product", product, typeof(Product));
            info.AddValue("static.Name", Name, typeof(string));
            info.AddValue("static.Email", Email, typeof(string));
            info.AddValue("static.Password", Password, typeof(string));
            info.AddValue("static.Phone", Phone, typeof(string));
            info.AddValue("static.Token", Token, typeof(string));
            info.AddValue("static.Protocol", Protocol, typeof(string));
            info.AddValue("static.Avatar", Avatar, typeof(string));
            info.AddValue("static.UserId", UserId, typeof(int));
            //info.AddValue("static.Servers", Servers, typeof(OysterVPNModel.Server[]));
            //info.AddValue("static.Remember", Remember, typeof(bool));
            //info.AddValue("static.defaultProtocol", defaultProtocol, typeof(string));
            info.AddValue("static.launchStartup", launchStartup, typeof(bool));
            info.AddValue("static.internetKillSwitch", internetKillSwitch, typeof(bool));
            info.AddValue("static.StartVpnMinimize", StartVpnMinimize, typeof(bool));
            info.AddValue("static.ConnectLastUsedServer", ConnectLastUsedServer, typeof(bool));
            info.AddValue("static.AllAppsUseVpn", AllAppsUseVpn, typeof(bool));
            info.AddValue("static.SitesUseVpn", SitesUseVpn, typeof(bool));
            info.AddValue("static.SitesDontUseVpn", SitesDontUseVpn, typeof(bool));
            info.AddValue("static.SplitTunnelSitesList", SplitTunnelSitesList, typeof(List<string>));
            info.AddValue("static.Server", Server, typeof(OysterVPNModel.Server));
            info.AddValue("static.LastUsedServer", LastUsedServer, typeof(OysterVPNModel.Server));
            info.AddValue("static.CurrentLocation", CurrentLocation, typeof(OysterVPNModel.CurrentLocation));
            info.AddValue("static.Servers", Servers, typeof(Array));
            //info.AddValue("static.lastServerFetched", lastServerFetched, typeof(DateTime));
            //info.AddValue("static.loggedin", loggedin, typeof(bool));
            //info.AddValue("static.redialAutomatically", redialAutomatically, typeof(bool));
            //info.AddValue("static.autoconnect", autoconnect, typeof(bool));
            //info.AddValue("static.disconnectonExit", disconnectonExit, typeof(bool));
            //info.AddValue("static.openvpnConnected", openvpnConnected, typeof(bool));
            //info.AddValue("static.connectedServerIP", connectedServerIP, typeof(string));
        }

        public static bool getAllSitesUseVpn() => AllAppsUseVpn;

        public static List<string> getSitesList() => SplitTunnelSitesList;
        public static bool getSitesDontUseVpn() => SitesDontUseVpn;
        public static bool getSitesUseVpn() => SitesUseVpn;

        public static bool getopenvpnConnected() =>
            openvpnConnected;

        public static string getPassword() =>
            Security.Decrypt(Password);

        public static int getUserId() => UserId;
                

        public static Product getProduct() =>
            product;

        public static string getAvatar() =>
             Security.Decrypt(Avatar);

        public static bool getStartVpnMinimize () =>
           StartVpnMinimize;

        public static bool getConnectLastUsedServer() =>
            ConnectLastUsedServer;

        public static string[] getProtocols() =>
            Protocols;

        public static string getProtocol() =>
          Protocol;


        public static bool getredialAutomatically() =>
            redialAutomatically;

        public static bool getRemember() =>
            Remember;

        public static OysterVPNModel.Server getServer() =>
            Server;

        public static OysterVPNModel.CurrentLocation getCurrentLocation() =>
            CurrentLocation;

        public static OysterVPNModel.Server[] getServers() =>
            Servers;

        public static OysterVPNModel.Server getLastServerUsed() =>
           LastUsedServer;

        public static OysterVPNModel.Server[] getServers(string protocol)
        {
            try
            {
                return (from server in Servers
                        where server.Protocol == protocol
                        orderby server.IsFavourited descending, server.Name
                        select server).ToArray<OysterVPNModel.Server>();
            }
            catch
            {
                return new OysterVPNModel.Server[0];
            }
        }

        public static string getsettingFilePath() =>
            Storage.UserDataFolder + @"\" + SettingPath;


        public static bool isTapDriverInstalled()
        {
            openvpnCredentialscheck();
            bool flag = false;
            string id = "";
            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (interface2.Description.Contains("TAP-Windows"))
                {
                    id = interface2.Id;
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
            }
            return flag;
        }     

        public static void netsh(string command)
        {
            try
            {

                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process.Start(startInfo);

                startInfo.Arguments = "/C" + command + "";

                Process.Start(startInfo);

              //  Process.Start("cmd", "/C" + command + "");
                
                //client.netsh(command);
            }
            catch
            {
                Process.Start("cmd", "/C" + command + "");
            }
        }


        public static void openvpnCredentialscheck()
        {
            string[] contents = new string[] { getEmail(), getPassword() };
            try
            {
                File.Delete(openvpnaccPath);
                File.WriteAllLines(openvpnaccPath, contents);
            }
            catch
            {
            }
        }

        public static void PrepareTapDriver()
        {
            // MessageBox.Show("Tap Driver is not installed, click OK to install it.");
            Process process1 = new Process();
            ProcessStartInfo info1 = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.BaseDirectory + @"\Resources\driver\tap.exe"
            };
            process1.StartInfo = info1;
            Process process = process1;
            process.Start();
            process.WaitForExit();
        }

        private static void saveSettings()
        {
            BinarySerialize();
        }

        public static void setautoconnect(bool value)
        {
            autoconnect = value;
            saveSettings();
        }

        public static void setconnectedServerIP(string ipaddress)
        {
            connectedServerIP = ipaddress;
            saveSettings();
        }

        public static void setdisconnectonExit(bool value)
        {
            disconnectonExit = value;
            saveSettings();
        }

        public static void setEmail(string Email)
        {
            Settings.Email = Security.Encrypt(Email);
            saveSettings();
        }

        public static void setinternetKillSwitch(bool value)
        {
            internetKillSwitch = value;
            saveSettings();
        }

        public static void setlaunchStartup(bool value)
        {
            launchStartup = value;
            saveSettings();
        }

     

        public static void setStartVpnMinimize(bool value)
        {
            StartVpnMinimize = value;
            saveSettings();
        }

        public static void setConnectLastUsedServer(bool value)
        {
            ConnectLastUsedServer = value;
            saveSettings();
        }

        public static void setLastUsedServer(OysterVPNModel.Server Server)
        {
            Settings.LastUsedServer = Server;
            saveSettings();
        }

        public static void setLoggedin(bool loggedin)
        {
            Settings.loggedin = loggedin;
            saveSettings();
        }

        public static void setName(string Name)
        {
            Settings.Name = Security.Encrypt(Name);
            saveSettings();
        }

        public static void setPhone(string Phone)
        {
            Settings.Phone = Security.Encrypt(Phone);
            saveSettings();
        }

        public static void setAvatar(string Avatar)
        {
            Settings.Avatar = Security.Encrypt(Avatar);
            saveSettings();
        }

        public static void setToken(string Token)
        {
            Settings.Token = Security.Encrypt(Token);
            saveSettings();
        }


        public static void setopenvpnConnected(bool connect)
        {
            openvpnConnected = connect;
            saveSettings();
        }


        public static void setSitesUseVpn(bool sitesUsevpn)
        {
            Settings.SitesUseVpn = sitesUsevpn;
            saveSettings();
        }

        public static void setSitesDontUseVpn(bool SitesDontUsevpn)
        {
            Settings.SitesDontUseVpn = SitesDontUsevpn;
            saveSettings();
        }


        public static void setAllSitesUseVpn(bool AllAppsUseVpn)
        {
            Settings.AllAppsUseVpn= AllAppsUseVpn;
            saveSettings();
        }


        public static void setSitesList(List<string> sitesList)
        {
            Settings.SplitTunnelSitesList = sitesList;
            saveSettings();
        }


        public static void setUserId(int Id)
        {
            Settings.UserId = Id;
            saveSettings();
        }

        public static void setPassword(string Password)
        {
            Settings.Password = Security.Encrypt(Password);
            saveSettings();
        }

        public static void setProduct(Product product)
        {
            Settings.product = product;
            saveSettings();
        }

        public static void setProtocol(string _Protocol)
        {
            defaultProtocol = _Protocol;
            Protocol = _Protocol;
            saveSettings();
        }

        public static void setredialAutomatically(bool value)
        {
            redialAutomatically = value;
            saveSettings();
        }

        public static void setRemember(bool remember)
        {
            Remember = remember;
            saveSettings();
        }

        public static void setServer(OysterVPNModel.Server Server)
        {
            Settings.Server = Server;
            saveSettings();
        }

        public static void setCurrentLocation(OysterVPNModel.CurrentLocation CurrentLocation)
        {
            Settings.CurrentLocation = CurrentLocation;
            saveSettings();
        }

        public static void setServerConnect()
        {
            Server.IsConnected = true;
        }

        public static void setServerDisconnect()
        {
            Server.IsConnected = false;
        }

        public static void setServers(OysterVPNModel.Server[] Servers)
        {
            Settings.Servers = Servers;
            saveSettings();
        }


    }
}

