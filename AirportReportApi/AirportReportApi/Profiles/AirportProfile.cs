using AirportReportApi.Core.Models;
using AutoMapper;

namespace AirportReportApi.Core.Profiles;

public class AirportProfile : Profile
{
    public AirportProfile()
    {
        CreateMap<AirportWeatherModel, AirportDto>()
            .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.Temperature))
            .ForMember(dest => dest.RelativeHumidity, opt => opt.MapFrom(src => src.RelativeHumidity))
            .ForMember(dest => dest.CloudCoverage, opt => opt.MapFrom(src => src.CloudCoverage))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.WindSpeed, opt => opt.MapFrom(src => src.WindSpeed))
            .ForMember(dest => dest.WindDirection, opt => opt.MapFrom(src => src.WindDirection));
        
        CreateMap<AirportDetailsModel, AirportDto>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Runways, opt => opt.MapFrom(src => src.Runways));
    }
}