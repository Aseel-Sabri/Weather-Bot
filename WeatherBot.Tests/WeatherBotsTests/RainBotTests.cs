using AutoFixture.Xunit2;
using FluentAssertions;
using WeatherBot.Bots;
using WeatherBot.Models;

namespace WeatherBot.Tests.WeatherBotsTests;

public class RainBotTests
{
    [Theory]
    [InlineAutoData(30, 30.5)]
    public void Should_PrintMessage_When_NotifiedWithHumidityAboveThreshold(
        int humidityThreshold, double actualHumidity, RainBotOptions botOptions, WeatherData weatherData)
    {
        // Arrange
        botOptions.HumidityThreshold = humidityThreshold;
        weatherData.Humidity = actualHumidity;
        var writer = new StringWriter();
        Console.SetOut(writer);
        var rainBot = new RainBot(botOptions);

        // Act
        rainBot.OnNext(weatherData);
        var consoleOutput = writer.ToString();

        // Assert
        consoleOutput.Should().Contain(botOptions.Message);
    }

    [Theory]
    [InlineAutoData(30, 29.5)]
    public void Should_NotPrintMessage_When_NotifiedWithHumidityUnderThreshold(
        int humidityThreshold, double actualHumidity, RainBotOptions botOptions, WeatherData weatherData)
    {
        // Arrange
        botOptions.HumidityThreshold = humidityThreshold;
        weatherData.Humidity = actualHumidity;
        var writer = new StringWriter();
        Console.SetOut(writer);
        var rainBot = new RainBot(botOptions);

        // Act
        rainBot.OnNext(weatherData);
        var consoleOutput = writer.ToString();

        // Assert
        consoleOutput.Should().BeEmpty();
    }
}