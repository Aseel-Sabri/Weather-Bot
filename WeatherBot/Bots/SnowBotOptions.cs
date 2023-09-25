namespace WeatherBot.Bots;

public class SnowBotOptions : IWeatherBotOptions
{
    public const string SnowBot = "SnowBot";
    public bool Enabled { get; set; }
    public string Message { get; set; } = string.Empty;
    public int TemperatureThreshold { get; set; }
}