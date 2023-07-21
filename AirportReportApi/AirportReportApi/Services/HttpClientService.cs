using System.Net.Http.Headers;
using System.Text;
using AirportReportApi.Core.Configurations;

namespace AirportReportApi.Core.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _weatherClient;
    private readonly HttpClient _detailsClient;
    
    /// <summary>
    /// Given an AirportWeatherConfig and AirportDetailsConfig, this service will return a configured HttpClient
    /// with the appropriate headers, base address, and authentication.
    /// </summary>
    /// <param name="weatherConfig">AirportWeatherConfig</param>
    /// <param name="detailsConfig">AirportDetailsConfig</param>
    public HttpClientService(AirportWeatherConfig weatherConfig, AirportDetailsConfig detailsConfig)
    {
        _weatherClient = GetConfiguredClient(weatherConfig);
        _detailsClient = GetConfiguredClient(detailsConfig);
    }
    
    /// <summary>
    /// Returns the HttpClient configured for the airport details API.
    /// </summary>
    /// <returns>HttpClient</returns>
    public HttpClient GetDetailsClient()
    {
        return _detailsClient;
    }
    
    /// <summary>
    /// Returns the HttpClient configured for the airport weather API.
    /// </summary>
    /// <returns>HttpClient</returns>
    public HttpClient GetWeatherClient()
    {
        return _weatherClient;
    }

    private static HttpClient GetConfiguredClient(AirportConfig config)
    {
        HttpClient client = new();
        
        if (config.BaseUrl is not null)
        {
            client.BaseAddress = new Uri(config.BaseUrl);
        }
        
        if (config.Headers is not null)
        {
            foreach (var headerField in config.Headers)
            {
                // Each header field should only have one key-value pair.
                client.DefaultRequestHeaders.Add(headerField.Keys.First(), headerField.Values.First());
            }
        }

        if (config.UserName is null || config.Password is null) return client;
        
        var authHeaderValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.UserName}:{config.Password}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

        return client;
    }
}