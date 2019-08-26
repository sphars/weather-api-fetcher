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
            //create the rest client, along with a custom serializer
            var client = new RestClient("https://www.metaweather.com/api/")
                .UseSerializer(() => new JsonNetSerializer());

            //create the request
            var request = new RestRequest("location/2487610", DataFormat.Json);

            //Location location = new Location();
            //request.AddObject(location); //for POSTing
            
            //Deserialize the request to an object along with the response
            var response = client.Get<Location>(request);

            //foreach (Source source in response.Data.Sources)
            //    Console.WriteLine("{0}: {1}", source.Title, source.URL);

            Console.WriteLine("---");

            Console.WriteLine("Date\t\tMin\t\tMax\t\tState");
            foreach (ConsolidatedWeather cw in response.Data.Consolidated_Weather)
            {
                Console.WriteLine("{0}\t{1} F\t{2} F\t{3}", cw.Applicable_Date.ToShortDateString(), ConvertTemp.CtoF(cw.Min_Temp).ToString().PadRight(6, '0'), ConvertTemp.CtoF(cw.Max_Temp).ToString().PadRight(6, '0'), cw.Weather_State_Name);
            }

            Console.WriteLine("---");

            var request2 = new RestRequest("location/2487610/2019/8/25", DataFormat.Json);
            var response2 = client.Execute<List<ConsolidatedWeather>>(request2);


            Console.ReadKey();
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

        /// <summary>
        /// Class to convert temperatures
        /// </summary>
        static class ConvertTemp
        {
            /// <summary>
            /// Convert Celsius to Farenheit
            /// </summary>
            /// <param name="temp">Temperature in celsius</param>
            /// <returns>Temperature in farenheit</returns>
            public static double CtoF(double temp) => (9.0 / 5.0 * temp) + 32;

            /// <summary>
            /// Convert Farenheit to Celsius
            /// </summary>
            /// <param name="temp">Temperature in Farenheit</param>
            /// <returns>Temperature in celsius</returns>
            public static double FtoC(double temp) => 5.0 / 9.0 * (temp - 32);

            /// <summary>
            /// Converts celsius to kelvin
            /// </summary>
            /// <param name="temp">Temperature in celsius</param>
            /// <returns>Temperature in kelvin</returns>
            public static double CtoK(double temp) => FtoK(CtoF(temp));

            /// <summary>
            /// Converts farenheit to kelvin
            /// </summary>
            /// <param name="temp">Temperature in farenheit</param>
            /// <returns>Temperature in kelvin</returns>
            public static double FtoK(double temp) => ((temp - 32) * (5.0 / 9.0)) + 273.15;
        }

    }
}
