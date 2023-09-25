namespace WeatherBot.Bots;

public class SunBotOptions : IWeatherBotOptions
{
    public const string SunBot = "SunBot";
    public bool Enabled { get; set; }
    public string Message { get; set; } = string.Empty;
    public int TemperatureThreshold { get; set; }
}