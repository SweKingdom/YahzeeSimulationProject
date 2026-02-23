using System.Collections.Immutable;
using Newtonsoft.Json;
using Models.OpenWeather;

namespace Playground.Lesson11.Repositories;

public class OpenWeatherRepos
{
    private readonly HttpClient _httpClient;
    //Get your own API key at https://home.openweathermap.org
    private readonly string _apiKey = "YOUR_API_KEY";

    public OpenWeatherRepos()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/")
        };
    }
    public async Task<Forecast> GetForecastAsync(string City)
    {
        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={_apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);
        return forecast;

    }
    private async Task<Forecast> ReadWebApiAsync(string uri)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        
        //Convert Json to NewsResponse
        string content = await response.Content.ReadAsStringAsync();
        WeatherApiData wd = JsonConvert.DeserializeObject<WeatherApiData>(content);

        var forecast = new Forecast()
        {
            City = wd.city.name,
            Items = wd.list.Select(wdle => new ForecastItem()
            {
                DateTime = UnixTimeStampToDateTime(wdle.dt),
                Temperature = wdle.main.temp,
                WindSpeed = wdle.wind.speed,
                Description = wdle.weather.First().description,
                Icon = $"http://openweathermap.org/img/w/{wdle.weather.First().icon}.png"
            }).ToImmutableList()
        };
        return forecast;
    }
    private DateTime UnixTimeStampToDateTime(double unixTimeStamp) => DateTime.UnixEpoch.AddSeconds(unixTimeStamp).ToLocalTime();

}

