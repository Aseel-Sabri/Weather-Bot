namespace WeatherBot.Bots;

public interface IWeatherBot : IObserver<object>
{
    public bool Enabled { get; set; }
    public string Message { get; set; }

    protected void PrintActivationMessage();
}