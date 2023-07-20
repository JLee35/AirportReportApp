using AirportReportApi.Core.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirportReportApi.Core.Data;

public class AirportRepository : IAirportRepository
{
    private readonly HttpClient _httpClient;
    
    public AirportRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    // public AirportDetails GetAirportDetailsById(string id)
    // {
    //     return new AirportDetails
    //     {
    //         Identifier = "KJFK",
    //         Name = "John F. Kennedy International Airport",
    //         AvailableRunways = "4",
    //         LatLong = "40.6413° N, 73.7781° W"
    //     };
    // }

    public async Task<string> GetAirportDetailsById(string id)
    {
        return null;
    }

    public async Task<string> GetAirportWeatherById(string id)
    {
        try
        {
            string requestUrl = _httpClient.BaseAddress + id;
            
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

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