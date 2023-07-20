using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Services;

public interface IAirportReportService
{
    public AirportReport GetAirportReportById(string id);
}