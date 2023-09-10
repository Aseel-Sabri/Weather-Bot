# Weather Bot Console Application

## Table of Contents

- [Description](#description)
- [Features](#features)
- [Usage](#usage)
- [Configuration](#configuration)
- [Design Patterns](#design-patterns)

## Description

This console application allows users to analyze weather data in JSON or XML format using three types of bots: rain bot, sun bot, and snow bot. Each bot is activated based on specific weather conditions and displays a pre-configured message on the screen when triggered. The application loads its configurations from an `appsettings.json` file.

## Features

- Analyze weather data in JSON or XML format.
- Three types of bots:
  - Rain bot: Activated when humidity is above a threshold.
  - Sun bot: Activated when temperature is above a threshold.
  - Snow bot: Activated when temperature is below a threshold.
- Configurable bot messages and activation thresholds.
- Observer and Strategy design patterns for extensibility.


## Usage

To use the Weather Bot Console Application, follow these steps:

1. Ensure you have .NET Core installed on your system.

2. Run the application using the following command:

	```bash
	dotnet run
	```

3.  You will be prompted to enter weather data in JSON format. For example:
	``` plaintext
	Enter weather data:
	{"Location": "City Name", "Temperature": 32, "Humidity": 40}
	```
    
4.  Press Enter to submit the weather data.
    
5.  The application will analyze the data and display messages from the activated bots. For instance:
    
	``` plaintext
	SunBot activated!
	SunBot: "Wow, it's a scorcher out there!"
	```
    
6.  You can exit the application by entering `q` or `quit`.
    
## Configuration

The application's configuration is stored in the `appsettings.json` file. You can customize the bot's behavior by modifying this file. Here's an example of the configuration structure:

```json
{
  "RainBot": {
    "enabled": true,
    "humidityThreshold": 70,
    "message": "It looks like it's about to pour down!"
  },
  "SunBot": {
    "enabled": true,
    "temperatureThreshold": 30,
    "message": "Wow, it's a scorcher out there!"
  },
  "SnowBot": {
    "enabled": false,
    "temperatureThreshold": 0,
    "message": "Brrr, it's getting chilly!"
  }
}
```

Adjust the threshold values and bot messages to suit your preferences.


## Design Patterns

This project utilizes the Strategy design pattern to determine the appropriate parser for weather data based on its format (JSON or XML). The Strategy pattern provides a flexible way to switch between different parsing strategies without altering the core logic. Additionally, the Observer design pattern is employed to notify and trigger different weather bots (rain bot, sun bot, snow bot) when weather data is updated. This design pattern allows for easy extensibility by adding new bot types in the future without modifying existing code. These design patterns enhance the maintainability and scalability of the application by separating concerns and promoting code reusability.