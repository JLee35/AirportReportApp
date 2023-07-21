namespace AirportReportApi.Core.Models;

public class AirportDetailsModel
{
    public string? Identifier { get; init; }
    public string? Name { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public List<RunwayModel>? Runways { get; init; }

    internal void SetBestRunway(int windDirection)
    {
        Dictionary<string, int> crosswindComponents = new();

        if (Runways != null)
        {
            foreach (RunwayModel runway in Runways)
            {
                if (runway.MagneticHeading != null)
                {
                    int crosswindComponent = int.Abs((int)(runway.MagneticHeading - windDirection));
                    if (runway.Name != null) crosswindComponents.Add(runway.Name, crosswindComponent);
                }
            }

            string? bestRunway = crosswindComponents.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;

            foreach (RunwayModel runway in Runways)
            {
                if (runway.Name == bestRunway) runway.IsBestRunway = true;
            }
        }
    }
}