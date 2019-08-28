using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace WeatherAPIFetcher
{
    static class ZipcodeDataRequest
    {
        private const string BaseUrl = "https://api.zippopotam.us/";

        public static IRestClient Client { get; set; } = new RestClient(BaseUrl)
            .UseSerializer(() => new Program.JsonNetSerializer());

        public static IRestResponse<ZipcodeData> GetZipcodeData(string zipcode)
        {
            //create the request
            var request = new RestRequest("us/" + zipcode, DataFormat.Json);

            //deserialize the request data
            var response = Client.Get<ZipcodeData>(request);

            return response;
        }
    }
}
