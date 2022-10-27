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


        private const int MINIMUM_SPLASH_TIME = 1500; // Miliseconds  

        private Mutex _mutex;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public App()
        {
            string Value = string.Empty;
            Log.Active = true;
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
                        splashScreen.Hide();
                        LoginSplash splash = new LoginSplash();
                        splash.ShowDialog();
                        
                    });
                });

                // Add Event handler to exit event.
                Exit += CloseMutexHandler;

            }


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
