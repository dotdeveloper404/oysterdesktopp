using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMessaging
{
    static public class ExtensionUserInfo
    {
        public static string Email { get; set; }
        public static string Password { get; set; }

        public static bool IsLogin { get; set; }

        public static event EventHandler DisConnectedChanged;

        public static bool IsConnectVpnExtension { get; set; }

        private static bool _IsDisConnectVpnExtension;
        //  public static bool IsDisConnectVpnExtension { get; set; }
        public static bool IsDisConnectVpnExtension
        {
            get { return _IsDisConnectVpnExtension; }

            set
            {
                if (_IsDisConnectVpnExtension == value) return;


                //  OnDisConnectChange(_IsDisConnectVpnExtension);
           

                _IsDisConnectVpnExtension = value;
                OnDisConnectChange(IsDisConnectVpnExtension);
            }

        }

     

        private static void OnDisConnectChange(bool val)
        {
            if (DisConnectedChanged != null)
                DisConnectedChanged(val, EventArgs.Empty);

        }

    }
}
