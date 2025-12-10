using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    // Map state codes to representative lat/lon (approximate center of main city)
    static readonly Dictionary<string, string> stateToLatLon = new()
    {
        {"CO", "39.7392,-104.9903"}, // Denver
        {"NY", "42.6526,-73.7562"},  // Albany
        {"CA", "34.0522,-118.2437"}, // Los Angeles
        {"TX", "29.7604,-95.3698"}   // Houston
    };

    static async Task Main()
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("CanAmWeatherApp/1.0");

        Console.WriteLine("Welcome to the Can/Am Weather App!");
        Console.Write("Enter a US state code (e.g., CO, NY, CA): ");
        string? state = Console.ReadLine()?.ToUpper();

        if (string.IsNullOrWhiteSpace(state) || !stateToLatLon.ContainsKey(state))
        {
            Console.WriteLine($"No coordinates found for '{state}'.");
            return;
        }

        string latLon = stateToLatLon[state];
        string pointsUrl = $"https://api.weather.gov/points/{latLon}";

        HttpResponseMessage pointsResponse;
        try
        {
            pointsResponse = await client.GetAsync(pointsUrl);
            pointsResponse.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching point data: {ex.Message}");
            return;
        }

        string pointsJson = await pointsResponse.Content.ReadAsStringAsync();
        JsonDocument pointsDoc = JsonDocument.Parse(pointsJson);

        if (!pointsDoc.RootElement.TryGetProperty("properties", out JsonElement props) ||
            !props.TryGetProperty("forecast", out JsonElement forecastElem) ||
            forecastElem.ValueKind != JsonValueKind.String)
        {
            Console.WriteLine("Could not find forecast URL in point data.");
            return;
        }

        string forecastUrl = forecastElem.GetString()!;

        HttpResponseMessage forecastResponse;
        try
        {
            forecastResponse = await client.GetAsync(forecastUrl);
            forecastResponse.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching forecast: {ex.Message}");
            return;
        }

        string forecastJson = await forecastResponse.Content.ReadAsStringAsync();
        JsonDocument forecastDoc = JsonDocument.Parse(forecastJson);

        if (!forecastDoc.RootElement.TryGetProperty("properties", out JsonElement forecastProps) ||
            !forecastProps.TryGetProperty("periods", out JsonElement periods))
        {
            Console.WriteLine("Forecast data is unavailable.");
            return;
        }

        Console.WriteLine($"\nForecast for {state} (approx. {latLon}):\n");

        foreach (var p in periods.EnumerateArray())
        {
            string name = p.GetProperty("name").GetString() ?? "N/A";
            string detailed = p.GetProperty("detailedForecast").GetString() ?? "No details";
            int temp = p.GetProperty("temperature").GetInt32();
            string unit = p.GetProperty("temperatureUnit").GetString() ?? "N/A";

            Console.WriteLine(name);
            Console.WriteLine($"Temp: {temp}{unit}");
            Console.WriteLine(detailed);
            Console.WriteLine("----------------------");
        }
    }
}
