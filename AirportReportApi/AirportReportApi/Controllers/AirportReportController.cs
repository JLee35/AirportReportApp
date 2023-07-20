using AirportReportApi.Core.Models;
using AirportReportApi.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirportReportApi.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class AirportReportController : ControllerBase
{
    
    private readonly IAirportReportService _service;
    private readonly ILogger<AirportReportController> _logger;
    
    public AirportReportController(IAirportReportService service, ILogger<AirportReportController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public IActionResult GetAirportReportById(string id)
    {
        AirportReport report = _service.GetAirportReportById(id);
        
        if (report is null)
        {
            _logger.LogInformation($"Airport report with id {id} not found.");
            return NotFound();
        }
        
        return Ok(report);
    }
}

