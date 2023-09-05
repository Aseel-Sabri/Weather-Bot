using FluentResults;

namespace WeatherBot.WeatherParsers;

public interface IParserProvider
{
    Result<IWeatherParser> GetSuitableParser(string input);
}