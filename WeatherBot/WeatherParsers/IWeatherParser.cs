using FluentResults;
using WeatherBot.Models;

namespace WeatherBot.WeatherParsers;

public interface IWeatherParser
{
    Result<WeatherData> ParseWeatherInfo(string weatherRawData);
}