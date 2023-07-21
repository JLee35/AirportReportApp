using System.Globalization;
using AirportReportApi.Core.Data;
using AutoMapper;
using System.Text.Json;
using AirportReportApi.Core.Models;

namespace AirportReportApi.Core.Services;

public class AirportReportService : IAirportReportService
{
    private readonly IAirportRepository _airportRepository;
    private readonly IMapper _mapper;
    
    public AirportReportService(IAirportRepository airportRepository, IMapper mapper)
    {
        _airportRepository = airportRepository;
        _mapper = mapper;
    }

    public async Task<AirportDetailsModel> GetAirportReportById(string id)
    {
        // return await _airportRepository.GetAirportWeatherById(id);
        
        string data = await _airportRepository.GetAirportDetailsById(id);
        
        var airportDetailsModel = MapAirportDetails(data);

        return airportDetailsModel;
    }

    private async Task<string> GetAirportDetailsById(string id)
    {
        return await _airportRepository.GetAirportDetailsById(id);
    }
    
    private async Task<string> GetAirportWeatherById(string id)
    {
        return await _airportRepository.GetAirportWeatherById(id);
    }
    
    private AirportDetailsModel MapAirportDetails(string data)
    {
        JsonDocument jsonDocument = JsonDocument.Parse(data);
        JsonElement rootElement = jsonDocument.RootElement;

        string? icao = rootElement.GetProperty("icao").GetString();
        string? name = rootElement.GetProperty("name").GetString();
        
        // Not sure if culture invariance is necessary, but may help
        // in situations where the API returns a decimal with a comma.
        string? latitude = rootElement.GetProperty("latitude").GetDecimal().ToString(CultureInfo.InvariantCulture);
        string? longitude = rootElement.GetProperty("longitude").GetDecimal().ToString(CultureInfo.InvariantCulture);

        JsonElement runways = rootElement.GetProperty("runways");
        
        List<RunwayModel> runwayModels = MapRunways(runways);
        
        return new AirportDetailsModel
        {
            Identifier = icao,
            Name = name,
            Latitude = latitude,
            Longitude = longitude,
            Runways = runwayModels
        };
    }

    private List<RunwayModel> MapRunways(JsonElement runways)
    {
        // Foreach runway in runways, map to RunwayModel.
        // Return list of RunwayModel.
        
        List<RunwayModel> runwayModels = new List<RunwayModel>();
        
        if (runways.ValueKind != JsonValueKind.Array)
        {
            // This is a problem, because all airports
            // should have at least one runway.
            // Will leave since this won't go into production
            // and can assume that the API will always return
            // a list of runways.
            return new List<RunwayModel>();
        }

        var elements = new JsonElement[runways.GetArrayLength()];
        int index = 0;

        foreach (JsonElement element in runways.EnumerateArray())
        {
            elements[index] = element;
            index++;
        }
        
        foreach (var element in elements)
        {
            string ident = element.GetProperty("ident").GetString();
            string name = element.GetProperty("name").GetString();
            int magneticHeading = element.GetProperty("magneticHeading").GetInt16();
            string recipName = element.GetProperty("recipName").GetString();
            int recipMagneticHeading = element.GetProperty("recipMagneticHeading").GetInt16();
            
            runwayModels.Add(new RunwayModel
            {
                Identifier = ident,
                Name = name,
                MagneticHeading = magneticHeading,
                ReciprocalName = recipName,
                ReciprocalMagneticHeading = recipMagneticHeading
            });
        }

        return runwayModels;
    }
}