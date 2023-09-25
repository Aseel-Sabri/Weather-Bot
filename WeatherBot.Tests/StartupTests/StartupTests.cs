using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using WeatherBot.Bots;
using WeatherBot.UserInterface;
using WeatherBot.WeatherParsers;
using WeatherBot.WeatherServices;

namespace WeatherBot.Tests.StartupTests;

public class StartupTests
{
    [Fact]
    public void Should_RegisterParserServices()
    {
        // Act 
        var serviceProvider = Startup.ConfigureServices();

        // Assert
        using (new AssertionScope())
        {
            serviceProvider.GetServices<IWeatherParser>().Should()
                .Contain(parser => parser.GetType() == typeof(JsonWeatherParser))
                .And.Contain(parser => parser.GetType() == typeof(XmlWeatherParser));

            serviceProvider.GetService<IParserProvider>().Should().NotBeNull();
        }
    }

    [Fact]
    public void Should_RegisterWeatherServices()
    {
        // Act 
        var serviceProvider = Startup.ConfigureServices();

        // Assert
        serviceProvider.GetService<IWeatherServices>().Should().NotBeNull();
    }

    [Fact]
    public void Should_RegisterBotsServices()
    {
        // Act 
        var serviceProvider = Startup.ConfigureServices();

        // Assert
        serviceProvider.GetService<RainBotOptions>().Should().NotBeNull();
        serviceProvider.GetService<SunBotOptions>().Should().NotBeNull();
        serviceProvider.GetService<SnowBotOptions>().Should().NotBeNull();

        serviceProvider.GetServices<IWeatherBot>().Should()
            .Contain(bot => bot.GetType() == typeof(RainBot))
            .Which.Options.Should().Be(serviceProvider.GetService<RainBotOptions>());

        serviceProvider.GetServices<IWeatherBot>().Should()
            .Contain(bot => bot.GetType() == typeof(SunBot))
            .Which.Options.Should().Be(serviceProvider.GetService<SunBotOptions>());

        serviceProvider.GetServices<IWeatherBot>().Should()
            .Contain(bot => bot.GetType() == typeof(SnowBot))
            .Which.Options.Should().Be(serviceProvider.GetService<SnowBotOptions>());
    }

    [Fact]
    public void Should_RegisterUserInterfaceServices()
    {
        // Act 
        var serviceProvider = Startup.ConfigureServices();

        // Assert
        serviceProvider.GetService<IUserInterface>().Should().NotBeNull();
    }
}