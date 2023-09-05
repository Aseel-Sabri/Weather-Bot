using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WeatherBot.Bots;
using WeatherBot.UserInterface;
using WeatherBot.WeatherParsers;
using WeatherBot.WeatherServices;


namespace WeatherBot;

public static class Startup
{
    private const string _configFile = "appsettings.json";

    public static IServiceProvider ConfigureServices()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        ConfigureParserServices(serviceCollection);
        ConfigureUserInterfaceServices(serviceCollection);
        ConfigureWeatherServices(serviceCollection);
        ConfigureBotsOptions(serviceCollection);
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
        serviceCollection
            .AddSingleton<IWeatherBot, RainBot>()
            .AddSingleton<IWeatherBot, SnowBot>()
            .AddSingleton<IWeatherBot, SunBot>();
    }

    private static void ConfigureBotsOptions(IServiceCollection serviceCollection)
    {
        var builder = new ConfigurationBuilder();

        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(_configFile, optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        var rainBotOptions = config.GetSection(RainBotOptions.RainBot).Get<RainBotOptions>() ?? new RainBotOptions();
        serviceCollection.AddSingleton<RainBotOptions>(rainBotOptions);

        var snowBotOptions = config.GetSection(SnowBotOptions.SnowBot).Get<SnowBotOptions>() ?? new SnowBotOptions();
        serviceCollection.AddSingleton<SnowBotOptions>(snowBotOptions);

        var sunBotOptions = config.GetSection(SunBotOptions.SunBot).Get<SunBotOptions>() ?? new SunBotOptions();
        serviceCollection.AddSingleton<SunBotOptions>(sunBotOptions);
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