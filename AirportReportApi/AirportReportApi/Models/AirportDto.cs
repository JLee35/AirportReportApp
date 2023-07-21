namespace AirportReportApi.Core.Models;

public record AirportDto
{
    public string? Identifier { get; init; }
    public string? Name { get; init; }
    
}