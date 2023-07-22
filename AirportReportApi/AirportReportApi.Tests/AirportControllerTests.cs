using System.Runtime.InteropServices.ComTypes;
using AirportReportApi.Core.Controllers;
using AirportReportApi.Core.Models;
using AirportReportApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiReportApi.Tests;

public class AirportControllerTests
{
    [Fact]
    public async Task GetAirportById_ReturnsOk_WhenAirportFound()
    {
        // Arrange
        var airportId = "KJFK";
        var airportDto = new AirportDto
        {
            Identifier = "KJFK",
            Name = "John F. Kennedy International Airport",
            // leaving the other fields null for brevity
        };
        
        var mockService = new Mock<IAirportReportService>();
        mockService.Setup(s => s.GetAirportReportById(airportId)).ReturnsAsync(airportDto);
        
        var mockLogger = new Mock<ILogger<AirportController>>();
        var controller = new AirportController(mockService.Object, mockLogger.Object);
        
        // Act
        var result = await controller.GetAirportById(airportId);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(airportDto, ((OkObjectResult) result).Value);
    }

    [Fact]
    public async Task GetAirportById_ReturnsNotFound_WhenAirportNotFound()
    {
        // Arrange
        var airportId = "NonExistentId";
        
        var mockService = new Mock<IAirportReportService>();
        mockService.Setup(s => s.GetAirportReportById(airportId))!.ReturnsAsync((AirportDto) null!);
        
        var mockLogger = new Mock<ILogger<AirportController>>();
        var controller = new AirportController(mockService.Object, mockLogger.Object);
        
        // Act
        var result = await controller.GetAirportById(airportId);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAirportsByIds_ReturnsAirports_WhenAirportsExist()
    {
        // Arrange
        var airportIds = new List<string> {"KGEG", "KSFO", "KSEA"};
        var airportDtos = new List<AirportDto>
        {
            new()
            {
                Identifier = "KGEG",
                Name = "Spokane International Airport",
                // leaving the other fields null for brevity
            },
            new()
            {
                Identifier = "KSFO",
                Name = "San Francisco International Airport",
            },
            new()
            {
                Identifier = "KSEA",
                Name = "Seattle-Tacoma International Airport",
            }
        };
        
        var mockService = new Mock<IAirportReportService>();
        mockService.Setup(s => s.GetAirportReportsByIds(airportIds)).ReturnsAsync(airportDtos);
        
        var mockLogger = new Mock<ILogger<AirportController>>();
        var controller = new AirportController(mockService.Object, mockLogger.Object);
        
        // Act
        var result = await controller.GetAirportsByIds(airportIds);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(airportDtos, ((OkObjectResult) result).Value);
    }
    
    [Fact]
    public async Task GetAirportsByIds_ReturnsNotFound_WhenNoAirportsExist()
    {
        // Arrange
        var airportIds = new List<string> {"NonExistentId1", "NonExistentId2"};
        
        var mockService = new Mock<IAirportReportService>();
        mockService.Setup(s => s.GetAirportReportsByIds(airportIds)).ReturnsAsync(new List<AirportDto>());
        
        var mockLogger = new Mock<ILogger<AirportController>>();
        var controller = new AirportController(mockService.Object, mockLogger.Object);
        
        // Act
        var result = await controller.GetAirportsByIds(airportIds);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}