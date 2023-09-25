namespace WeatherBot.Bots;

public interface IWeatherBotOptions
{
    bool Enabled { get; set; }
    string Message { get; set; }
}