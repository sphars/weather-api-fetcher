using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization;
using Newtonsoft.Json;

namespace RestAPITest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://www.metaweather.com/api/")
                .UseSerializer(() => new JsonNetSerializer());

            var request = new RestRequest("location/2487610", DataFormat.Json);

            Location location = new Location();
            request.AddObject(location);

            var response = client.Get(request);
            var content = response.Content;

            var response2 = client.Get<Location>(request);

            Console.WriteLine(response.Content);

            Console.WriteLine("---");

            foreach(Source source in response2.Data.Sources)
                Console.WriteLine("{0}: {1}", source.Title, source.URL);

            Console.WriteLine("---");

            //foreach(ConsolidatedWeather cw in response2.Data.ConsolidatedWeather)
            //    Console.WriteLine("{0}: {1} C", cw.ApplicableDate, cw.Temp);

            Console.ReadKey();
        }

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
}
