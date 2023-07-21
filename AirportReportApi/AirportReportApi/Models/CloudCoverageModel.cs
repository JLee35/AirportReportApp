namespace AirportReportApi.Core.Models;

public class CloudCoverageModel
{
    internal List<CloudModel> Clouds { get; init; } = new();

    private enum CloudCoverage
    {
        Few = 0,
        Scattered = 1,
        Broken = 2,
        Overcast = 3
    }
    
    private readonly Dictionary<string, CloudCoverage> _coverageMap = new()
    {
        { "few", CloudCoverage.Few },
        { "sct", CloudCoverage.Scattered },
        { "bkn", CloudCoverage.Broken },
        { "ovc", CloudCoverage.Overcast }
    };

    public string GetCoverageSummary()
    {
        if (Clouds.Count == 0)
        {
            return "Clear";
        }
        
        var lowestCloud = FindLowestAltitudeWithGreatestCoverage();
        return GetCoverageSummaryString(lowestCloud);
    }
    
    private string GetCoverageSummaryString(CloudModel cloud)
    {
        var coverage = _coverageMap[cloud.Coverage].ToString();
        var altitude = (int)cloud.AltitudeFeet;
        
        return $"{coverage} at {altitude} feet";
    }

    private CloudModel FindLowestAltitudeWithGreatestCoverage()
    {   
        var sortedCloudModels = Clouds
            .OrderBy(cm => cm.IsCeiling)
            .ThenBy(cm => _coverageMap[cm.Coverage])
            .ThenBy(cm => cm.AltitudeFeet);
        
        return sortedCloudModels.First();
    }
}