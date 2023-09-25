using WeatherBot.Models;

namespace WeatherBot.Bots;

public class SnowBot : WeatherBotBase
{
    private SnowBotOptions _options;

    public SnowBot(SnowBotOptions options)
    {
        _options = options;
    }

    public override IWeatherBotOptions Options
    {
        get => _options;
        set => _options = value as SnowBotOptions ?? throw new InvalidOperationException();
    }

    public override void OnNext(WeatherData weatherData)
    {
        if (weatherData.Temperature < _options.TemperatureThreshold)
            PrintActivationMessage();
    }
}