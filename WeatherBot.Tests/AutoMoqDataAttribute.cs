using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace WeatherBot.Tests;

[AttributeUsage(AttributeTargets.Method)]
public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true, GenerateDelegates = true });

        return fixture;
    })
    {
    }
}