namespace AirportReportApi.Core.Models;

public record AirportWeatherModel
{
    public decimal TemperatureF { get; set; }
    public int RelativeHumidityPercentage { get; set; }
    public string? CloudCoverage { get; set; }
    public decimal VisibilitySm { get; set; }
    public decimal WindSpeedKts { get; set; }
    public bool IsWindVariable { get; set; }
    public int WindDirectionDegrees { get; set; }
    public List<WindForecastModel>? WindForecasts { get; set; }
}