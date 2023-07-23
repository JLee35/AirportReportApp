using System.Diagnostics;
using System.Globalization;
using AutoMapper;
using System.Text.Json;
using AirportReportApi.Core.Enums;
using AirportReportApi.Core.Models;
using AirportReportApi.Core.Repositories;

namespace AirportReportApi.Core.Services;

public class AirportReportService : IAirportReportService
{
    private readonly IAirportRepository _airportRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ApplicationLog> _logger;

    private delegate AirportWeatherModel WeatherCollectionDelegate(AirportWeatherModel weatherModel, JsonElement parentElement);
    
    public AirportReportService(IAirportRepository airportRepository, IMapper mapper, ILogger<ApplicationLog> logger)
    {
        _airportRepository = airportRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    /// <summary>
    /// Given an airport identifier (ICAO), return a DTO containing
    /// airport and weather information.
    /// </summary>
    /// <param name="id">string</param>
    /// <returns>AirportDto</returns>
    public async Task<AirportDto> GetAirportReportById(string id)
    {
        _logger.LogInformation("GetAirportReportById called with id: {Id}", id);
        
        AirportWeatherModel airportWeatherModel = await GetAirportWeatherById(id);
        AirportDetailsModel airportDetailsModel = await GetAirportDetailsById(id);
        
        // Set best runway for wind now that we have both pieces of information.
        airportDetailsModel.SetBestRunway(airportWeatherModel.WindDirectionDegrees);
        
        return GetAirportDto(airportWeatherModel, airportDetailsModel);
    }

    public async Task<List<AirportDto>> GetAirportReportsByIds(List<string> ids)
    {
        List<AirportDto> airports = new();

        foreach (string id in ids)
        {
            AirportDto airport = await GetAirportReportById(id);
            airports.Add(airport);
        }

        return airports;
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
        string details = await _airportRepository.GetAirportInformationById(id, ReportType.Details);
        JsonElement detailsRootElement = GetRootElement(details);
        return MapAirportDetails(detailsRootElement);
    }
    
    private async Task<AirportWeatherModel> GetAirportWeatherById(string id)
    {
        _logger.LogInformation("GetAirportWeatherById called with id: {Id}", id);
        
        AirportWeatherModel weatherModel = new();
        JsonElement conditionsElement, forecastConditionsElement;
        
        try
        {
            var weather = await _airportRepository.GetAirportInformationById(id, ReportType.Weather);
            JsonElement rootElement = GetRootElement(weather);
            JsonElement reportElement = rootElement.GetProperty("report");
            conditionsElement = reportElement.GetProperty("conditions");
            JsonElement forecastElement = reportElement.GetProperty("forecast");
            forecastConditionsElement = forecastElement.GetProperty("conditions");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError("Exception occurred while gathering weather: {ExMessage}", ex.Message);
            return weatherModel;
        }
        
        weatherModel = MapCurrentAirportWeather(conditionsElement);
        List<WindForecastModel> windForecasts = GetWindForecast(forecastConditionsElement);
        weatherModel.WindForecasts = windForecasts;
        
        _logger.LogInformation("GetAirportWeatherById returning weatherModel: {WeatherModel}", weatherModel);
        return weatherModel;
    }

    private AirportWeatherModel MapCurrentAirportWeather(JsonElement conditionsElement)
    {   
        var weatherModel = new AirportWeatherModel();
        weatherModel = AddTempVisibilityAndHumidity(conditionsElement, weatherModel);
        weatherModel = AddWind(conditionsElement, weatherModel);
        weatherModel = AddClouds(conditionsElement, weatherModel);
        
        return weatherModel;
    }

    private AirportWeatherModel AddTempVisibilityAndHumidity(JsonElement conditionsElement,
        AirportWeatherModel weatherModel)
    {
        weatherModel = GetWeatherByCategory((weather, conditions) =>
        {
            var tempC = conditions.GetProperty("tempC").GetDecimal();
            var relativeHumidityPercent = conditions.GetProperty("relativeHumidity").GetInt16();
            var visibilitySm = conditions.GetProperty("visibility").GetProperty("distanceSm").GetDecimal();

            weather.TemperatureF = tempC;
            weather.RelativeHumidityPercentage = relativeHumidityPercent;
            weather.VisibilitySm = visibilitySm;

            return weather;
        }, weatherModel, conditionsElement);

        return weatherModel;
    }

    private AirportWeatherModel AddWind(JsonElement conditionsElement, AirportWeatherModel weatherModel)
    {
        weatherModel = GetWeatherByCategory((weather, conditions) =>
        {
            JsonElement windElement = conditions.GetProperty("wind");
            var windSpeedMph = windElement.GetProperty("speedKts").GetDecimal();
            bool isWindVariable = windElement.GetProperty("variable").GetBoolean();
            
            weather.WindSpeedKts = windSpeedMph;
            weather.IsWindVariable = isWindVariable;

            if (!isWindVariable)
            {
                var windDirectionDegrees = windElement.GetProperty("direction").GetInt16();
                weather.WindDirectionDegrees = windDirectionDegrees;
            }
            
            return weather;
        }, weatherModel, conditionsElement);

        return weatherModel;
    }

    private AirportWeatherModel AddClouds(JsonElement conditionsElement,
        AirportWeatherModel weatherModel)
    {
        weatherModel = GetWeatherByCategory((weather, conditions) =>
        {
            var cloudCoverage = GetCloudCoverage(conditions.GetProperty("cloudLayers"));
            weather.CloudCoverage = cloudCoverage;

            return weather;
        }, weatherModel, conditionsElement);

        return weatherModel;
    }

    private AirportWeatherModel GetWeatherByCategory(
        WeatherCollectionDelegate collectionDelegate, 
        AirportWeatherModel weatherModel, JsonElement parentElement)
    {
        try
        {
            weatherModel = collectionDelegate(weatherModel, parentElement);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError("Exception occurred while gathering weather: {ExMessage}", ex.Message);
        }

        return weatherModel;
    }
    
    private List<WindForecastModel> GetWindForecast(JsonElement forecastConditionsElement)
    {
        List<WindForecastModel> windForecastModels = new();
        var conditions = GetChildElements(forecastConditionsElement);
        
        // According to specs, we only need the first two forecasts if available.
        for (int forecastCount = 0; forecastCount < 2; forecastCount++)
        {
            try
            {
                JsonElement windElement = conditions[forecastCount].GetProperty("wind");
                decimal windSpeedKts = windElement.GetProperty("speedKts").GetDecimal();
                bool isWindVariable = windElement.GetProperty("variable").GetBoolean();

                JsonElement periodElement = conditions[forecastCount].GetProperty("period");
                string forecastTimeOffset = GetForecastTimeOffset(periodElement);

                var windForecastModel = new WindForecastModel
                {
                    TimeOffset = forecastTimeOffset,
                    WindSpeedKts = windSpeedKts,
                    IsWindVariable = isWindVariable
                };

                if (!isWindVariable)
                {
                    short windDirectionDegrees = windElement.GetProperty("direction").GetInt16();
                    windForecastModel.WindDirectionDegrees = windDirectionDegrees;    
                }
                
                windForecastModels.Add(windForecastModel);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Exception raised: {ExMessage}", ex.Message);
            }
        }

        return windForecastModels;
    }

    private string GetForecastTimeOffset(JsonElement forecastPeriodElement)
    {
        var dateStart = forecastPeriodElement.GetProperty("dateStart").GetString();
        
        if (dateStart is null)
        {
            _logger.LogWarning("dateStart was found to be null in GetForecastTimeOffset");
            return string.Empty;
        }
        
        var start = DateTimeOffset.Parse(dateStart).UtcDateTime;
        var currentTime = DateTime.UtcNow;
        var difference = currentTime - start;
        
        // Format difference into hrs:min.
        string formattedDifference = difference.ToString("h\\h\\:mm\\m");
        return formattedDifference;
    }
    
    private AirportDetailsModel MapAirportDetails(JsonElement rootElement)
    {
        try
        {
            var icao = rootElement.GetProperty("icao").GetString();
            var name = rootElement.GetProperty("name").GetString();

            decimal latitude = rootElement.GetProperty("latitude").GetDecimal();
            decimal longitude = rootElement.GetProperty("longitude").GetDecimal();            
            
            LatLong latLong = GetNormalizedLatLong(latitude, longitude);
            
            JsonElement runways = rootElement.GetProperty("runways");
            List<RunwayModel> runwayModels = MapRunways(runways);

            return new AirportDetailsModel
            {
                Identifier = icao,
                Name = name,
                Latitude = latLong.Latitude,
                Longitude = latLong.Longitude,
                Runways = runwayModels
            };
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError("Exception occurred while gathering airport details: {ExMessage}", ex.Message);
        }

        return new AirportDetailsModel();
    }

    private static LatLong GetNormalizedLatLong(decimal latitude, decimal longitude)
    {
        // Need to determine whether the latitude is in the northern or southern hemisphere
        // and whether the longitude is in the eastern or western hemisphere.
        // Latitude is positive if in northern hemisphere, negative if in southern hemisphere.
        // Longitude is positive if in eastern hemisphere, negative if in western hemisphere.
        string latitudeDirection = latitude > 0 ? "N" : "S";
        string longitudeDirection = longitude > 0 ? "E" : "W";
        
        string latitudeString = GetNormalizedCoordinate(Math.Abs(latitude), latitudeDirection);
        string longitudeString = GetNormalizedCoordinate(Math.Abs(longitude), longitudeDirection);
        
        return new LatLong
        {
            Latitude = latitudeString,
            Longitude = longitudeString
        };
    }
    
    private static string GetNormalizedCoordinate(decimal coordinate, string direction)
    {
        // Cast both decimals to strings, then split by decimal point.
        string[] coordinateSplit = coordinate.ToString(CultureInfo.InvariantCulture).Split('.');
        string coordinateHours = coordinateSplit[0];
        string coordinateMinutes = coordinateSplit[1];
        
        decimal coordinateHoursAsAPercentage = decimal.Parse($".{coordinateMinutes}");
        decimal coordinateHoursAsMinutes = coordinateHoursAsAPercentage * 60;
        string coordinateMinutesString = coordinateHoursAsMinutes.ToString("F2", CultureInfo.InvariantCulture);
        
        return $"{direction}{coordinateHours}°{coordinateMinutesString}'";
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

            Debug.Assert(coverage != null, nameof(coverage) + " != null");
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
        
        List<RunwayModel> runwayModels = new();

        foreach (var element in elements)
        {
            var name = element.GetProperty("name").GetString();
            var magneticHeading = element.GetProperty("magneticHeading").GetInt16();
            var recipName = element.GetProperty("recipName").GetString();
            var recipMagneticHeading = element.GetProperty("recipMagneticHeading").GetInt16();
            
            runwayModels.Add(new RunwayModel
            {
                Name = name,
                MagneticHeading = magneticHeading
            });
            
            runwayModels.Add(new RunwayModel
            {
                Name = recipName,
                MagneticHeading = recipMagneticHeading
            });
        }

        return runwayModels;
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