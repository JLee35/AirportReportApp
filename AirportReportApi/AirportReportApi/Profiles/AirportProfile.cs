using AirportReportApi.Core.Models;
using AutoMapper;

namespace AirportReportApi.Core.Profiles;

public class AirportProfile : Profile
{
    public AirportProfile()
    {
        CreateMap<AirportWeatherModel, AirportDto>()
            .ForMember(dest => dest.TemperatureInF, opt => opt.MapFrom(src => $"{(src.TemperatureF * 9 / 5) + 32}"))
            .ForMember(dest => dest.RelativeHumidity, opt => opt.MapFrom(src => src.RelativeHumidityPercentage))
            .ForMember(dest => dest.CloudCoverage, opt => opt.MapFrom(src => src.CloudCoverage))
            .ForMember(dest => dest.VisibilitySm, opt => opt.MapFrom(src => src.VisibilitySm))
            .ForMember(dest => dest.WindSpeedMph, opt => opt.MapFrom(src => src.WindSpeedKts))
            .ForMember(dest => dest.ForecastTimeOffset, opt => opt.MapFrom(src => src.TimeOffset))
            .ForMember(dest => dest.WindDirection,
                opt => opt.MapFrom(src => GetCardinalDirection(src.WindDirectionDegrees)));
            
        
        CreateMap<AirportDetailsModel, AirportDto>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Runways, opt => opt.MapFrom(src => src.Runways));
    }
    
    private static string GetCardinalDirection(int direction)
    {
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