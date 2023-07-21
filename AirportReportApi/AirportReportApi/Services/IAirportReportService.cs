using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Services;

public interface IAirportReportService
{
    public Task<AirportWeatherModel> GetAirportReportById(string id);
}