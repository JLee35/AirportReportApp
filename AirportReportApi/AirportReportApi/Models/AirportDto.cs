namespace AirportReportApi.Core.Models;

public record AirportDto
{
    public string? Identifier { get; init; }
    public string? Name { get; init; }
    public string? Temperature { get; init; }
    public string? RelativeHumidity { get; init; }
    public string? CloudCoverage { get; init; }
    public string? Visibility { get; init; }
    public string? WindSpeed { get; init; }
    public string? WindDirection { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public List<RunwayModel>? Runways { get; init; }
    
}