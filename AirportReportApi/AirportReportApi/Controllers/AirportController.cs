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
        _logger.LogInformation("GetAirportById called with id: {Id}", id);
        AirportDto? dto = await _service.GetAirportReportById(id);
        
        if (dto == null)
        {
            var message = $"Airport with id {id} not found";
            _logger.LogInformation(message);
            return NotFound(message);
        }
        
        _logger.LogInformation("GetAirportById returning dto: {Dto}", dto);
        return Ok(dto);
    }

    [HttpPost("multiple")]
    public async Task<IActionResult> GetAirportsByIds([FromBody] List<string> ids)
    {
        _logger.LogInformation("GetAirportsByIds called with ids: {Ids}", ids);

        List<AirportDto> airports = await _service.GetAirportReportsByIds(ids);

        if (airports.Count == 0)
        {
            return NotFound("No airports found");
        }
        
        _logger.LogInformation("GetAirportsByIds returning airports: {Airports}", airports);
        return Ok(airports);
    }
}

