using AutoFixture.Xunit2;

namespace WeatherBot.Tests;

public class InlineAutoMoqDataAttribute : CompositeDataAttribute
{
    public InlineAutoMoqDataAttribute(params object[] values) : base(new InlineDataAttribute(values),
        new AutoMoqDataAttribute())
    {
    }
}