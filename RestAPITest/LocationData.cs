using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace WeatherAPIFetcher
{
    namespace ZipcodeData
    {
        public class ZipcodeData
        {
            [JsonProperty(PropertyName = "post code")]
            public string Post_Code { get; set; }
            public string Country { get; set; }
            [JsonProperty(PropertyName = "country abbreviation")]
            public string Country_Abbreviation { get; set; }
            public List<Place> Places { get; set; }
        }

        public class Place
        {
            [JsonProperty(PropertyName = "place name")]
            public string Place_Name { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string State { get; set; }
            [JsonProperty(PropertyName = "state abbreviation")]
            public string State_Abbreviation { get; set; }
        }
    }

    namespace IPData
    {
        public class IPData
        {
            public string Status { get; set; }
            public Data Data { get; set; }
        }

        public class Data
        {
            public string Ipv4 { get; set; }
            public string Continent_Name { get; set; }
            public string Country_Name { get; set; }
            public string Subdivision_1_Name { get; set; }
            public string Subdivision_1_ISO_Code { get; set; }
            public object Subdivision_2_Name { get; set; }
            public string City_Name { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Postal_Code { get; set; }
        }
    }
    
}
