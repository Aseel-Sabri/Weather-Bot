using Microsoft.Extensions.DependencyInjection;
using WeatherBot.UserInterface;


namespace WeatherBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Startup.ConfigureServices();
            var userInterface = serviceProvider.GetRequiredService<IUserInterface>();
            userInterface.Run();
        }
    }
}