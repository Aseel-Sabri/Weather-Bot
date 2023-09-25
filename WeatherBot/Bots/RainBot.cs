using WeatherBot.Models;

namespace WeatherBot.Bots;

public class RainBot : WeatherBotBase
{
    private RainBotOptions _options;

    public RainBot(RainBotOptions options)
    {
        _options = options;
    }

    public override IWeatherBotOptions Options
    {
        get => _options;
        set => _options = value as RainBotOptions ?? throw new InvalidOperationException();
    }

    public override void OnNext(WeatherData weatherData)
    {
        if (weatherData.Humidity > _options.HumidityThreshold)
            PrintActivationMessage();
    }
}