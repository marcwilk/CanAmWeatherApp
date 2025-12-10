# Can/Am Weather App

A simple C# console application that fetches and displays weather forecasts from the **National Weather Service API** for US states.

---

## Features

- Allows the user to input a US state code (e.g., `CO`, `NY`, `CA`).
- Uses representative latitude and longitude for the main city in each state.
- Retrieves forecast data from the National Weather Service API.
- Displays detailed daily forecasts including:
  - Period name (e.g., "Today", "Tonight")
  - Temperature and unit
  - Detailed forecast description

---

## Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Internet connection to access the National Weather Service API

---

## Usage

1. Open a terminal in the project directory.
2. Build the project:
   dotnet build
3. Run the application:
    dotnet run
4. When prompted, enter a US state code:
    Enter a US state code (e.g., CO, NY, CA): CO
5. The app will display the forecast for that state’s main city.

## Example Output

Welcome to the Can/Am Weather App!
Enter a US state code (e.g., CO, NY, CA): CO

Forecast for CO (approx. 39.7392,-104.9903):

Today
Temp: 75F
Sunny with light winds.

Tonight
Temp: 50F
Clear skies overnight.
