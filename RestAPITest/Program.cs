using System;
using System.Linq;

namespace WeatherAPIFetcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a 5-digit zipcode to get the forecast (US zipcodes only).\n" +
                "A blank entry will attempt to fetch your location via your public IP.\n" +
                "Powered by Dark Sky - https://darksky.net/dev \n" +
                "Enter 'N' to exit.\n");

            while(true)
            {
                Console.WriteLine("-----");
                Console.Write("Enter a zipcode: ");
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
                Console.WriteLine("  {0} | {1}, {2} | {3}, {4} | {5}",
                            data.Post_Code,
                            data.Places[0].Place_Name,
                            data.Places[0].State_Abbreviation,
                            data.Places[0].Latitude,
                            data.Places[0].Longitude,
                            publicIP);
            }
            else if(locationData.GetType() == typeof(IPData.IPData))
            {
                var data = (IPData.IPData)locationData;
                Console.WriteLine("  {0} | {1}, {2} | {3}, {4} | {5}",
                            data.Data.Postal_Code,
                            data.Data.City_Name,
                            data.Data.Subdivision_1_ISO_Code,
                            data.Data.Latitude,
                            data.Data.Longitude,
                            publicIP);
            }
        }

        public static void PrintWeatherData(DarkSkyData weather)
        {
            Console.WriteLine("  Current temp: {0}{1}F\n", weather.currently.temperature, (char)0176);

            DateTime firstDate = DateTimeOffset.FromUnixTimeSeconds(weather.daily.data.First().time).UtcDateTime;
            DateTime lastDate = DateTimeOffset.FromUnixTimeSeconds(weather.daily.data.Last().time).UtcDateTime;

            Console.WriteLine("  Forecast for {0}-{1}:", firstDate.ToString("MMMM d"), lastDate.ToString("MMMM d"));
            Console.WriteLine($"  {"Date",-20}| {"Low (°F)",-10}| {"High (°F)",-10}| {"Summary"}");
            Console.WriteLine("  ".PadRight(85, '-'));
            foreach (var day in weather.daily.data)
            {
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(day.time).UtcDateTime;

                Console.WriteLine("  {0,-20}| {1,-10}| {2,-10}| {3}",
                    date.ToString("ddd, MMMM d"),
                    day.temperatureLow,
                    day.temperatureHigh,
                    day.summary);
            }
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
