namespace OysterVPNModel
{
    using App;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Server
    {
        public int Id { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Dns { get; set; }
        public string DnsWithPort { get; set; }
        public string Iso { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Distance { get; set; }



        public string Ip { get; set; }
        public int? Port { get; set; }
        public string Protocol { get; set; }
        public string Ipsec = "";
        public string RemoteId;
        public bool IsTrial;
        public bool Active;
        public string Address;
        //private string Flag;
        public string Flag { get; set; }
        public bool IsFavourited;
        public bool IsConnected;

        public string Name { get; set; }

        public string SpeedIndex { get; set; }
        public string Latency { get; set; }
        public string DownloadSpeed { get; set; }


        public Regions Region { get; set; }




        public string flag
        {
            get =>
                this.Flag.Replace(Settings.ApiAssetUrl, @"/OysterVPN;component/assets/");
            set =>
                this.Flag = value;
        }

        public string fav =>
            "/OysterVPN;component/assets/" + (this.IsFavourited ? "star-fill.png" : "star-empty.png");

        public string connect
        {
            get
            {
                if (!this.IsConnected)
                {
                    return "Connect";
                }
                return "Connected";
            }
        }

        public enum Regions
        {
            //[Description("Asia")]
            Africa,
            Antarctica,
            Asia,
            Europe,
            North_America,
            Oceania,
            South_America

        }

        public List<Server> getServers()
        {
            OysterVPNLibrary.HttpClient client = new OysterVPNLibrary.HttpClient();
            Uri uri = new Uri(Settings.ApiUrl + "servers?orderby=id&orderdir=DESC");
            var JsonServers = client.GetData(uri, Settings.AuthToken);
            dynamic _servers = JObject.Parse(JsonServers);


            List<Server> servers = new List<Server>();

            foreach (var item in _servers.data)
            {

                servers.Add(new Server()
                {
                    Id = item.server_id,
                    Name = item.name,
                    Flag = item.flag,
                    Dns = item.dns,
                    Ip = item.ip_address,
                    Port = item.port,
                    DnsWithPort = item.dns + " " + item.port,
                    IsFavourited = item.is_favourited,
                    Continent = item.continent,
                    Latency = "--",
                    SpeedIndex = "--",
                    DownloadSpeed = "--",
                });

            }

            return servers;

        }


    }
}

