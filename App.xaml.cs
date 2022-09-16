using App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using System.IO;
using NativeMessaging;
using System.Windows.Controls.Primitives;
using OysterVPNLibrary;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Web;
using Microsoft.Data.Sqlite;
using NetFwTypeLib; // Located in FirewallAPI.dll
using System.Text.RegularExpressions;
using System.Drawing;
using System.Reflection;

namespace OysterVPN
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        static Host Host;


        //readonly static string[] AllowedOrigins = new string[] { "chrome-extension://clopahnedcaapecinoalaagdljhfiflh" };
        //readonly static string Description = "OysterVPN Chrome Server Extension";
        private const int MINIMUM_SPLASH_TIME = 1500; // Miliseconds  

        private Mutex _mutex;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        #region
          /*
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
             _notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location); //OysterWPF.Properties.Resources.MyIcon;
            _notifyIcon.Visible = true;

            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }
          */
        #endregion

        public App()
        {
         
            //remove internet block
           // INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
           //Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
           // firewallPolicy.Rules.Remove("OysterVPN Block Internet");

            string Value = string.Empty;
         //   var t = GetCookie_Chrome("com.oystertech.vpn", "1",ref  Value);
       //    // System.Windows.Forms.MessageBox.Show(t.ToString());

            Log.Active = true;

            Host = new MyHost();
            Host.SupportedBrowsers.Add(ChromiumBrowser.GoogleChrome);
            Host.SupportedBrowsers.Add(ChromiumBrowser.MicrosoftEdge);
            Host.SupportedBrowsers.Add(ChromiumBrowser.Mozilla);

            if (!Host.IsRegistered())
            {
                Host.Register();
            }
            //var threads = new List<Thread>();
            //var thread = threads.FirstOrDefault(x => x.Name == "ExtensionThread");

            //if (thread != null) { thread.Abort(); }

            Thread backgroundThread = new Thread(new ThreadStart(Host.Listen));
            backgroundThread.Priority = ThreadPriority.Highest;
            backgroundThread.Name = "ExtensionThread";
            backgroundThread.Start();

            //TimeSpan interval = new TimeSpan(0, 0, 2);
            //Thread.Sleep(interval);
             

           // Log.LogMessage("Extension Class Variables : " + "IsConnectVpnExtension" + ExtensionUserInfo.IsConnectVpnExtension + "IsDisConnectVpnExtension" + ExtensionUserInfo.IsDisConnectVpnExtension)


            #region check chrome extension login button pressed
            //System.Windows.Forms.MessageBox.Show(ExtensionUserInfo.IsDisConnectVpnExtension.ToString());
            //if (ExtensionUserInfo.IsDisConnectVpnExtension == true)
            //{
            //    HomeScreen home = new HomeScreen();
            //    System.Windows.Forms.MessageBox.Show("Test");
            //    System.Windows.Forms.MessageBox.Show(home.btnDisConnect.Content.ToString());
            //    home.btnDisConnect.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            //}
            #endregion


            //  ExtensionUserInfo.DisConnectedChanged += CheckConnectionChrome;

        }

        private void ApplicationStart(object sender, StartupEventArgs e)
        {

            Settings.AuthToken = Settings.getToken();
        
            // Try to grab mutex
            bool createdNew;
            _mutex = new Mutex(true, "OysterVPN", out createdNew);
            if (!createdNew)
            {
                // Bring other instance to front and exit.
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
              
                //show your MainWindow

                Application.Current.Shutdown();

            }
            else
            {

                  //check auth token

                    if (Settings.fetchSettings()) {
                    Settings.AuthToken = Settings.getToken();

                    if (Settings.AuthToken != null && Settings.AuthToken != "")
                    {

                        Home h = new Home();
                        h.ShowDialog();

                       // HomeWindow w = new HomeWindow();
                    ///    w.ShowDialog();

                        return;
                    }
                }
                //initialize the splash screen and set it as the application main window
                SplashForm splashScreen = new SplashForm();
                splashScreen.Show();

                //in order to ensure the UI stays responsive, we need to
                //do the work on a different thread
                Task.Factory.StartNew(() =>
                {
                    //simulate some work being done
                    System.Threading.Thread.Sleep(3000);

                    //since we're not on the UI thread
                    //once we're done we need to use the Dispatcher
                    //to create and show the main window
                    this.Dispatcher.Invoke(() =>
                    {
                        //initialize the main window, set it as the application main window
                        //and close the splash screen
                        //var mainWindow = new Login();
                        //this.MainWindow = mainWindow;
                        splashScreen.Hide();
                        LoginSplash splash = new LoginSplash();
                        splash.ShowDialog();
                        //Login login = new Login();
                        //login.ShowDialog();
                        
                    });
                });

                // Add Event handler to exit event.
                Exit += CloseMutexHandler;

            }

            //if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            //{
            //    return;
            //}


            // Determine if login was successful
            //if (Settings.getLoggedin() == true)
            //{
            //    HomeScreen home = new HomeScreen();

            //    home.ShowDialog();

            //}
            //else
            //{

            //    Window login = new Login();
            //    login.ShowDialog();

            //}


            // Window loginSplash = new LoginSplash();
            //  loginSplash.ShowDialog();

         //   Login login = new Login();
           // login.ShowDialog();
            //HomeWindow w = new HomeWindow();
            // w.ShowDialog();

          

        }




        protected virtual void CloseMutexHandler(object sender, EventArgs e)
        {
            _mutex?.Close();
        }


     
        private void Application_Exit(object sender , EventArgs e)
        {
            //JObject data = new JObject();
            //data.Add("IsOpenExe", 0);
            //Host.SendMessage(data);
        }


        public void CheckConnectionChrome(object sender, EventArgs e)
        {
            Log.Active = true;
            try
            {

                if (ExtensionUserInfo.IsDisConnectVpnExtension == true)
                {

                  HomeScreen home = new HomeScreen();

                    System.Windows.Forms.MessageBox.Show("asdsadsa");
                    //System.Windows.Forms.MessageBox.Show(home.connect_label.ToString());
                    //System.Windows.Forms.MessageBox.Show(home.btnConnect.ToString());
                    //System.Windows.Forms.MessageBox.Show(home.btnConnect.Visibility.ToString());

                    //home.btnDisConnect.Visibility = Visibility.Hidden;
                    //home.btnConnect.Visibility = Visibility.Visible;
                    //System.Windows.Forms.MessageBox.Show(home.btnDisConnect.Visibility.ToString());
                    //home.btnDisConnect.Visibility = Visibility.Hidden;
                    //home.btnConnect.Visibility = Visibility.Visible;
                    //home.connect_label.Content = "Disconnected";
                    ///  HomeScreen d = new HomeScreen();

                    //System.Windows.Forms.MessageBox.Show(home.btnConnect.ToString());
                     home.btnDisConnect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                }
            }
            catch (Exception ex)
            {
                Log.LogMessage("asdsa" + ex.Message.ToString());
                //   throw ex;
            }


            }

        private static string GetChromeCookiePath()
        {
            string s = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            s += @"\Google\Chrome\User Data\Default\cookies";

            if (!File.Exists(s))
                return string.Empty;

            return s;
        }

        public void IDispoable()
        {

        }

        private static bool GetCookie_Chrome(string strHost, string strField, ref string Value)
        {
            Value = string.Empty;
            bool fRtn = false;
            string strPath, strDb;

            // Check to see if Chrome Installed
            strPath = GetChromeCookiePath();
            if (string.Empty == strPath) // Nope, perhaps another browser
                return false;

            try
            {
                strDb = "Data Source=" + strPath ;

                using (SqliteConnection conn = new SqliteConnection(strDb))
                {
                    using (SqliteCommand cmd = conn.CreateCommand())
                    {   
                        cmd.CommandText = "SELECT value FROM cookies WHERE host_key LIKE '%" +
                            strHost + "%' AND name LIKE '%" + strField + "%';";

                        conn.Open();
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Value = reader.GetString(0);
                                if (!Value.Equals(string.Empty))
                                {
                                    fRtn = true;
                                    break;
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Value = string.Empty;
                fRtn = false;
            }
            return fRtn;
        }

    }
    }
