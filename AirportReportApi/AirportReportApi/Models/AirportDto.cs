namespace AirportReportApi.Core.Models;

public record AirportDto
{
    public string? Identifier { get; set; }
    public string? Name { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public AirportWeatherDto? Weather { get; set; }
    public List<RunwayModel>? Runways { get; init; }
    
}