using System.Net;
using AirportReportApi.Core.Enums;
using AirportReportApi.Core.Services;

namespace AirportReportApi.Core.Repositories;

public class AirportRepository : IAirportRepository
{
    private readonly IHttpClientService _httpClientService;
    public AirportRepository(IHttpClientService clientService)
    {
        _httpClientService = clientService;
    }

    public async Task<string> GetAirportInformationById(string id, ReportType reportType)
    {
        HttpClient client = 
            reportType == ReportType.Details ? _httpClientService.GetDetailsClient() : _httpClientService.GetWeatherClient();
        
        string requestUrl = client.BaseAddress + id;

        HttpResponseMessage response = await client.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                throw new HttpRequestException($"Airport with identifier {id} could not be found.", null, HttpStatusCode.NotFound);
            case HttpStatusCode.BadRequest:
                throw new BadHttpRequestException("Bad request.");
            case HttpStatusCode.Unauthorized:
                throw new HttpRequestException("Access denied.", null, HttpStatusCode.Unauthorized);
        }
        
        throw new HttpRequestException("A problem occurred while retrieving airport information.", null, HttpStatusCode.InternalServerError);
    }
}