namespace OysterVPNLibrary.FVPNService
{
    using OysterVPNModel;
    using System;
    using System.CodeDom.Compiler;
    using System.ServiceModel;
    using System.Threading.Tasks;

    [GeneratedCode("System.ServiceModel", "4.0.0.0"), ServiceContract(ConfigurationName="FVPNService.FVPNService")]
    public interface FVPNService
    {
        [OperationContract(Action="http://tempuri.org/FVPNService/allowfastestvpn", ReplyAction="http://tempuri.org/FVPNService/allowfastestvpnResponse")]
        void allowfastestvpn();
        [OperationContract(Action="http://tempuri.org/FVPNService/allowfastestvpn", ReplyAction="http://tempuri.org/FVPNService/allowfastestvpnResponse")]
        Task allowfastestvpnAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/closeInternet", ReplyAction="http://tempuri.org/FVPNService/closeInternetResponse")]
        void closeInternet();
        [OperationContract(Action="http://tempuri.org/FVPNService/closeInternet", ReplyAction="http://tempuri.org/FVPNService/closeInternetResponse")]
        Task closeInternetAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/closetcpudp", ReplyAction="http://tempuri.org/FVPNService/closetcpudpResponse")]
        void closetcpudp();
        [OperationContract(Action="http://tempuri.org/FVPNService/closetcpudp", ReplyAction="http://tempuri.org/FVPNService/closetcpudpResponse")]
        Task closetcpudpAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/Connect", ReplyAction="http://tempuri.org/FVPNService/ConnectResponse")]
        bool Connect(Server server);
        [OperationContract(Action="http://tempuri.org/FVPNService/Connect", ReplyAction="http://tempuri.org/FVPNService/ConnectResponse")]
        Task<bool> ConnectAsync(Server server);
        [OperationContract(Action="http://tempuri.org/FVPNService/Disconnect", ReplyAction="http://tempuri.org/FVPNService/DisconnectResponse")]
        bool Disconnect();
        [OperationContract(Action="http://tempuri.org/FVPNService/Disconnect", ReplyAction="http://tempuri.org/FVPNService/DisconnectResponse")]
        Task<bool> DisconnectAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/isallowfastestvpn", ReplyAction="http://tempuri.org/FVPNService/isallowfastestvpnResponse")]
        bool isallowfastestvpn();
        [OperationContract(Action="http://tempuri.org/FVPNService/isallowfastestvpn", ReplyAction="http://tempuri.org/FVPNService/isallowfastestvpnResponse")]
        Task<bool> isallowfastestvpnAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/iscloseInternet", ReplyAction="http://tempuri.org/FVPNService/iscloseInternetResponse")]
        bool iscloseInternet();
        [OperationContract(Action="http://tempuri.org/FVPNService/iscloseInternet", ReplyAction="http://tempuri.org/FVPNService/iscloseInternetResponse")]
        Task<bool> iscloseInternetAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/killProcess", ReplyAction="http://tempuri.org/FVPNService/killProcessResponse")]
        void killProcess(string processname);
        [OperationContract(Action="http://tempuri.org/FVPNService/killProcess", ReplyAction="http://tempuri.org/FVPNService/killProcessResponse")]
        Task killProcessAsync(string processname);
        [OperationContract(Action="http://tempuri.org/FVPNService/netsh", ReplyAction="http://tempuri.org/FVPNService/netshResponse")]
        void netsh(string command);
        [OperationContract(Action="http://tempuri.org/FVPNService/netsh", ReplyAction="http://tempuri.org/FVPNService/netshResponse")]
        Task netshAsync(string command);
        [OperationContract(Action="http://tempuri.org/FVPNService/openInternet", ReplyAction="http://tempuri.org/FVPNService/openInternetResponse")]
        void openInternet();
        [OperationContract(Action="http://tempuri.org/FVPNService/openInternet", ReplyAction="http://tempuri.org/FVPNService/openInternetResponse")]
        Task openInternetAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/opentcpudp", ReplyAction="http://tempuri.org/FVPNService/opentcpudpResponse")]
        void opentcpudp();
        [OperationContract(Action="http://tempuri.org/FVPNService/opentcpudp", ReplyAction="http://tempuri.org/FVPNService/opentcpudpResponse")]
        Task opentcpudpAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/openVPN", ReplyAction="http://tempuri.org/FVPNService/openVPNResponse")]
        void openVPN();
        [OperationContract(Action="http://tempuri.org/FVPNService/openVPN", ReplyAction="http://tempuri.org/FVPNService/openVPNResponse")]
        Task openVPNAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/PrepareTapDriver", ReplyAction="http://tempuri.org/FVPNService/PrepareTapDriverResponse")]
        void PrepareTapDriver();
        [OperationContract(Action="http://tempuri.org/FVPNService/PrepareTapDriver", ReplyAction="http://tempuri.org/FVPNService/PrepareTapDriverResponse")]
        Task PrepareTapDriverAsync();
        [OperationContract(Action="http://tempuri.org/FVPNService/shell", ReplyAction="http://tempuri.org/FVPNService/shellResponse")]
        void shell(string command);
        [OperationContract(Action="http://tempuri.org/FVPNService/shell", ReplyAction="http://tempuri.org/FVPNService/shellResponse")]
        Task shellAsync(string command);
        [OperationContract(Action="http://tempuri.org/FVPNService/Status", ReplyAction="http://tempuri.org/FVPNService/StatusResponse")]
        string Status();
        [OperationContract(Action="http://tempuri.org/FVPNService/Status", ReplyAction="http://tempuri.org/FVPNService/StatusResponse")]
        Task<string> StatusAsync();
    }
}

