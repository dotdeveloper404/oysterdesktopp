namespace OysterVPNLibrary
{
    using System;
    using System.Net.NetworkInformation;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class NetworkInterfaceCard
    {
        public IPAddressCollection dns;
        public string Id;
        public bool isDNSAuto = false;

        public string Name { get; set; }

        public int Index { get; set; }
    }
}

