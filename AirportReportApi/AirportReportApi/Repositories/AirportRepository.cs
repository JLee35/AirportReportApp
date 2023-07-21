using AirportReportApi.Core.Data;
using AirportReportApi.Core.Services;

namespace AirportReportApi.Core.Repositories;

public class AirportRepository : IAirportRepository
{
    private readonly IHttpClientService _httpClientService;
    public AirportRepository(IHttpClientService clientService)
    {
        _httpClientService = clientService;
    }

    public async Task<string> GetAirportDetailsById(string id)
    {
        try
        {
            HttpClient detailsClient = _httpClientService.GetDetailsClient();
            string requestUrl = detailsClient.BaseAddress + id;

            HttpResponseMessage response = await detailsClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return await Task.FromResult("Problem!");
        }
        catch (HttpRequestException ex)
        {
            // TODO: Log exception.
            Console.WriteLine(ex);
            return await Task.FromResult("Problem!");
        }
    }

    public async Task<string> GetAirportWeatherById(string id)
    {
        try
        {
            HttpClient weatherClient = _httpClientService.GetWeatherClient();
            string requestUrl = weatherClient.BaseAddress + id;
            
            HttpResponseMessage response = await weatherClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return await Task.FromResult("Problem!");
        } 
        catch (HttpRequestException ex)
        {
            // TODO: Log exception.
            Console.WriteLine(ex);
            return await Task.FromResult("Problem!");
        }
    }
}