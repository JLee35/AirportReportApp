namespace AirportReportApi.Core.Models;

public record CloudModel
{
    public CloudCoverage Coverage { get; init; }
    public decimal AltitudeFeet { get; init; }
    public bool IsCeiling { get; init; }
    
    public enum CloudCoverage
    {
        Clear = -1,
        Few = 0,
        Scattered = 1,
        Broken = 2,
        Overcast = 3
    }
    
    public static readonly Dictionary<string, CloudCoverage> CoverageMap = new()
    {
        { "clr", CloudCoverage.Clear},
        { "few", CloudCoverage.Few },
        { "sct", CloudCoverage.Scattered },
        { "bkn", CloudCoverage.Broken },
        { "ovc", CloudCoverage.Overcast }
    };
}