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

    public Task<string> GetAirportReportById(string id)
    {
        return _airportRepository.GetAirportDetailsById(id);
    }
    
    private Task<string> GetAirportWeatherById(string id)
    {
        return _airportRepository.GetAirportWeatherById(id);
    }
}