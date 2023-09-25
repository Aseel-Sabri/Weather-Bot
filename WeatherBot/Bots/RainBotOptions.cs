namespace WeatherBot.Bots;

public class RainBotOptions : IWeatherBotOptions
{
    public const string RainBot = "RainBot";
    public bool Enabled { get; set; }
    public string Message { get; set; } = string.Empty;
    public int HumidityThreshold { get; set; }
}