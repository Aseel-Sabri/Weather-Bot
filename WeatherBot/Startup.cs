using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WeatherBot.Bots;
using WeatherBot.UserInterface;
using WeatherBot.WeatherParsers;
using WeatherBot.WeatherServices;


namespace WeatherBot;

public static class Startup
{
    public static IServiceProvider ConfigureServices()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        ConfigureParserServices(serviceCollection);
        ConfigureUserInterfaceServices(serviceCollection);
        ConfigureWeatherServices(serviceCollection);
        ConfigureBotServices(serviceCollection);

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        SubscribeBots(serviceProvider);

        return serviceProvider;
    }


    private static void ConfigureParserServices(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IWeatherParser, JsonWeatherParser>()
            .AddSingleton<IWeatherParser, XmlWeatherParser>()
            .AddSingleton<List<IWeatherParser>>(serviceProvider =>
                serviceProvider.GetServices<IWeatherParser>().ToList())
            .AddSingleton<IFormatRecognizer, FormatRecognizer>();
    }

    private static void ConfigureWeatherServices(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IWeatherServices, WeatherServices.WeatherServices>();
    }


    private static void ConfigureBotServices(IServiceCollection serviceCollection)
    {
        IConfiguration config = LoadConfigurations();

        var rainBot = config.GetSection("RainBot").Get<RainBot>() ?? new RainBot();
        var snowBot = config.GetSection("SnowBot").Get<SnowBot>() ?? new SnowBot();
        var sunBot = config.GetSection("SunBot").Get<SunBot>() ?? new SunBot();

        serviceCollection
            .AddSingleton<IWeatherBot>(rainBot)
            .AddSingleton<IWeatherBot>(snowBot)
            .AddSingleton<IWeatherBot>(sunBot);
    }


    private static string GetConfigurationFilePath()
    {
        var fileName = "config.json";
        var filePath =
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                fileName);
        return filePath;
    }

    private static IConfiguration LoadConfigurations()
    {
        var filePath = GetConfigurationFilePath();

        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(filePath, optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();
        return config;
    }


    private static void ConfigureUserInterfaceServices(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IUserInterface, ConsoleUserInterface>();
    }


    private static void SubscribeBots(IServiceProvider serviceProvider)
    {
        var weatherService = serviceProvider.GetService<IWeatherServices>();

        var weatherBots = serviceProvider.GetServices<IWeatherBot>();
        foreach (var weatherBot in weatherBots)
        {
            weatherBot.SubscribeIfEnabled(weatherService);
        }
    }
}