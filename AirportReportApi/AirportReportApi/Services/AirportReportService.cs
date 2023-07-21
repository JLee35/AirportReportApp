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

    public async Task<AirportDto> GetAirportReportById(string id)
    {
        AirportWeatherModel airportWeatherModel = await GetAirportWeatherById(id);
        AirportDetailsModel airportDetailsModel = await GetAirportDetailsById(id);
        
        return GetAirportDto(airportWeatherModel, airportDetailsModel);
    }

    private AirportDto GetAirportDto(AirportWeatherModel airportWeatherModel,
        AirportDetailsModel airportDetailsModel)
    {
        AirportDto airportDto = _mapper.Map<AirportDto>(airportDetailsModel);
        airportDto = _mapper.Map(airportWeatherModel, airportDto);

        return airportDto;
    }

    private static JsonElement GetRootElement(string data)
    {
        var jsonDocument = JsonDocument.Parse(data);
        JsonElement rootElement = jsonDocument.RootElement;

        return rootElement;
    }

    private async Task<AirportDetailsModel> GetAirportDetailsById(string id)
    {
        string details = await _airportRepository.GetAirportDetailsById(id);
        JsonElement detailsRootElement = GetRootElement(details);
        return MapAirportDetails(detailsRootElement);
    }
    
    private async Task<AirportWeatherModel> GetAirportWeatherById(string id)
    {
        string weather = await _airportRepository.GetAirportWeatherById(id);
        JsonElement weatherRootElement = GetRootElement(weather);
        JsonElement parentElement = weatherRootElement.GetProperty("report").GetProperty("conditions");
        
        return MapAirportWeather(parentElement);
    }

    private AirportWeatherModel MapAirportWeather(JsonElement parentElement)
    {
        var tempC = parentElement.GetProperty("tempC").GetDecimal();
        var relativeHumidityPercent = parentElement.GetProperty("relativeHumidity").GetInt16();
        var visibilitySm = parentElement.GetProperty("visibility").GetProperty("distanceSm").GetDecimal();
        var windSpeedMph = parentElement.GetProperty("wind").GetProperty("speedKts").GetDecimal();
        var windDirectionMagnetic = parentElement.GetProperty("wind").GetProperty("direction").GetInt16();
        var cloudCoverage = GetCloudCoverage(parentElement.GetProperty("cloudLayers"));
        
        return new AirportWeatherModel
        {
            TemperatureF = tempC,
            RelativeHumidityPercentage = relativeHumidityPercent,
            CloudCoverage = cloudCoverage,
            VisibilitySm = visibilitySm,
            WindSpeedMph = windSpeedMph,
            WindDirectionDegrees = windDirectionMagnetic
        };
    }
    
    private AirportDetailsModel MapAirportDetails(JsonElement rootElement)
    {
        var icao = rootElement.GetProperty("icao").GetString();
        var name = rootElement.GetProperty("name").GetString();
        
        // Not sure if culture invariance is necessary, but may help
        // in situations where the API returns a decimal with a comma.
        var latitude = rootElement.GetProperty("latitude").GetDecimal().ToString(CultureInfo.InvariantCulture);
        var longitude = rootElement.GetProperty("longitude").GetDecimal().ToString(CultureInfo.InvariantCulture);

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
        List<CloudModel> cloudModels = new();
        var elements = GetChildElements(clouds);
        foreach (var element in elements)
        {
            var coverage = element.GetProperty("coverage").GetString();
            var altitudeFt = element.GetProperty("altitudeFt").GetDecimal();
            var isCeiling = element.GetProperty("ceiling").GetBoolean();
            
            cloudModels.Add(new CloudModel
            {
                Coverage = CloudModel.CoverageMap[coverage],
                AltitudeFeet = altitudeFt,
                IsCeiling = isCeiling
            });
        }
        
        CloudCoverageModel cloudCoverageModel = new CloudCoverageModel
        {
            Clouds = cloudModels
        };

        return cloudCoverageModel.GetCoverageSummary();
    }

    private static List<RunwayModel> MapRunways(JsonElement runways)
    {
        // Foreach runway in runways, map to RunwayModel.
        // Return list of RunwayModel.
        var elements = GetChildElements(runways);

        return (from element in elements
            let ident = element.GetProperty("ident").GetString()
            let name = element.GetProperty("name").GetString()
            let magneticHeading = element.GetProperty("magneticHeading").GetInt16()
            let recipName = element.GetProperty("recipName").GetString()
            let recipMagneticHeading = element.GetProperty("recipMagneticHeading").GetInt16()
            select new RunwayModel
            {
                Identifier = ident,
                Name = name,
                MagneticHeading = magneticHeading,
                ReciprocalName = recipName,
                ReciprocalMagneticHeading = recipMagneticHeading
            }).ToList();
    }

    private static JsonElement[] GetChildElements(JsonElement parent)
    {
        if (parent.ValueKind != JsonValueKind.Array)
        {
            return Array.Empty<JsonElement>();
        }

        var children = new JsonElement[parent.GetArrayLength()];
        var index = 0;

        foreach (var element in parent.EnumerateArray())
        {
            children[index] = element;
            index++;
        }

        return children;
    }
}