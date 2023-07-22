
using System.Net;
using AirportReportApi.Core.Enums;
using AirportReportApi.Core.Repositories;
using AirportReportApi.Core.Services;
using ApiReportApi.Tests.Mocks;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ApiReportApi.Tests;

public class AirportReportRepositoryTests
{
    
    [Fact]
    public async Task GetAirportInformationById_ShouldReturnAirportDetails_WhenReportTypeIsDetails()
    {
        // Arrange
        string validId = "KJFK";
        ReportType reportType = ReportType.Details;

        HttpResponseMessage successResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("Mocked details json")
        };
        
        MockHttpClientService mockClientService = new MockHttpClientService(successResponse);
        AirportRepository airportRepository = new AirportRepository(mockClientService);
        
        // Act
        string result = await airportRepository.GetAirportInformationById(validId, reportType);
        
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAirportInformationById_ShouldReturnAirportWeather_WhenReportTypeIsValid()
    {
        // Arrange
        string validId = "KJFK";
        ReportType reportType = ReportType.Weather;

        HttpResponseMessage successResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("Mocked details json")
        };
        
        MockHttpClientService mockClientService = new MockHttpClientService(successResponse);
        AirportRepository airportRepository = new AirportRepository(mockClientService);
        
        // Act
        string result = await airportRepository.GetAirportInformationById(validId, reportType);
        
        // Assert
        Assert.NotNull(result);
    }
}
