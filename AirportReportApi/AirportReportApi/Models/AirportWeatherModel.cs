namespace AirportReportApi.Core.Models;

public record AirportWeatherModel
{
    public decimal TemperatureF { get; init; }
    public int RelativeHumidityPercentage { get; init; }
    public string? CloudCoverage { get; init; }
    public decimal VisibilitySm { get; init; }
    public decimal WindSpeedMph { get; init; }
    public int WindDirectionDegrees { get; init; }
}