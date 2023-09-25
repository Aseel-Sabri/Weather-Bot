using FluentResults;
using WeatherBot.Models;

namespace WeatherBot.WeatherServices;

public interface IWeatherServices : IObservable<WeatherData>
{
    Result UpdateWeather(string weatherRawData);
}