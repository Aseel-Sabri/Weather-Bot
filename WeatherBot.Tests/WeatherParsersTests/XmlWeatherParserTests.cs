using AutoFixture.Xunit2;
using FluentAssertions;
using WeatherBot.WeatherParsers;

namespace WeatherBot.Tests.WeatherParsersTests;

public class XmlWeatherParserTests
{
    [Theory]
    [InlineAutoData(
        "<WeatherData><Location>City Name</Location><Temperature>32</Temperature><Humidity>40</Humidity></WeatherData")]
    [InlineAutoData(
        "<WeatherData><Location>City Name</Location><Temperature>32</Temperature><Humidity>40<Humidity></WeatherData>")]
    [InlineAutoData(
        "<WeatherData><Location>City Name</Location><Temperature>temp</Temperature><Humidity>40</Humidity></WeatherData>")]
    [InlineAutoData("<Location>City Name</Location><Temperature>32</Temperature><Humidity>40</Humidity></WeatherData>")]
    [InlineAutoData("<Location>City Name</Location><Temperature>32</Temperature><Humidity>40</Humidity>")]
    [InlineAutoData("{\"Location\": \"City Name\", \"Temperature\": 32, \"Humidity\": 40}")]
    public void ParseWeatherInfo_Should_Fail_When_InvalidInputFormat(string weatherRawData,
        XmlWeatherParser weatherParser)
    {
        // Act
        var result = weatherParser.ParseWeatherInfo(weatherRawData);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Theory]
    [InlineAutoData(
        "<WeatherData><Location>City Name</Location><Temperature>32</Temperature><Humidity>40</Humidity></WeatherData>",
        "City Name", 32, 40)]
    public void ParseWeatherInfo_Should_ReturnWeatherData_When_ValidInput(
        string weatherRawData,
        string location,
        double temperature,
        double humidity,
        XmlWeatherParser weatherParser)
    {
        // Act
        var result = weatherParser.ParseWeatherInfo(weatherRawData);
        var weatherData = result.Value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        weatherData.Location.Should().Be(location);
        weatherData.Temperature.Should().Be(temperature);
        weatherData.Humidity.Should().Be(humidity);
    }


    [Theory]
    [InlineAutoData(
        "<WeatherData><Location>City Name</Location><Temperature>32</Temperature><Humidity>40</Humidity></WeatherData>")]
    public void IsSupportedFormat_Should_ReturnTrue_When_ValidXmlInput(
        string weatherRawData,
        XmlWeatherParser weatherParser)
    {
        // Act
        var isSupportedFormat = weatherParser.IsSupportedFormat(weatherRawData);

        // Assert
        isSupportedFormat.Should().BeTrue();
    }
}