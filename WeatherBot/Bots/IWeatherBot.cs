using WeatherBot.Models;
using WeatherBot.WeatherServices;

namespace WeatherBot.Bots;

public interface IWeatherBot : IObserver<WeatherData>
{
    public bool Enabled { get; set; }
    public string Message { get; set; }

    protected void PrintActivationMessage();

    void SubscribeIfEnabled(IWeatherServices provider);
}