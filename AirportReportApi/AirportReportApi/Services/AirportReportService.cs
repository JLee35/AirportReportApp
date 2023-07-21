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

    private delegate AirportWeatherModel WeatherCollectionDelegate(AirportWeatherModel weatherModel, JsonElement parentElement);
    
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
        string details = await _airportRepository.GetAirportInformationById(id, ReportType.Details);
        JsonElement detailsRootElement = GetRootElement(details);
        return MapAirportDetails(detailsRootElement);
    }
    
    private async Task<AirportWeatherModel> GetAirportWeatherById(string id)
    {
        AirportWeatherModel weatherModel = new();
        string weather;

        try
        {
            weather = await _airportRepository.GetAirportInformationById(id, ReportType.Weather);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return weatherModel;
        }
        
        JsonElement rootElement = GetRootElement(weather);
        JsonElement reportElement = rootElement.GetProperty("report");
        JsonElement conditionsElement = reportElement.GetProperty("conditions");
        JsonElement forecastElement = reportElement.GetProperty("forecast");
        JsonElement forecastConditionsElement = forecastElement.GetProperty("conditions");
        JsonElement forecastPeriodElement = forecastElement.GetProperty("period");
        
        weatherModel = MapCurrentAirportWeather(conditionsElement);
        var weatherForecast = GetWeatherForecast(forecastConditionsElement);
        weatherModel.WeatherForecast = weatherForecast;
        
        // Get time offset.
        weatherModel.WeatherForecast.TimeOffset = GetForecastTimeOffset(forecastPeriodElement);

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

    private static AirportWeatherModel AddTempVisibilityAndHumidity(JsonElement conditionsElement,
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

    private static AirportWeatherModel AddWind(JsonElement conditionsElement, AirportWeatherModel weatherModel)
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

    private static AirportWeatherModel AddClouds(JsonElement conditionsElement,
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

    private static AirportWeatherModel GetWeatherByCategory(
        WeatherCollectionDelegate collectionDelegate, 
        AirportWeatherModel weatherModel, JsonElement parentElement)
    {
        try
        {
            weatherModel = collectionDelegate(weatherModel, parentElement);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"Exception occurred while gathering weather: {ex.Message}");
        }

        return weatherModel;
    }
    
    private static WeatherForecastModel GetWeatherForecast(JsonElement forecastConditionsElement)
    {
        WeatherForecastModel weatherForecastModel = new()
        {
            WindForecasts = GetWindForecastModels(forecastConditionsElement)
        };
        return weatherForecastModel;
    }

    private static List<WindForecastModel> GetWindForecastModels(JsonElement conditionsElement)
    {
        List<WindForecastModel> windForecastModels = new();
        var conditions = GetChildElements(conditionsElement);
        
        // According to specs, we only need the first two forecasts if available.
        for (int forecastCount = 0; forecastCount < 2; forecastCount++)
        {
            try
            {
                JsonElement windElement = conditions[forecastCount].GetProperty("wind");
                decimal windSpeedKts = windElement.GetProperty("speedKts").GetDecimal();
                bool isWindVariable = windElement.GetProperty("variable").GetBoolean();

                var windForecastModel = new WindForecastModel
                {
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
                // TODO: Log exception with Logger.
                Console.WriteLine($"Exception raised: {ex.Message}");
            }
        }

        return windForecastModels;
    }

    private static string GetForecastTimeOffset(JsonElement forecastPeriodElement)
    {
        var dateStart = forecastPeriodElement.GetProperty("dateStart").GetString();
        
        // In a production environment, we should log this.
        if (dateStart is null) return string.Empty;
        
        var start = DateTimeOffset.Parse(dateStart).UtcDateTime;
        var currentTime = DateTime.UtcNow;
        var difference = currentTime - start;
        
        // Format difference into hrs:min.
        string formattedDifference = difference.ToString("h\\h\\:mm\\m");
        return formattedDifference;
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