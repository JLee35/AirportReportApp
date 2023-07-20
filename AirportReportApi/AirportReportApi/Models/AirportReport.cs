namespace AirportReportApi.Core.Models;

public struct AirportReport
{
    public string Identifier { get; init; }
    public string Name { get; init; }
    public string AvailableRunways { get; init; }
    public string LatLong { get; init; }
    
    // More to come...
}