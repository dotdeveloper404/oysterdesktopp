using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OysterVPNModel
{
    [Serializable]
    public class CurrentLocation
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Host { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
            
    }
}
