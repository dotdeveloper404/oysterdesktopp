namespace OysterVPNLibrary.Library
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public static class DisconnectWrapper
    {
        public static void CloseConnection(string connectionstring)
        {
            try
            {
                char[] separator = new char[] { '-' };
                string[] strArray = connectionstring.Split(separator);
                if (strArray.Length != 4)
                {
                    throw new Exception("Invalid connectionstring - use the one provided by Connections.");
                }
                char[] chArray2 = new char[] { ':' };
                string[] strArray2 = strArray[0].Split(chArray2);
                char[] chArray3 = new char[] { ':' };
                string[] strArray3 = strArray[1].Split(chArray3);
                char[] chArray4 = new char[] { '.' };
                string[] strArray4 = strArray2[0].Split(chArray4);
                char[] chArray5 = new char[] { '.' };
                string[] strArray5 = strArray3[0].Split(chArray5);
                ConnectionInfo info = new ConnectionInfo {
                    dwState = 12
                };
                byte[] buffer = new byte[] { byte.Parse(strArray4[0]), byte.Parse(strArray4[1]), byte.Parse(strArray4[2]), byte.Parse(strArray4[3]) };
                byte[] buffer2 = new byte[] { byte.Parse(strArray5[0]), byte.Parse(strArray5[1]), byte.Parse(strArray5[2]), byte.Parse(strArray5[3]) };
                info.dwLocalAddr = BitConverter.ToInt32(buffer, 0);
                info.dwRemoteAddr = BitConverter.ToInt32(buffer2, 0);
                info.dwLocalPort = htons(int.Parse(strArray2[1]));
                info.dwRemotePort = htons(int.Parse(strArray3[1]));
                int num = SetTcpEntry(GetPtrToNewObject(info));
                switch (num)
                {
                    case -1:
                        throw new Exception("Unsuccessful");

                    case 0x41:
                        throw new Exception("User has no sufficient privilege to execute this API successfully");

                    case 0x57:
                        throw new Exception("Specified port is not in state to be closed down");
                }
                if (num != 0)
                {
                    throw new Exception("Unknown error (" + num.ToString() + ")");
                }
            }
            catch (Exception exception)
            {
                string[] textArray1 = new string[] { "CloseConnection failed (", connectionstring, ")! [", exception.GetType().ToString(), ",", exception.Message, "]" };
                throw new Exception(string.Concat(textArray1));
            }
        }

        public static void CloseLocalIP(string IP)
        {
            ConnectionInfo[] infoArray = getTcpTable();
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i].dwLocalAddr == IPStringToInt(IP))
                {
                    infoArray[i].dwState = 12;
                    int num2 = SetTcpEntry(GetPtrToNewObject(infoArray[i]));
                }
            }
        }

        public static void CloseLocalPort(int port)
        {
            ConnectionInfo[] infoArray = getTcpTable();
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (port == ntohs(infoArray[i].dwLocalPort))
                {
                    infoArray[i].dwState = 12;
                    int num2 = SetTcpEntry(GetPtrToNewObject(infoArray[i]));
                }
            }
        }

        public static void CloseRemoteIP(string IP)
        {
            ConnectionInfo[] infoArray = getTcpTable();
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i].dwRemoteAddr == IPStringToInt(IP))
                {
                    infoArray[i].dwState = 12;
                    int num2 = SetTcpEntry(GetPtrToNewObject(infoArray[i]));
                }
            }
        }

        public static void CloseRemotePort(int port)
        {
            ConnectionInfo[] infoArray = getTcpTable();
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (port == ntohs(infoArray[i].dwRemotePort))
                {
                    infoArray[i].dwState = 12;
                    int num2 = SetTcpEntry(GetPtrToNewObject(infoArray[i]));
                }
            }
        }

        public static string[] Connections() => 
            Connections(ConnectionState.All);

        public static string[] Connections(ConnectionState state)
        {
            ConnectionInfo[] infoArray = getTcpTable();
            ArrayList list = new ArrayList();
            foreach (ConnectionInfo info in infoArray)
            {
                if ((state == ConnectionState.All) || ((int)state == info.dwState))
                {
                    string str = IPIntToString(info.dwLocalAddr) + ":" + ntohs(info.dwLocalPort).ToString();
                    string str2 = IPIntToString(info.dwRemoteAddr) + ":" + ntohs(info.dwRemotePort).ToString();
                    list.Add(str + "-" + str2 + "-" + info.dwState.ToString() + "-" + info.dwState.ToString());
                }
            }
            return (string[]) list.ToArray(typeof(string));
        }

        private static IntPtr GetPtrToNewObject(object obj)
        {
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }

        private static ConnectionInfo[] getTcpTable()
        {
            ConnectionInfo[] infoArray2;
            IntPtr zero = IntPtr.Zero;
            bool flag = false;
            try
            {
                int pdwSize = 0;
                GetTcpTable(IntPtr.Zero, ref pdwSize, false);
                zero = Marshal.AllocCoTaskMem(pdwSize);
                flag = true;
                GetTcpTable(zero, ref pdwSize, false);
                int num2 = Marshal.ReadInt32(zero);
                IntPtr ptr = zero;
                ptr = (IntPtr) (((int) zero) + 4);
                ConnectionInfo[] infoArray = new ConnectionInfo[num2];
                int num3 = Marshal.SizeOf<ConnectionInfo>(new ConnectionInfo());
                for (int i = 0; i < num2; i++)
                {
                    infoArray[i] = (ConnectionInfo) Marshal.PtrToStructure(ptr, typeof(ConnectionInfo));
                    ptr = (IntPtr) (((int) ptr) + num3);
                }
                infoArray2 = infoArray;
            }
            catch (Exception exception)
            {
                string[] textArray1 = new string[] { "getTcpTable failed! [", exception.GetType().ToString(), ",", exception.Message, "]" };
                throw new Exception(string.Concat(textArray1));
            }
            finally
            {
                if (flag)
                {
                    Marshal.FreeCoTaskMem(zero);
                }
            }
            return infoArray2;
        }

        [DllImport("iphlpapi.dll")]
        private static extern int GetTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder);
        [DllImport("wsock32.dll")]
        private static extern int htons(int netshort);
        private static string IPIntToString(int IP)
        {
            byte[] bytes = BitConverter.GetBytes(IP);
            string[] textArray1 = new string[] { bytes[0].ToString(), ".", bytes[1].ToString(), ".", bytes[2].ToString(), ".", bytes[3].ToString() };
            return string.Concat(textArray1);
        }

        private static int IPStringToInt(string IP)
        {
            if (IP.IndexOf(".") < 0)
            {
                throw new Exception("Invalid IP address");
            }
            char[] separator = new char[] { '.' };
            string[] strArray = IP.Split(separator);
            if (strArray.Length != 4)
            {
                throw new Exception("Invalid IP address");
            }
            byte[] buffer = new byte[] { byte.Parse(strArray[0]), byte.Parse(strArray[1]), byte.Parse(strArray[2]), byte.Parse(strArray[3]) };
            return BitConverter.ToInt32(buffer, 0);
        }

        [DllImport("wsock32.dll")]
        private static extern int ntohs(int netshort);
        [DllImport("iphlpapi.dll")]
        private static extern int SetTcpEntry(IntPtr pTcprow);

        [StructLayout(LayoutKind.Sequential)]
        private struct ConnectionInfo
        {
            public int dwState;
            public int dwLocalAddr;
            public int dwLocalPort;
            public int dwRemoteAddr;
            public int dwRemotePort;
        }

        public enum ConnectionState
        {
            All,
            Closed,
            Listen,
            Syn_Sent,
            Syn_Rcvd,
            Established,
            Fin_Wait1,
            Fin_Wait2,
            Close_Wait,
            Closing,
            Last_Ack,
            Time_Wait,
            Delete_TCB
        }
    }
}

