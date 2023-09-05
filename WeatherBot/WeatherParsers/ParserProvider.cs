using FluentResults;

namespace WeatherBot.WeatherParsers;

public class ParserProvider : IParserProvider
{
    private readonly List<IWeatherParser> _weatherParsers;

    public ParserProvider(IEnumerable<IWeatherParser> weatherParsers)
    {
        _weatherParsers = weatherParsers;
    }

    public Result<IWeatherParser> GetSuitableParser(string input)
    {
        var weatherParser = _weatherParsers.FirstOrDefault(weatherParser => weatherParser.IsSupportedFormat(input));
        return weatherParser is null
            ? Result.Fail("Invalid Format")
            : Result.Ok(weatherParser);
    }
}