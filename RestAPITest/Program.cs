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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your 5-digit zipcode to get the forecast (US zipcodes only).\nBlank entry will attempt to fetch your location via your public IP.\nEnter 'N' to exit.\n");

            while(true)
            {
                Console.WriteLine("-----");
                Console.Write("Enter your zipcode: ");
                string line = Console.ReadLine();
                string publicIP = new System.Net.WebClient().DownloadString("https://api.ipify.org"); //get public IP

                if (string.IsNullOrEmpty(line))
                {   
                    var ipDataRequest = IPDataRequest.GetIPData(publicIP);
                    if (ipDataRequest.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine("Something went wrong: {0}", ipDataRequest.StatusCode);
                    }
                    else
                    {
                        var ipData = ipDataRequest.Data;
                        var weatherRequest = DSWeatherDataRequest.GetWeatherData(ipData.Data.Latitude, ipData.Data.Longitude);
                        var weatherData = weatherRequest.Data;

                        PrintLocationData(ipData, publicIP);
                        PrintWeatherData(weatherData);
                    }
                }
                else if (int.TryParse(line, result: out int zipcode) && line.Length <= 5)
                {
                    var zipcodeDataRequest = ZipcodeDataRequest.GetZipcodeData(zipcode.ToString().PadLeft(5, '0'));
                    if (zipcodeDataRequest.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine("Something went wrong: {0}", zipcodeDataRequest.StatusCode);
                    }
                    else
                    {
                        var zipcodeData = zipcodeDataRequest.Data;
                        var weatherRequest = DSWeatherDataRequest.GetWeatherData(zipcodeData.Places[0].Latitude, zipcodeData.Places[0].Longitude);
                        var weatherData = weatherRequest.Data;

                        PrintLocationData(zipcodeData, publicIP);
                        PrintWeatherData(weatherData);
                    }
                }
                else if (line.ToLower() != "n" || line.Length > 5)
                {
                    Console.WriteLine("That's not a valid zipcode.");
                }
                else
                {
                    break;
                }

            }

        }

        public static void PrintLocationData(object locationData, string publicIP)
        {
            if(locationData.GetType() == typeof(ZipcodeData.ZipcodeData))
            {
                var data = (ZipcodeData.ZipcodeData)locationData;
                Console.WriteLine("  {0} | {1} | {2}, {3} | {4}, {5}",
                            data.Post_Code,
                            publicIP,
                            data.Places[0].Place_Name,
                            data.Places[0].State_Abbreviation,
                            data.Places[0].Latitude,
                            data.Places[0].Longitude);
            }
            else if(locationData.GetType() == typeof(IPData.IPData))
            {
                var data = (IPData.IPData)locationData;
                Console.WriteLine("  {0} | {1} | {2}, {3} | {4}, {5}",
                            data.Data.Postal_Code,
                            publicIP,
                            data.Data.City_Name,
                            data.Data.Subdivision_1_ISO_Code,
                            data.Data.Latitude,
                            data.Data.Longitude);
            }
        }

        public static void PrintWeatherData(DarkSkyData weather)
        {
            Console.WriteLine("  Current temp: {0}{1}F", weather.currently.temperature, (char)0176);
        }

        public static void PrintLocationData2(Location location)
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
