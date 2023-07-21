namespace AirportReportApi.Core.Models;

public record AirportDto
{
    public string? Identifier { get; init; }
    public string? Name { get; init; }
    public string? TemperatureInF { get; init; }
    public string? RelativeHumidity { get; init; }
    public string? CloudCoverage { get; init; }
    public string? VisibilitySm { get; init; }
    public string? WindSpeedMph { get; init; }
    public string? WindDirection { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public List<RunwayModel>? Runways { get; init; }
    
}