using AirportReportApi.Core.Models;
using AirportReportApi.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirportReportApi.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class AirportController : ControllerBase
{
    
    private readonly IAirportReportService _service;
    private readonly ILogger<AirportController> _logger;
    
    public AirportController(IAirportReportService service, ILogger<AirportController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public IActionResult GetAirportById(string id)
    {
        Task<string> details = _service.GetAirportReportById(id);
        
        if (details is null)
        {
            _logger.LogInformation("Airport details with id {Id} not found.", id);
            return NotFound();
        }
        
        return Ok(details);
    }
}

