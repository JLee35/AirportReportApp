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

    public async Task<AirportWeatherModel> GetAirportReportById(string id)
    {
        // return await _airportRepository.GetAirportWeatherById(id);
        string weather = await _airportRepository.GetAirportWeatherById(id);
        JsonElement weatherRootElement = GetRootElement(weather);
        JsonElement parentElement = weatherRootElement.GetProperty("report").GetProperty("conditions");
        
        var airportWeatherModel = MapAirportWeather(parentElement);
        return airportWeatherModel;

        // string details = await _airportRepository.GetAirportDetailsById(id);
        // JsonElement detailsRootElement = GetRootElement(details);
        // var airportDetailsModel = MapAirportDetails(detailsRootElement);
        // return airportDetailsModel;
    }

    private JsonElement GetRootElement(string data)
    {
        JsonDocument jsonDocument = JsonDocument.Parse(data);
        JsonElement rootElement = jsonDocument.RootElement;

        return rootElement;
    }

    private async Task<string> GetAirportDetailsById(string id)
    {
        return await _airportRepository.GetAirportDetailsById(id);
    }
    
    private async Task<string> GetAirportWeatherById(string id)
    {
        return await _airportRepository.GetAirportWeatherById(id);
    }

    private AirportWeatherModel MapAirportWeather(JsonElement parentElement)
    {
        var tempC = parentElement.GetProperty("tempC").GetDecimal();
        var relativeHumidityPercent = parentElement.GetProperty("relativeHumidity").GetInt16();
        var visibilitySm = parentElement.GetProperty("visibility").GetProperty("distanceSm").GetDecimal();
        var windSpeedMph = parentElement.GetProperty("wind").GetProperty("speedKts").GetDecimal();
        var windDirectionMagnetic = parentElement.GetProperty("wind").GetProperty("direction").GetInt16();
        var cloudCoverage = GetCloudCoverage(parentElement);
        
        return new AirportWeatherModel
        {
            Temperature = tempC.ToString(CultureInfo.InvariantCulture),
            RelativeHumidity = relativeHumidityPercent.ToString(CultureInfo.InvariantCulture),
            CloudCoverage = cloudCoverage,
            Visibility = visibilitySm.ToString(CultureInfo.InvariantCulture),
            WindSpeed = windSpeedMph.ToString(CultureInfo.InvariantCulture),
            WindDirection = windDirectionMagnetic.ToString(CultureInfo.InvariantCulture)
        };
    }
    
    private AirportDetailsModel MapAirportDetails(JsonElement rootElement)
    {
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

    private static string GetCloudCoverage(JsonElement clouds)
    {
        // TODO: Get actual coverage from JsonElement.
        return "bad!";
    }

    private static List<RunwayModel> MapRunways(JsonElement runways)
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
            string? ident = element.GetProperty("ident").GetString();
            string? name = element.GetProperty("name").GetString();
            int magneticHeading = element.GetProperty("magneticHeading").GetInt16();
            string? recipName = element.GetProperty("recipName").GetString();
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