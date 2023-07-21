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
    public async Task<IActionResult> GetAirportById(string id)
    {
        _logger.LogInformation("GetAirportById called with id: {id}", id);
        AirportDto dto = await _service.GetAirportReportById(id);
        
        _logger.LogInformation("GetAirportById returning dto: {dto}", dto);
        return Ok(dto);
    }
}

