using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using FluentResults;
using WeatherBot.Models;


namespace WeatherBot.WeatherParsers;

public class XmlWeatherParser : IWeatherParser
{
    public Result<WeatherData> ParseWeatherInfo(string? weatherRawData)
    {
        const string errorMessage = "Invalid XML Format";
        if (string.IsNullOrWhiteSpace(weatherRawData))
        {
            return Result.Fail(errorMessage);
        }

        WeatherData? weatherData;
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(weatherRawData)))
        {
            var serializer = new XmlSerializer(typeof(WeatherData));
            weatherData = (WeatherData?)serializer.Deserialize(stream);
        }

        return weatherData is null
            ? Result.Fail(errorMessage)
            : Result.Ok(weatherData);
    }

    public bool IsMatchingFormat(string input)
    {
        const string xmlPattern = @"^\s*<(\s|.)*>\s*$";
        return Regex.IsMatch(input, xmlPattern);
    }
}