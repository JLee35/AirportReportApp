using AirportReportApi.Core.Data;

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
        return await _airportRepository.GetAirportWeatherById(id);
    }
    
    // private async Task<string> GetAirportWeatherById(string id)
    // {
    //     return await _airportRepository.GetAirportWeatherById(id);
    // }
}