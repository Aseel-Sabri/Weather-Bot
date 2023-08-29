using WeatherBot.Models;

namespace WeatherBot.Bots;

public class SnowBot : WeatherBotBase
{
    public int TemperatureThreshold { get; set; }

    public override void OnNext(WeatherData weatherData)
    {
        if (weatherData.Temperature < TemperatureThreshold)
            PrintActivationMessage();
    }
}