using FluentResults;

namespace WeatherBot.WeatherParsers;

public interface IFormatRecognizer
{
    Result<IWeatherParser> GetSuitableParser(string input);
}