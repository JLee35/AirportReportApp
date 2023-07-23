using AirportReportApi.Core.Models;
using AutoMapper;

namespace AirportReportApi.Core.Profiles;

public class AirportProfile : Profile
{
    public AirportProfile()
    {
        CreateMap<AirportWeatherModel, AirportDto>()
            .ForPath(dest => dest.Weather.TemperatureInF, opt => opt.MapFrom(src => $"{(src.TemperatureF * 9 / 5) + 32}"))
            .ForPath(dest => dest.Weather.RelativeHumidity, opt => opt.MapFrom(src => src.RelativeHumidityPercentage))
            .ForPath(dest => dest.Weather.CloudCoverage, opt => opt.MapFrom(src => src.CloudCoverage))
            .ForPath(dest => dest.Weather.VisibilitySm, opt => opt.MapFrom(src => src.VisibilitySm))
            .ForPath(dest => dest.Weather.WindForecasts, opt => opt.MapFrom(src => src.WindForecasts))
            .ForPath(dest => dest.Weather.WindSpeedMph, opt => opt.MapFrom(src => $"{src.WindSpeedKts * (decimal)1.15078}"))
            .ForPath(dest => dest.Weather.WindDirection,
                opt => opt.MapFrom(src => GetCardinalDirection(src.WindDirectionDegrees, src.IsWindVariable)));
            
        
        CreateMap<AirportDetailsModel, AirportDto>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Runways, opt => opt.MapFrom(src => src.Runways));
    }
    
    private static string GetCardinalDirection(int direction, bool isWindVariable)
    {
        if (isWindVariable) return "Variable";
        
        return direction switch
        {
            0 or 360 => "N",
            90 => "E",
            180 => "S",
            270 => "W",
            > 0 and < 90 => "NE",
            > 90 and < 180 => "SE",
            > 180 and < 270 => "SW",
            > 270 and < 360 => "NW",
            _ => "N/A"
        };
    }
}