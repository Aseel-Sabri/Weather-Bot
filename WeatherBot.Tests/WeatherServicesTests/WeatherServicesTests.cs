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
    public void ShouldFailWhenNoSuitableParser(
        string errorMessage,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        mockParserProvider
            .Setup(p => p.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Fail(errorMessage));

        var result = weatherServices.UpdateWeather(It.IsAny<string>());
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == errorMessage);
    }

    [Theory]
    [AutoMoqData]
    public void ShouldFailWhenParsingFails(
        string errorMessage,
        Mock<IWeatherParser> mockWeatherParser,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        mockParserProvider
            .Setup(p => p.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Ok(mockWeatherParser.Object));

        mockWeatherParser
            .Setup(p => p.ParseWeatherInfo(It.IsAny<string>()))
            .Returns(Result.Fail(errorMessage));

        var result = weatherServices.UpdateWeather(It.IsAny<string>());
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == errorMessage);
    }


    [Theory]
    [AutoMoqData]
    public void ShouldNotifySubscribedBotsWhenParsingSucceed(
        WeatherData weatherData,
        List<IWeatherBot> weatherBots,
        Mock<IWeatherParser> mockWeatherParser,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        weatherBots.ForEach(weatherBot => weatherServices.Subscribe(weatherBot));

        mockParserProvider
            .Setup(p => p.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Ok(mockWeatherParser.Object));

        mockWeatherParser
            .Setup(p => p.ParseWeatherInfo(It.IsAny<string>()))
            .Returns(Result.Ok(weatherData));

        weatherServices.UpdateWeather(It.IsAny<string>());

        weatherBots.ForEach(weatherBot =>
            Mock.Get(weatherBot).Verify(p => p.OnNext(weatherData), Times.Once));
    }


    [Theory]
    [AutoMoqData]
    public void ShouldReturnOkWhenParsingSucceed(
        Mock<IWeatherParser> mockWeatherParser,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        mockParserProvider
            .Setup(p => p.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Ok(mockWeatherParser.Object));

        mockWeatherParser
            .Setup(p => p.ParseWeatherInfo(It.IsAny<string>()))
            .Returns(Result.Ok(It.IsAny<WeatherData>()));

        var result = weatherServices.UpdateWeather(It.IsAny<string>());

        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [AutoMoqData]
    public void ShouldPassSameWeatherRawDataToParserProviderAndParser(
        string weatherRawData,
        Mock<IWeatherParser> mockWeatherParser,
        [Frozen] Mock<IParserProvider> mockParserProvider,
        WeatherServices.WeatherServices weatherServices)
    {
        mockParserProvider
            .Setup(p => p.GetSuitableParser(It.IsAny<string>()))
            .Returns(Result.Ok(mockWeatherParser.Object));

        mockWeatherParser
            .Setup(p => p.ParseWeatherInfo(It.IsAny<string>()))
            .Returns(Result.Ok(It.IsAny<WeatherData>()));

        var result = weatherServices.UpdateWeather(weatherRawData);

        mockParserProvider.Verify(p => p.GetSuitableParser(weatherRawData));
        mockWeatherParser.Verify(p => p.ParseWeatherInfo(weatherRawData));
    }
}