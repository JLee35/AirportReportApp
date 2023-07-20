using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Data;

public class AirportReportRepository : IAirportReportRepository
{
    public AirportReport GetAirportReportById(string id)
    {
        return new AirportReport
        {
            Identifier = "KJFK",
            Name = "John F. Kennedy International Airport",
            AvailableRunways = "4",
            LatLong = "40.6413° N, 73.7781° W"
        };
    }
}