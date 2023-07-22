namespace AirportReportApi.Core.Models;

public record RunwayModel()
{
    public string? Name { get; init; }
    public int? MagneticHeading { get; init; }
    public bool IsBestRunway { get; set; }
}