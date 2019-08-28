using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace WeatherAPIFetcher
{
    static class MetaWeatherRequest
    {
        public static IRestClient client { get; set; } = new RestClient("https://www.metaweather.com/api/")
                .UseSerializer(() => new Program.JsonNetSerializer());

        public static IRestResponse<Location> GetLocation(int woeid)
        {
            //create the request
            var request = new RestRequest("location/" + woeid, DataFormat.Json);

            //Deserialize the request to an object along with the response
            var response = client.Get<Location>(request);

            return response;
        }
    }
}
