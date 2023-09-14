using AutoFixture.Xunit2;
using FluentAssertions;
using FluentResults;
using Moq;
using WeatherBot.Bots;
using WeatherBot.Models;
using WeatherBot.WeatherParsers;

namespace WeatherBot.Tests.WeatherServicesTests;

public class WeatherServicesTests
{
    [Theory]
    [AutoMoqData]
    public void Should_Fail_When_NoSuitableParser(
        string errorMessage,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        // Arrange
        mockParserProvider
            .Setup(parserProvider => parserProvider.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Fail(errorMessage));

        // Act
        var result = weatherServices.UpdateWeather(It.IsAny<string>());

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(error => error.Message == errorMessage);
    }

    [Theory]
    [AutoMoqData]
    public void Should_Fail_When_ParsingFails(
        string errorMessage,
        Mock<IWeatherParser> mockWeatherParser,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        // Arrange
        mockParserProvider
            .Setup(parserProvider => parserProvider.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Ok(mockWeatherParser.Object));

        mockWeatherParser
            .Setup(parser => parser.ParseWeatherInfo(It.IsAny<string>()))
            .Returns(Result.Fail(errorMessage));

        // Act
        var result = weatherServices.UpdateWeather(It.IsAny<string>());

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(error => error.Message == errorMessage);
    }


    [Theory]
    [AutoMoqData]
    public void Should_NotifySubscribedBots_When_ParsingSucceeds(
        WeatherData weatherData,
        List<IWeatherBot> weatherBots,
        Mock<IWeatherParser> mockWeatherParser,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        // Arrange
        weatherBots.ForEach(weatherBot => weatherServices.Subscribe(weatherBot));

        mockParserProvider
            .Setup(parserProvider => parserProvider.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Ok(mockWeatherParser.Object));

        mockWeatherParser
            .Setup(parser => parser.ParseWeatherInfo(It.IsAny<string>()))
            .Returns(Result.Ok(weatherData));

        // Act
        weatherServices.UpdateWeather(It.IsAny<string>());

        // Assert
        weatherBots.ForEach(weatherBot =>
            Mock.Get(weatherBot).Verify(bot => bot.OnNext(weatherData), Times.Once));
    }


    [Theory]
    [AutoMoqData]
    public void Should_ReturnOk_When_ParsingSucceeds(
        Mock<IWeatherParser> mockWeatherParser,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        // Arrange
        mockParserProvider
            .Setup(parserProvider => parserProvider.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Ok(mockWeatherParser.Object));

        mockWeatherParser
            .Setup(parser => parser.ParseWeatherInfo(It.IsAny<string>()))
            .Returns(Result.Ok(It.IsAny<WeatherData>()));

        // Act
        var result = weatherServices.UpdateWeather(It.IsAny<string>());

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [AutoMoqData]
    public void Should_PassSameInputToParserProviderAndParser(
        string weatherRawData,
        Mock<IWeatherParser> mockWeatherParser,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        // Arrange
        mockParserProvider
            .Setup(parserProvider => parserProvider.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Ok(mockWeatherParser.Object));

        mockWeatherParser
            .Setup(parser => parser.ParseWeatherInfo(It.IsAny<string>()))
            .Returns(Result.Ok(It.IsAny<WeatherData>()));

        // Act
        weatherServices.UpdateWeather(weatherRawData);

        // Assert
        mockParserProvider.Verify(parserProvider => parserProvider.GetSuitableParser(weatherRawData));
        mockWeatherParser.Verify(parser => parser.ParseWeatherInfo(weatherRawData));
    }
}