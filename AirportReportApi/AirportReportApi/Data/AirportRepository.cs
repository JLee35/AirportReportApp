using AirportReportApi.Core.Services;

namespace AirportReportApi.Core.Data;

public class AirportRepository : IAirportRepository
{
    private readonly IHttpClientService _httpClientService;
    public AirportRepository(IHttpClientService clientService)
    {
        _httpClientService = clientService;
    }

    public async Task<string> GetAirportDetailsById(string id)
    {
        return null;
    }

    public async Task<string> GetAirportWeatherById(string id)
    {
        Console.WriteLine("In repository");
        try
        {
            HttpClient weatherClient = _httpClientService.GetWeatherClient();
            string requestUrl = weatherClient.BaseAddress + id;
            
            HttpResponseMessage response = await weatherClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        } 
        catch (HttpRequestException ex)
        {
            // TODO: Log exception.
            Console.WriteLine(ex);
            return null;
        }
    }
}