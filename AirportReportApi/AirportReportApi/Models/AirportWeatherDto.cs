namespace AirportReportApi.Core.Models;

public class AirportWeatherDto
{
    public decimal TemperatureInF { get; set; }
    public decimal RelativeHumidity { get; set; }
    public string? CloudCoverage { get; init; }
    public decimal VisibilitySm { get; set; }
    public decimal WindSpeedMph { get; set; }
    public string? WindDirection { get; set; }
    
    public List<WindForecastModel>? WindForecasts { get; set; }
}