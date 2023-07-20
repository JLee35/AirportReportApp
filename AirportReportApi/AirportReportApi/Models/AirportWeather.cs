namespace AirportReportApi.Core.Models;

public record AirportWeather
{
    public string Temperature { get; init; }
    public string RelativeHumidity { get; init; }
    public string CloudCoverage { get; init; }
    public string Visibility { get; init; }
    public string WindSpeed { get; init; }
    public string WindDirection { get; init; }
    
    // More to come...
}