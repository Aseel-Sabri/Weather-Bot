using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WeatherBot.Bots;
using WeatherBot.WeatherParsers;


namespace WeatherBot;

public static class Startup
{
    private static IConfiguration LoadConfigurations()
    {
        var filePath = GetConfigurationFilePath();

        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(filePath, optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();
        return config;
    }

    private static string GetConfigurationFilePath()
    {
        var fileName = "config.json";
        var filePath =
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                fileName);
        return filePath;
    }

    public static IServiceProvider ConfigureServices()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        ConfigureBotServices(serviceCollection);
        ConfigureParserServices(serviceCollection);

        return serviceCollection
            .BuildServiceProvider();
    }

    private static void ConfigureBotServices(IServiceCollection serviceCollection)
    {
        IConfiguration config = LoadConfigurations();
        var rainBot = config.GetSection("RainBot").Get<RainBot>();
        var sunBot = config.GetSection("SunBot").Get<SunBot>();
        var snowBot = config.GetSection("SnowBot").Get<SnowBot>();

        // TODO
        serviceCollection
            .AddSingleton(rainBot ?? new RainBot())
            .AddSingleton(snowBot ?? new SnowBot())
            .AddSingleton(sunBot ?? new SunBot());
    }

    private static void ConfigureParserServices(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<JsonWeatherParser>()
            .AddSingleton<XmlWeatherParser>()
            .AddSingleton<List<IWeatherParser>>(serviceProvider => new List<IWeatherParser>
            {
                serviceProvider.GetService<JsonWeatherParser>(),
                serviceProvider.GetService<XmlWeatherParser>()
            })
            .AddSingleton<IFormatRecognizer, FormatRecognizer>();
    }
}