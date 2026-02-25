using System;
using AutoMapper;
using FirstAPI.DTO;
using FirstAPI.Models;

namespace FirstAPI.Mappings;

public class WeatherProfile : Profile
{
    public WeatherProfile()
    {
        CreateMap<WeatherRequest, Weather>();
        CreateMap<Weather, WeatherRequest>();
    }
}
