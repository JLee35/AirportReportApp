namespace AirportReportApi.Core.Models;

public record AirportDetailsModel
{
    public string? Identifier { get; init; }
    public string? Name { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public List<RunwayModel>? Runways { get; init; }
    
    // More to come...
}