using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Services;

public interface IAirportReportService
{
    public Task<AirportDto> GetAirportReportById(string id);
    public Task<List<AirportDto>> GetAirportReportsByIds(List<string> ids);
}