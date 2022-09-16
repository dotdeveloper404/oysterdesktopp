namespace FastestVPNLibrary.FVPNService
{
    using OysterVPNModel;
    using System;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;

    [DebuggerStepThrough, GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class FVPNServiceClient : ClientBase<OysterVPNLibrary.FVPNService.FVPNService>, OysterVPNLibrary.FVPNService.FVPNService
    {
        public FVPNServiceClient()
        {
        }

        public FVPNServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        public FVPNServiceClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        public FVPNServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public FVPNServiceClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public void allowfastestvpn()
        {
            base.Channel.allowfastestvpn();
        }

        public Task allowfastestvpnAsync() => 
            base.Channel.allowfastestvpnAsync();

        public void closeInternet()
        {
            base.Channel.closeInternet();
        }

        public Task closeInternetAsync() => 
            base.Channel.closeInternetAsync();

        public void closetcpudp()
        {
            base.Channel.closetcpudp();
        }

        public Task closetcpudpAsync() => 
            base.Channel.closetcpudpAsync();

        public bool Connect(Server server) => 
            base.Channel.Connect(server);

        public Task<bool> ConnectAsync(Server server) => 
            base.Channel.ConnectAsync(server);

        public bool Disconnect() => 
            base.Channel.Disconnect();

        public Task<bool> DisconnectAsync() => 
            base.Channel.DisconnectAsync();

        public bool isallowfastestvpn() => 
            base.Channel.isallowfastestvpn();

        public Task<bool> isallowfastestvpnAsync() => 
            base.Channel.isallowfastestvpnAsync();

        public bool iscloseInternet() => 
            base.Channel.iscloseInternet();

        public Task<bool> iscloseInternetAsync() => 
            base.Channel.iscloseInternetAsync();

        public void killProcess(string processname)
        {
            base.Channel.killProcess(processname);
        }

        public Task killProcessAsync(string processname) => 
            base.Channel.killProcessAsync(processname);

        public void netsh(string command)
        {
            base.Channel.netsh(command);
        }

        public Task netshAsync(string command) => 
            base.Channel.netshAsync(command);

        public void openInternet()
        {
            base.Channel.openInternet();
        }

        public Task openInternetAsync() => 
            base.Channel.openInternetAsync();

        public void opentcpudp()
        {
            base.Channel.opentcpudp();
        }

        public Task opentcpudpAsync() => 
            base.Channel.opentcpudpAsync();

        public void openVPN()
        {
            base.Channel.openVPN();
        }

        public Task openVPNAsync() => 
            base.Channel.openVPNAsync();

        public void PrepareTapDriver()
        {
            base.Channel.PrepareTapDriver();
        }

        public Task PrepareTapDriverAsync() => 
            base.Channel.PrepareTapDriverAsync();

        public void shell(string command)
        {
            base.Channel.shell(command);
        }

        public Task shellAsync(string command) => 
            base.Channel.shellAsync(command);

        public string Status() => 
            base.Channel.Status();

        public Task<string> StatusAsync() => 
            base.Channel.StatusAsync();
    }
}

