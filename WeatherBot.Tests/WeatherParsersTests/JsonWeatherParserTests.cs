using AutoFixture.Xunit2;
using FluentAssertions;
using WeatherBot.WeatherParsers;

namespace WeatherBot.Tests.WeatherParsersTests;

public class JsonWeatherParserTests
{
    [Theory]
    [InlineAutoData("{\"Location\": \"City Name\", \"Temperature\": 32, \"Humidity\": 40")]
    [InlineAutoData("{\"Location\": \"City Name\", \"Temperature\": 32, \"Humidity\" 40}")]
    [InlineAutoData("{\"Location\": \"City Name\", \"Temperature\": temp, \"Humidity\": 40}")]
    [InlineAutoData(
        "<WeatherData><Location>City Name</Location><Temperature>32</Temperature><Humidity>40</Humidity></WeatherData>")]
    public void Should_Fail_When_InvalidInputFormat(string weatherRawData, JsonWeatherParser weatherParser)
    {
        // Act
        var result = weatherParser.ParseWeatherInfo(weatherRawData);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Theory]
    [InlineAutoData("{\"Location\": \"City Name\", \"Temperature\": 32, \"Humidity\": 40}",
        "City Name", 32, 40)]
    public void Should_ReturnWeatherData_When_ValidInput(
        string weatherRawData,
        string location,
        double temperature,
        double humidity,
        JsonWeatherParser weatherParser)
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
    [InlineAutoData("{\"Location\": \"City Name\", \"Temperature\": 32, \"Humidity\": 40}")]
    public void IsSupportedFormat_Should_ReturnTrue_When_ValidJsonInput(
        string weatherRawData,
        JsonWeatherParser weatherParser)
    {
        // Act
        var isSupportedFormat = weatherParser.IsSupportedFormat(weatherRawData);

        // Assert
        isSupportedFormat.Should().BeTrue();
    }
}