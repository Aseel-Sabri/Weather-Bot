using System.Text.Json;
using FluentResults;
using WeatherBot.Models;

namespace WeatherBot.WeatherParsers;

public class JsonWeatherParser : IWeatherParser
{
    public Result<WeatherData> ParseWeatherInfo(string weatherRawData)
    {
        var weatherData = JsonSerializer.Deserialize<WeatherData>(weatherRawData);
        return weatherData is null
            ? Result.Fail("Invalid JSON Format")
            : Result.Ok(weatherData);
    }
}