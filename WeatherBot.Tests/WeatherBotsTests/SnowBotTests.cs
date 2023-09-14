using AutoFixture.Xunit2;
using FluentAssertions;
using WeatherBot.Bots;
using WeatherBot.Models;

namespace WeatherBot.Tests.WeatherBotsTests;

public class SnowBotTests
{
    [Theory]
    [InlineAutoData(30, 29.5)]
    public void Should_PrintMessage_When_NotifiedWithTemperatureUnderThreshold(
        int temperatureThreshold, double actualTemperature, SnowBotOptions botOptions, WeatherData weatherData)
    {
        // Arrange
        botOptions.TemperatureThreshold = temperatureThreshold;
        weatherData.Temperature = actualTemperature;
        var writer = new StringWriter();
        Console.SetOut(writer);
        var snowBot = new SnowBot(botOptions);

        // Act
        snowBot.OnNext(weatherData);
        var consoleOutput = writer.ToString();

        // Assert
        consoleOutput.Should().Contain(botOptions.Message);
    }

    [Theory]
    [InlineAutoData(30, 30.5)]
    public void Should_NotPrintMessage_When_NotifiedWithTemperatureAboveThreshold(
        int temperatureThreshold, double actualTemperature, SnowBotOptions botOptions, WeatherData weatherData)
    {
        // Arrange
        botOptions.TemperatureThreshold = temperatureThreshold;
        weatherData.Temperature = actualTemperature;
        var writer = new StringWriter();
        Console.SetOut(writer);
        var snowBot = new SnowBot(botOptions);

        // Act
        snowBot.OnNext(weatherData);
        var consoleOutput = writer.ToString();

        // Assert
        consoleOutput.Should().BeEmpty();
    }
}