using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization;
using Newtonsoft.Json;

namespace WeatherAPIFetcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your 5-digit zipcode to get the forecast. No entry will attempt to fetch your location.\nEnter 'N' to exit");

            while(true)
            {
                Console.Write("Enter your zipcode: ");
                string line = Console.ReadLine();

                if (int.TryParse(line, out int zipcode))
                {
                    try
                    {
                        var response = MetaWeatherRequest.GetLocation(zipcode);
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            Console.WriteLine("Something went wrong: {0}", response.StatusCode);
                        }
                        else
                        {
                            PrintLocationData(response.Data);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was an error: " + ex.Message);
                    }
                    
                }
                else if (line.ToLower() != "n")
                {
                    Console.WriteLine("That's not a valid zipcode.");
                }
                else
                {
                    break;
                }

            }

        }

        public static void PrintLocationData(Location location)
        {
            Console.WriteLine($"Data for {location.Title}");
            Console.WriteLine("---");

            Console.WriteLine($"{"Date",-11}{"Min Temp",-11}{"Max Temp",-11}{"State",-12}{"Confidence"}");
            foreach (ConsolidatedWeather cw in location.Consolidated_Weather)
            {
                Console.WriteLine("{0,-11}{1,-6} {5}F  {2,-6} {5}F  {3,-12}{4}%",
                    cw.Applicable_Date.ToShortDateString(),
                    ConvertTemp.CtoF(cw.Min_Temp).ToString().PadRight(6, '0'),
                    ConvertTemp.CtoF(cw.Max_Temp).ToString().PadRight(6, '0'),
                    cw.Weather_State_Name,
                    cw.Predictability,
                    (char)0176);
            }

            Console.WriteLine("---");
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
