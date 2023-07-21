namespace AirportReportApi.Core.Models;

public record AirportWeatherModel
{
    public decimal TemperatureF { get; init; }
    public int RelativeHumidityPercentage { get; init; }
    public string? CloudCoverage { get; init; }
    public decimal VisibilitySm { get; init; }
    public decimal WindSpeedKts { get; init; }
    public int WindDirectionDegrees { get; init; }
    public string? TimeOffset { get; init; }
    
    // According to the specs, we should have at least two forecasts
    // coming from the API. But I found some airports may only have
    // one (KSFF for example).
    public List<WeatherForecastModel>? WeatherForecasts { get; set; }
}