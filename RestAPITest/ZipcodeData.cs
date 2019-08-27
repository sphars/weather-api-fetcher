using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace WeatherAPIFetcher
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
