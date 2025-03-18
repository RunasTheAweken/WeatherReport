using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

[Route("api/WeatherReport")]
[ApiController]
public class Forecast : ControllerBase
{
    private const string BASE_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";
    private const string API_KEY = "H2RFQJDDVZFD7VXT6E5V5GAED";
    private HttpClient client;

    private readonly IDatabase redisDb;
    public Forecast()
    {
        client = new HttpClient();
        var redis = ConnectionMultiplexer.Connect("localhost");
        redisDb = redis.GetDatabase();
    }
    [HttpGet("{city}/{dateStart}/{dateStop}")]
    public async Task<IActionResult> GetWeatherWithDates(string city, string dateStart, string dateStop)
    {
        return await GetWeather(city, dateStart, dateStop);
    }
    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeatherWithNoDates(string city)
    {
        string dateStart = DateTime.UtcNow.ToString("yyyy-MM-dd");
        string dateStop = DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd");
        return await GetWeather(city, dateStart, dateStop);
    }


    public async Task<IActionResult> GetWeather(string city, string dateStart, string dateStop)
    {
        WeatherResponse? weatherData = null; // <-- объявляем переменную заранее
        string cacheKey = $"{city}:{dateStart}:{dateStop}";
        string? cachedWeather = await redisDb.StringGetAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedWeather))
        {
            weatherData = JsonSerializer.Deserialize<WeatherResponse>(cachedWeather); // <-- убрал var
            return Ok(weatherData);
        }

        string url = $"{BASE_URL}{city}/{dateStart}/{dateStop}?unitGroup=metric&key={API_KEY}";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        weatherData = JsonSerializer.Deserialize<WeatherResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }); // <-- убрал var

        await redisDb.StringSetAsync(cacheKey, json, TimeSpan.FromMinutes(10));
        return Ok(weatherData);
    }


}

public class WeatherResponse
{
    public string? Address { get; set; }
    public string? Timezone { get; set; }
    public string? Description { get; set; }
    public List<DayForecast>? days { get; set; }
}
public class DayForecast
{
    public string? DateTime { get; set; }
    public double Temp { get; set; }
    public double FeelsLike { get; set; }
    public string? Conditions { get; set; }
}