namespace AirportReportApi.Core.Models;

public record RunwayModel()
{
    public string? Identifier { get; init; }
    public string? Name { get; init; }
    public int? MagneticHeading { get; init; }
    public string? ReciprocalName { get; init; }
    public int? ReciprocalMagneticHeading { get; init; }
}