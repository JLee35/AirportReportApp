namespace AirportReportApi.Core.Models;

public record WindForecastModel
{
    public string? TimeOffset { get; set; }
    public decimal WindSpeedKts { get; init; }
    public bool IsWindVariable { get; init; }
    public int WindDirectionDegrees { get; set; }
}