using Moq;
using WeatherBot.Bots;
using WeatherBot.WeatherServices;

namespace WeatherBot.Tests.WeatherBotsTests;

public class WeatherBotBaseTests
{
    [Theory]
    [AutoMoqData]
    public void Should_SubscribeWeatherServices_When_Enabled(
        Mock<WeatherBotBase> mockWeatherBot,
        Mock<IWeatherServices> mockWeatherServices)
    {
        const int callsCount = 3;
        mockWeatherBot.SetupSequence(bot => bot.Options.Enabled)
            .Returns(true)
            .Returns(false)
            .Returns(true);

        mockWeatherBot.Setup(bot => bot.SubscribeIfEnabled(It.IsAny<IWeatherServices>())).CallBase();

        for (int i = 0; i < callsCount; i++)
        {
            mockWeatherBot.Object.SubscribeIfEnabled(mockWeatherServices.Object);
        }

        mockWeatherServices.Verify(weatherService => weatherService.Subscribe(mockWeatherBot.Object), Times.Exactly(2));
    }
}