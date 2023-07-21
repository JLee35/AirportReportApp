namespace AirportReportApi.Core.Models;

public class CloudCoverageModel
{
    public string? Coverage { get; init; }
    public decimal? AltitudeFeet { get; init; }
    public bool? IsCeiling { get; init; }
}