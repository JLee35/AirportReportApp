using AirportReportApi.Core.Enums;

namespace AirportReportApi.Core.Models;

public record CloudModel
{
    public CloudCoverage Coverage { get; init; }
    public decimal AltitudeFeet { get; init; }
    public bool IsCeiling { get; init; }

    public static readonly Dictionary<string, CloudCoverage> CoverageMap = new()
    {
        { "clr", CloudCoverage.Clear},
        { "few", CloudCoverage.Few },
        { "sct", CloudCoverage.Scattered },
        { "bkn", CloudCoverage.Broken },
        { "ovc", CloudCoverage.Overcast }
    };
}