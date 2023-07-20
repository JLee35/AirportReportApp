using AirportReportApi.Core.Data;
using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Services;

public class AirportReportService : IAirportReportService
{
    private readonly IAirportRepository _airportRepository;
    
    public AirportReportService(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }

    public async Task<string> GetAirportReportById(string id)
    {
        return await _airportRepository.GetAirportDetailsById(id);
    }
    
    private async Task<string> GetAirportWeatherById(string id)
    {
        return await _airportRepository.GetAirportWeatherById(id);
    }
}