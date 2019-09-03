using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using RestSharp;
using RestSharp.Serialization;
using Newtonsoft.Json;

namespace WeatherAPIFetcher
{
    static class ZipcodeDataRequest
    {
        private const string BaseUrl = "https://api.zippopotam.us/";

        public static IRestClient Client { get; set; } = new RestClient(BaseUrl)
            .UseSerializer(() => new JsonNetSerializer());

        public static IRestResponse<ZipcodeData.ZipcodeData> GetZipcodeData(string zipcode)
        {
            //create the request
            var request = new RestRequest("us/" + zipcode, DataFormat.Json);

            //deserialize the request data
            var response = Client.Get<ZipcodeData.ZipcodeData>(request);

            return response;
        }
    }

    static class IPDataRequest
    {
        private const string BaseUrl = "https://ipvigilante.com/json/";

        public static IRestClient Client { get; set; } = new RestClient(BaseUrl)
            .UseSerializer(() => new JsonNetSerializer());

        public static IRestResponse<IPData.IPData> GetIPData(string ipAddress)
        {
            //create the request
            var request = new RestRequest(ipAddress + "/full", DataFormat.Json);

            //deserialize
            var response = Client.Get<IPData.IPData>(request);

            return response;
        }
    }

    static class DSWeatherDataRequest
    {
        private const string BaseUrl = "https://api.darksky.net/";

        public static IRestClient Client { get; set; } = new RestClient(BaseUrl)
            .UseSerializer(() => new JsonNetSerializer());

        public static IRestResponse<DarkSkyData> GetWeatherData(string lat, string lon)
        {
            //create the request
            var request = new RestRequest("forecast/{key}/{lat},{lon}/", DataFormat.Json);
            request.AddUrlSegment("key", ConfigurationManager.AppSettings.Get("DarkSkyAPIKey"));
            request.AddUrlSegment("lat", lat);
            request.AddUrlSegment("lon", lon);

            //deserialize
            var response = Client.Get<DarkSkyData>(request);

            return response;
        }

    }

    /// <summary>
    /// Custom serializer using NewtonSoft's JSON serializer
    /// </summary>
    public class JsonNetSerializer : IRestSerializer
    {
        public string Serialize(object obj) =>
        JsonConvert.SerializeObject(obj);

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content);

        public string[] SupportedContentTypes { get; } =
        {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
            };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }


}
