namespace AirportReportApi.Core.Configurations;

public class AirportConfig
{
    public string? BaseUrl { get; set; }
    public Dictionary<string, string>[]? Headers { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}