using WeatherBot.Models;
using WeatherBot.WeatherServices;

namespace WeatherBot.Bots;

public interface IWeatherBot : IObserver<WeatherData>
{
    IWeatherBotOptions Options { get; set; }

    protected void PrintActivationMessage();

    void SubscribeIfEnabled(IWeatherServices provider);
}