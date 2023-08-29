using WeatherBot.Models;

namespace WeatherBot.Bots;

public class RainBot : WeatherBotBase
{
    public int HumidityThreshold { get; set; }

    public override void OnNext(WeatherData weatherData)
    {
        if (weatherData.Humidity > HumidityThreshold)
            PrintActivationMessage();
    }
}