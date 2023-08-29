using WeatherBot.Models;
using WeatherBot.WeatherServices;

namespace WeatherBot.Bots;

public abstract class WeatherBotBase : IWeatherBot
{
    public bool Enabled { get; set; }
    public string Message { get; set; } = string.Empty;

    private IDisposable? _unsubscriber;

    public virtual void PrintActivationMessage()
    {
        Console.WriteLine($"{GetType().Name} activated!");
        Console.Write($"{GetType().Name}: ");
        Console.WriteLine(Message);
        Console.WriteLine();
    }

    public virtual void SubscribeIfEnabled(IWeatherServices provider)
    {
        if (Enabled)
            _unsubscriber = provider.Subscribe(this);
    }


    public virtual void OnCompleted()
    {
        _unsubscriber?.Dispose();
    }

    public virtual void OnError(Exception error)
    {
        throw error;
    }

    public abstract void OnNext(WeatherData weatherData);
}