using WeatherBot.Models;
using WeatherBot.WeatherServices;

namespace WeatherBot.Bots;

public abstract class WeatherBotBase : IWeatherBot
{
    private IDisposable? _unsubscriber;

    public virtual IWeatherBotOptions Options { get; set; }

    public virtual void PrintActivationMessage()
    {
        Console.WriteLine($"{GetType().Name} activated!");
        Console.Write($"{GetType().Name}: ");
        Console.WriteLine(Options.Message);
        Console.WriteLine();
    }

    public virtual void SubscribeIfEnabled(IWeatherServices provider)
    {
        if (Options.Enabled)
            _unsubscriber = provider.Subscribe(this);
    }


    public virtual void OnCompleted()
    {
        // Do Nothing
    }

    public virtual void OnError(Exception error)
    {
        throw error;
    }

    public abstract void OnNext(WeatherData weatherData);
}