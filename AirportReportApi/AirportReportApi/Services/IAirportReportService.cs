using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Services;

public interface IAirportReportService
{
    public Task<string> GetAirportReportById(string id);
}