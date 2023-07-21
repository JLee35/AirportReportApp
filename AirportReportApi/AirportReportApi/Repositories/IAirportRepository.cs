using AirportReportApi.Core.Enums;

namespace AirportReportApi.Core.Repositories;

public interface IAirportRepository
{
    public Task<string> GetAirportInformationById(string id, ReportType reportType);
}