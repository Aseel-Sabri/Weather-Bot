using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using FluentResults;
using WeatherBot.Models;


namespace WeatherBot.WeatherParsers;

public class XmlWeatherParser : IWeatherParser
{
    public Result<WeatherData> ParseWeatherInfo(string weatherRawData)
    {
        const string errorMessage = "Invalid XML Format";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(weatherRawData));
        var serializer = new XmlSerializer(typeof(WeatherData));
        try
        {
            var weatherData = (WeatherData?)serializer.Deserialize(stream);
            return weatherData is null
                ? Result.Fail(errorMessage)
                : Result.Ok(weatherData);
        }
        catch (Exception e)
        {
            return Result.Fail(errorMessage);
        }
    }

    public bool IsSupportedFormat(string input)
    {
        const string xmlPattern = @"^\s*<(\s|.)*>\s*$";
        return Regex.IsMatch(input, xmlPattern);
    }
}