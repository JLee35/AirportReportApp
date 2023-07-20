using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Data;

public interface IAirportReportRepository
{
    public AirportReport GetAirportReportById(string id);
}