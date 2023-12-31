﻿using WeatherBot.WeatherServices;
using static System.StringComparison;

namespace WeatherBot.UserInterface;

public class ConsoleUserInterface : IUserInterface
{
    private readonly IWeatherServices _weatherServices;

    public ConsoleUserInterface(IWeatherServices weatherServices)
    {
        _weatherServices = weatherServices;
    }

    public void Run()
    {
        var input = GetWeatherDataInput();
        while (!IsQuitCommand(input))
        {
            var updateWeatherResult = _weatherServices.UpdateWeather(input);
            if (updateWeatherResult.IsFailed)
            {
                foreach (var error in updateWeatherResult.Errors)
                {
                    Console.WriteLine(error.Message);
                }
            }

            Console.WriteLine();
            input = GetWeatherDataInput();
        }
    }

    private string GetWeatherDataInput()
    {
        Console.WriteLine("Enter weather data:");
        string? input;
        do
        {
            input = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(input));

        Console.WriteLine();
        return input!.Trim();
    }

    private bool IsQuitCommand(string input)
    {
        return input.Equals("q", CurrentCultureIgnoreCase)
               || input.Equals("quit", CurrentCultureIgnoreCase);
    }
}