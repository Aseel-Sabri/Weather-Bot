using FluentResults;
using WeatherBot.Models;
using WeatherBot.WeatherParsers;

namespace WeatherBot.WeatherServices;

public class WeatherServices : IWeatherServices
{
    private readonly IFormatRecognizer _formatRecognizer;
    private readonly HashSet<IObserver<WeatherData>> _subscribedWeatherBots = new();

    public WeatherServices(IFormatRecognizer formatRecognizer)
    {
        _formatRecognizer = formatRecognizer;
    }

    public Result UpdateWeather(string weatherRawData)
    {
        var parserResult = _formatRecognizer.GetSuitableParser(weatherRawData);
        if (parserResult.IsFailed)
            return parserResult.ToResult();

        var weatherResult = parserResult.Value.ParseWeatherInfo(weatherRawData);
        if (weatherResult.IsFailed)
            return weatherResult.ToResult();

        foreach (var weatherBot in _subscribedWeatherBots)
        {
            weatherBot.OnNext(weatherResult.Value);
        }

        return Result.Ok();
    }

    public IDisposable Subscribe(IObserver<WeatherData> observer)
    {
        _subscribedWeatherBots.Add(observer);
        return new WeatherServicesUnsubscriber(_subscribedWeatherBots, observer);
    }
}