using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using WeatherBot.WeatherParsers;

namespace WeatherBot.Tests.WeatherParsersTests;

public class ParserProviderTests
{
    [Theory]
    [AutoMoqData]
    public void ShouldFailWhenNoSuitableParser(
        [Frozen] IEnumerable<IWeatherParser> weatherParsers,
        ParserProvider parserProvider)
    {
        weatherParsers.ToList().ForEach(weatherParser =>
            Mock.Get(weatherParser)
                .Setup(p => p.IsSupportedFormat(It.IsAny<string>()))
                .Returns(false));
        const string errorMessage = "Invalid Format";

        var result = parserProvider.GetSuitableParser(It.IsAny<string>());

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == errorMessage);
    }


    [Theory]
    [AutoMoqData]
    public void ShouldReturnSuitableParserWhenFound(
        [Frozen] IEnumerable<IWeatherParser> weatherParsers,
        ParserProvider parserProvider)
    {
        var suitableWeatherParser = weatherParsers.First();

        Mock.Get(suitableWeatherParser)
            .Setup(p => p.IsSupportedFormat(It.IsAny<string>()))
            .Returns(true);

        var result = parserProvider.GetSuitableParser(It.IsAny<string>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(suitableWeatherParser);
    }
}