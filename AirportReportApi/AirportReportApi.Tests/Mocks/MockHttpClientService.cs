using AirportReportApi.Core.Services;

namespace ApiReportApi.Tests.Mocks;

public class MockHttpClientService : IHttpClientService
{
    private readonly MockHttpClient _client;
    public MockHttpClientService(HttpResponseMessage response)
    {
        _client = new MockHttpClient(response);
        _client.BaseAddress = new Uri("localhost:5000");
    }
    public HttpClient GetDetailsClient()
    {
        return _client;
    }
    
    public HttpClient GetWeatherClient()
    {
        return _client;
    }
}