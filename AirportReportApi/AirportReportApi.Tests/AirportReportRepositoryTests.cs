using AirportReportApi.Core.Data;
using AirportReportApi.Core.Models;

namespace ApiReportApi.Tests;

public class AirportReportRepositoryTests
{

    private static readonly HttpClient HttpClient = new ();
    private static readonly AirportRepository Repository = new (HttpClient);
        
    [Fact]
    public void GetAirportReportById_ExistingId_ReturnsAirportData()
    {
        // Arrange

        // Act
        // AirportDetails details = Repository.GetAirportDetailsById("KJFK");
        //
        // // Assert
        // Assert.Equal("KJFK", details.Identifier);
        // Assert.Equal("John F. Kennedy International Airport", details.Name);
        // Assert.Equal("4", details.AvailableRunways);
        // Assert.Equal("40.6413° N, 73.7781° W", details.LatLong);
    }

    [Fact]
    public void GetAirportWeatherReportById_ExistingId_ReturnsAirportWeatherData()
    {
        // Arrange
        
        // Act
        string weatherReport = Repository.GetAirportWeatherById("KJFK").Result;
        
        // Assert
        Assert.NotNull(weatherReport);
    }
}
