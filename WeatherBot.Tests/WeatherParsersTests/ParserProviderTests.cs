using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using WeatherBot.WeatherParsers;

namespace WeatherBot.Tests.WeatherParsersTests;

public class ParserProviderTests
{
    [Theory]
    [AutoMoqData]
    public void Should_Fail_When_NoSuitableParser(
        [Frozen] IEnumerable<IWeatherParser> weatherParsers,
        ParserProvider parserProvider)
    {
        // Arrange
        weatherParsers.ToList().ForEach(weatherParser =>
            Mock.Get(weatherParser)
                .Setup(parser => parser.IsSupportedFormat(It.IsAny<string>()))
                .Returns(false));

        // Act
        var result = parserProvider.GetSuitableParser(It.IsAny<string>());

        // Assert
        result.IsFailed.Should().BeTrue();
    }


    [Theory]
    [AutoMoqData]
    public void Should_ReturnSuitableParser_When_Found(
        [Frozen] IEnumerable<IWeatherParser> weatherParsers,
        ParserProvider parserProvider)
    {
        // Arrange
        var suitableWeatherParser = weatherParsers.First();
        Mock.Get(suitableWeatherParser)
            .Setup(parser => parser.IsSupportedFormat(It.IsAny<string>()))
            .Returns(true);

        // Act
        var result = parserProvider.GetSuitableParser(It.IsAny<string>());

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(suitableWeatherParser);
    }
}