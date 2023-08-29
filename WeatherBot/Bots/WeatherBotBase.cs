namespace WeatherBot.Bots;

public abstract class WeatherBotBase : IWeatherBot
{
    public bool Enabled { get; set; }
    public string Message { get; set; } = string.Empty;

    public virtual void PrintActivationMessage()
    {
        Console.WriteLine($"{GetType().Name} activated!");
        Console.Write($"{GetType().Name}: ");
        Console.WriteLine(Message);
        Console.WriteLine();
    }

    public virtual void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public virtual void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public virtual void OnNext(object value)
    {
        throw new NotImplementedException();
    }
}