namespace AirportReportApi.Core.Models;

public record CloudModel
{
    public string Coverage { get; init; }
    public decimal AltitudeFeet { get; init; }
    public bool IsCeiling { get; init; }
}