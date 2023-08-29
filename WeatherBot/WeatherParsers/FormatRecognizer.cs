using FluentResults;

namespace WeatherBot.WeatherParsers;

public class FormatRecognizer : IFormatRecognizer
{
    private readonly List<IWeatherParser> _weatherParsers;

    public FormatRecognizer(List<IWeatherParser> weatherParsers)
    {
        _weatherParsers = weatherParsers;
    }

    public Result<IWeatherParser> GetSuitableParser(string input)
    {
        var weatherParser = _weatherParsers.FirstOrDefault(weatherParser => weatherParser.IsMatchingFormat(input));
        return weatherParser is null
            ? Result.Fail("Invalid Format")
            : Result.Ok(weatherParser);
    }
}