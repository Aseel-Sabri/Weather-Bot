using WeatherBot.Models;

namespace WeatherBot.Bots;

public class SunBot : WeatherBotBase
{
    private SunBotOptions _options;

    public SunBot(SunBotOptions options)
    {
        _options = options;
    }

    public override IWeatherBotOptions Options
    {
        get => _options;
        set => _options = value as SunBotOptions ?? throw new InvalidOperationException();
    }

    public override void OnNext(WeatherData weatherData)
    {
        if (weatherData.Temperature > _options.TemperatureThreshold)
            PrintActivationMessage();
    }
}