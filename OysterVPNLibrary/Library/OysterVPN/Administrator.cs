namespace OysterVPNLibrary.Library.OysterVPN
{
    using App;
    using Microsoft.VisualBasic;
    using Microsoft.Win32;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    //using System.ServiceProcess;
    using System.Threading;
    using System.Windows;
    //using System.Windows.Forms;

    public class Administrator
    {
        public static void allowoystervpn()
        {
            try
            {
                Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\TCPIP6\Parameters", true).SetValue("DisabledComponents", 0xff, RegistryValueKind.DWord);
            }
            catch
            {
            }
            if (!isallowoystervpn())
            {
                string str = "netsh advfirewall firewall add rule  protocol=tcp name=\"OysterVPN\" dir=in  action=allow";
                Interaction.Shell("cmd.exe /c" + str, (AppWinStyle)AppWinStyle.Hide, false, -1);
                str = "netsh advfirewall firewall add rule  protocol=udp name=\"OysterVPN\" dir=in  action=allow program=\"" + AppDomain.CurrentDomain.BaseDirectory + "\"";
                Console.WriteLine(str);
                Interaction.Shell("cmd.exe /c" + str, (AppWinStyle)AppWinStyle.Hide, false, -1);
            }
        }

        public static void closeInternet()
        {
            Interaction.Shell("cmd.exe /cnetsh Advfirewall set allprofiles state on", (AppWinStyle)AppWinStyle.Hide, false, -1);
            Interaction.Shell("cmd.exe /cnetsh advfirewall firewall add rule name=\"oystervpninternetkillout\" dir=out action=block\r\nnetsh advfirewall firewall add rule name=\"oystervpninternetkillin\" dir=in action=block\r\n", (AppWinStyle)AppWinStyle.Hide, false, -1);
            closetcpudp();
        }

        public static void closetcpudp()
        {
            string str = "";
            if (Settings.defaultProtocol == "IKEV2")
            {
                str = "netsh advfirewall firewall add rule  protocol=tcp name=\"tcpoutblock\" dir=out  action=block localport=1-1193,1195-1722,1724-4442,4444-65535";
            }
            else
            {
                str = "netsh advfirewall firewall add rule  protocol=tcp name=\"tcpoutblock\" dir=out  action=block localport=1-1193,1195-1722,1724-4442,4444-50000";
            }
            Interaction.Shell("cmd.exe /c" + str, (AppWinStyle)AppWinStyle.Hide, false, -1);
        }

        public static string GetExecutingDirectoryName()
        {
            Uri uri = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(uri.AbsolutePath).Directory.FullName;
        }

        public static bool isallowoystervpn()
        {
            if (readoutput("netsh advfirewall firewall show rule OysterVPN\r\n").Contains("No rules match"))
            {
                return false;
            }
            return true;
        }

        public static bool iscloseInternet()
        {
            if (readoutput("netsh advfirewall firewall show rule oystervpninternetkillout\r\n").Contains("No rules match"))
            {
                return false;
            }
            return true;
        }

        public static void killProcess(string processname)
        {
            Process[] processesByName = Process.GetProcessesByName("openvpn");
            foreach (Process process in processesByName)
            {
                process.Kill();
            }
        }

        public static void netsh(string command)
        {
            new Thread(() => readoutput("netsh " + command + "\r\n")).Start();
        }

        public static void openInternet()
        {
            Interaction.Shell("cmd.exe /c" + "netsh advfirewall firewall del rule name=\"oystervpninternetkillin\"", (AppWinStyle)AppWinStyle.Hide, false, -1);
            Interaction.Shell("cmd.exe /c" + "netsh advfirewall firewall del rule name=\"oystervpninternetkillout\" ", (AppWinStyle)AppWinStyle.Hide, false, -1);
            opentcpudp();
        }

        public static void opentcpudp()
        {
            Interaction.Shell("cmd.exe /c" + "netsh advfirewall firewall del rule name=\"tcpoutblock\"", (AppWinStyle)AppWinStyle.Hide, false, -1);
        }

        public static void openVPN()
        {
            Interaction.Shell("cmd.exe /c" + "netsh advfirewall firewall del rule name=\"oystervpninternetkillin\"", (AppWinStyle)AppWinStyle.Hide, false, -1);
            Interaction.Shell("cmd.exe /c" + "netsh advfirewall firewall del rule name=\"oystervpninternetkillout\" ", (AppWinStyle)AppWinStyle.Hide, false, -1);
        }

        public static void PrepareTapDriver()
        {
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\driver\tap.exe");
            }
            catch
            {
            }
        }

        public static string readoutput(string command)
        {
            Process process = new Process();
            ProcessStartInfo info1 = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + command + " /c",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            process.StartInfo = info1;
            process.Start();
            string str = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return str;
        }

        public static void reinstallOysterVPNService()
        {
            runAsAdministrator(AppDomain.CurrentDomain.BaseDirectory + @"\restartService.exe");
        }

        public static void runAsAdministrator(string filename)
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = {
                        FileName = "\"" + filename + "\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            catch (Exception exception)
            {
                if (exception.Message == "The operation was canceled by the user")
                {
                    MessageBox.Show("Background service is not running, Exiting...");
                    Environment.Exit(0);
                }
            }
        }

        public static void shell(string command)
        {
            Interaction.Shell(command, (AppWinStyle)AppWinStyle.Hide, false, -1);
        }

        //public static void verifyOysterVPNService()
        //{
      // ServiceController controller = new ServiceController("OysterVPNService");
        //    try
        //    {
        //        if (controller.get_Status() == 1)
        //        {
        //            MessageBox.Show("Windows background service isn’t running. Please click the button below to restart it.", "Please restart background service");
        //            runAsAdministrator(AppDomain.CurrentDomain.BaseDirectory + @"\restartService.exe");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Windows background service isn’t running. Please click the button below to restart it.", "Please restart background service");
        //        runAsAdministrator(AppDomain.CurrentDomain.BaseDirectory + @"\restartService.exe");
        //    }
        //}
    }
}

