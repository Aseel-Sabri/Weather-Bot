using WeatherBot.Models;

namespace WeatherBot.WeatherServices;

public sealed class WeatherServicesUnsubscriber : IDisposable
{
    private readonly ISet<IObserver<WeatherData>> _observers;
    private readonly IObserver<WeatherData> _observer;

    internal WeatherServicesUnsubscriber(
        ISet<IObserver<WeatherData>> observers,
        IObserver<WeatherData> observer) => (_observers, _observer) = (observers, observer);

    public void Dispose() => _observers.Remove(_observer);
}