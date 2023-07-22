namespace AirportReportApi.Core.Services;

public interface IHttpClientService
{
    public HttpClient GetDetailsClient();
    public HttpClient GetWeatherClient();
}