using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Data;

public interface IAirportRepository
{
    public Task<string> GetAirportDetailsById(string id);
    public Task<string> GetAirportWeatherById(string id);
}