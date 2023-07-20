using AirportReportApi.Core.Data;
using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Services;

public class AirportReportService : IAirportReportService
{
    private IAirportReportRepository _airportReportRepository;
    
    public AirportReportService(IAirportReportRepository airportReportRepository)
    {
        _airportReportRepository = airportReportRepository;
    }

    public AirportReport GetAirportReportById(string id)
    {
        return _airportReportRepository.GetAirportReportById(id);
    }
}