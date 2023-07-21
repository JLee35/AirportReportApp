using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Services;

public interface IAirportReportService
{
    public Task<AirportDetailsModel> GetAirportReportById(string id);
}