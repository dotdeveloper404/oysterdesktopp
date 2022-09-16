using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace App
{
   static  class InternetKillSwitch
    {

        static private string vpnNIC;
        static private Int32 nicChoice;
        static private bool isKill;

       public static void Start()
        {


            // enable internet connection (it could have been disabled before calling this function)
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "ipconfig";
            info.Arguments = "/renew"; // or /release if you want to disconnect
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(info);
            p.WaitForExit();


            // get list of interfaces
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
             

            //nicChoice = Convert.ToInt32(Console.ReadLine());


            // store the interface id to a member value so we can compate when we detect a network change 
            vpnNIC = adapters[0].Id;


            // start listening for a network change
            isKill = false;
            NetworkChange.NetworkAddressChanged += new
            NetworkAddressChangedEventHandler(AddressChangedCallback);
   


        }

        static void AddressChangedCallback(object sender, EventArgs e)
        {
            if (!isKill)
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                Console.Beep(570, 100);
                Console.Beep(570, 100);
                Console.Beep(570, 100);
                Console.Beep(570, 100);
                Console.Write("\r\n[NETWORK CHANGE DETECTED]", Color.Red);
                Console.Write(" Checking if our vpn has been disabled.\n");
                if (adapters[0].OperationalStatus == OperationalStatus.Down)
                {
                    isKill = true;
                    KillNow();
                    Console.Write("[INTERNET DISABLED]", Color.Red);
                    Console.Write(" your vpn was dissconnected. (restart the kill switch and turn on your vpn before selecting the NIC)");
                }

            }

        }

        public static void KillNow()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "ipconfig";
            info.Arguments = "/release";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(info);
            p.WaitForExit();
        }
    }
}
