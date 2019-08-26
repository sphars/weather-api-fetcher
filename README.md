# Weather API Fetcher

A very simple weather API consumption C# console appliction, using [RestSharp](https://github.com/restsharp/RestSharp), [Newtonsoft Json.NET](https://www.newtonsoft.com/json) and [MetaWeather](https://www.metaweather.com/).  

This uses WOEID codes to look up information, and there's only so many that have been added to MetaWeather's database. These are also no longer provided by Yahoo. So that's kinda pointless. Will need to move to another weather provider.