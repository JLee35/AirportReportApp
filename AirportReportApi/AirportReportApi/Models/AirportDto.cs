namespace AirportReportApi.Core.Models;

public record AirportDto
{
    public string? Identifier { get; set; }
    public string? Name { get; init; }
    public decimal TemperatureInF { get; set; }
    public decimal RelativeHumidity { get; set; }
    public string? CloudCoverage { get; init; }
    public decimal VisibilitySm { get; set; }
    public decimal WindSpeedMph { get; set; }
    public string? WindDirection { get; set; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public WeatherForecastModel? WeatherForecast { get; set; }
    public List<RunwayModel>? Runways { get; init; }
    
}