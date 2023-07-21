namespace AirportReportApi.Core.Models;

public record WeatherForecastModel
{
    public decimal WindSpeedKts { get; init; }
    public int WindDirectionDegrees { get; init; }
}