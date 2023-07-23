using System.Net;
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
    
    /// <summary>
    /// Given an airport id (ICAO), returns airport details such as name, location,
    /// runways, weather, etc.
    /// </summary>
    /// <param name="id">String</param>
    /// <returns>AirportDto</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAirportById(string id)
    {
        _logger.LogInformation("GetAirportById called with id: {Id}", id);
        AirportDto? dto;

        try
        {
            dto = await _service.GetAirportReportById(id);
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }
            else
            {
                _logger.LogInformation(e.Message);
                return StatusCode(500, e.Message);
            }
        }
        
        _logger.LogInformation("GetAirportById returning dto: {Dto}", dto);
        return Ok(dto);
    }
    
    /// <summary>
    /// Given a list of airport ids (ICAO), returns a list of airport details such as name,
    /// location, runways, weather, etc.
    /// </summary>
    /// <param name="ids">String List></param>
    /// <returns>AirportDto List</returns>
    [HttpPost("multiple")]
    public async Task<IActionResult> GetAirportsByIds([FromBody] List<string> ids)
    {
        _logger.LogInformation("GetAirportsByIds called with ids: {Ids}", ids);
        List<AirportDto>? airports;
        
        try
        {
            airports = await _service.GetAirportReportsByIds(ids);
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogInformation(e.Message);
                return NotFound(e.Message);
            }

            _logger.LogInformation(e.Message);
            return StatusCode(500, e.Message);
        }
        
        _logger.LogInformation("GetAirportsByIds returning airports: {Airports}", airports);
        return Ok(airports);
    }
}

