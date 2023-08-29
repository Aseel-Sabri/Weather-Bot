namespace WeatherBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var serviceProvider = Startup.ConfigureServices();
        }
    }
}