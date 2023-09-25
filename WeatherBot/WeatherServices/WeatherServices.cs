using FluentResults;
using WeatherBot.Models;
using WeatherBot.WeatherParsers;

namespace WeatherBot.WeatherServices;

public class WeatherServices : IWeatherServices
{
    private readonly IParserProvider _parserProvider;
    private readonly HashSet<IObserver<WeatherData>> _subscribedWeatherBots = new();

    public WeatherServices(IParserProvider parserProvider)
    {
        _parserProvider = parserProvider;
    }

    public Result UpdateWeather(string weatherRawData)
    {
        var getParserResult = _parserProvider.GetSuitableParser(weatherRawData);
        if (getParserResult.IsFailed)
            return getParserResult.ToResult();

        var parser = getParserResult.Value;
        var weatherDataResult = parser.ParseWeatherInfo(weatherRawData);
        if (weatherDataResult.IsFailed)
            return weatherDataResult.ToResult();

        foreach (var weatherBot in _subscribedWeatherBots)
        {
            weatherBot.OnNext(weatherDataResult.Value);
        }

        return Result.Ok();
    }

    public IDisposable Subscribe(IObserver<WeatherData> observer)
    {
        _subscribedWeatherBots.Add(observer);
        return new WeatherServicesUnsubscriber(_subscribedWeatherBots, observer);
    }
}