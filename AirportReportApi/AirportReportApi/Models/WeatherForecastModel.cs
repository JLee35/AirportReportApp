namespace AirportReportApi.Core.Models;

public record WeatherForecastModel
{
    public string? TimeOffset { get; set; }
    
    public List<WindForecastModel>? WindForecasts { get; set; }
}