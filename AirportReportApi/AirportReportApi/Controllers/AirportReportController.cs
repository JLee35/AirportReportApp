using AirportReportApi.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirportReportApi.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class AirportReportController : ControllerBase
{
    
    private static readonly AirportReport AirportReport = new()
    {
        Identifier = "KJFK",
        Name = "John F. Kennedy International Airport",
        AvailableRunways = "4L, 4R, 13L, 13R, 22L, 22R, 31L, 31R",
        LatLong = "40.63980103, -73.77890015"
    };

    private readonly ILogger<AirportReportController> _logger;

    public AirportReportController(ILogger<AirportReportController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAirportReport")]
    public AirportReport Get()
    {
        return AirportReport;
    }
}

