using System;
using System.Collections.Generic;

namespace WeatherAPIFetcher
{
    public class ConsolidatedWeather
    {
        public object ID { get; set; }
        public string Weather_State_Name { get; set; }
        public string Weather_State_Abbr { get; set; }
        public string Wind_Direction_Compass { get; set; }
        public DateTime Created { get; set; }
        public DateTime Applicable_Date { get; set; }
        public double Min_Temp { get; set; }
        public double Max_Temp { get; set; }
        public double The_Temp { get; set; }
        public double Wind_Speed { get; set; }
        public double Wind_Direction { get; set; }
        public double Air_Pressure { get; set; }
        public int Humidity { get; set; }
        public double? Visibility { get; set; }
        public int Predictability { get; set; }
    }


    public class Parent
    {
        public string Title { get; set; }
        public string Location_Type { get; set; }
        public int Woeid { get; set; }
        public string Latt_Long { get; set; }
    }

    public class Source
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string URL { get; set; }
        public int Crawl_Rate { get; set; }
    }

    public class Location
    {
        public List<ConsolidatedWeather> Consolidated_Weather { get; set; }
        public DateTime Time { get; set; }
        public DateTime Sun_Rise { get; set; }
        public DateTime Sun_Set { get; set; }
        public string Timezone_Name { get; set; }
        public Parent Parent { get; set; }
        public List<Source> Sources { get; set; }
        public string Title { get; set; }
        public string Location_Type { get; set; }
        public int Woeid { get; set; }
        public string Latt_Long { get; set; }
        public string Timezone { get; set; }
    }
}
