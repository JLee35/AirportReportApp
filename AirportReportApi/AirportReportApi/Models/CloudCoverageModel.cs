namespace AirportReportApi.Core.Models;

public class CloudCoverageModel
{
    internal List<CloudModel> Clouds { get; init; } = new();

    public string GetCoverageSummary()
    {
        if (Clouds.Count == 0)
        {
            // This shouldn't happen, because the API should
            // return 'clr' at 0 ft if there are no clouds.
            // Leaving this here just in case.
            return "Clear";
        }
        
        var lowestCloud = FindLowestAltitudeWithGreatestCoverage();
        return GetCoverageSummaryString(lowestCloud);
    }
    
    private string GetCoverageSummaryString(CloudModel cloud)
    {
        var coverage = cloud.Coverage.ToString().ToLower();

        var summary = $"Skies {coverage}";
        var altitude = (int)cloud.AltitudeFeet;
        if (altitude > 0)
        {
            summary += $" at {altitude} feet";
        }

        return summary;
    }

    private CloudModel FindLowestAltitudeWithGreatestCoverage()
    {   
        var sortedCloudModels = Clouds
            .OrderBy(cm => cm.IsCeiling)
            .ThenBy(cm => cm.Coverage)
            .ThenBy(cm => cm.AltitudeFeet);
        
        return sortedCloudModels.First();
    }
}