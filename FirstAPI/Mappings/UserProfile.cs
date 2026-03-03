using System;
using AutoMapper;
using FirstAPI.DTO;
using FirstAPI.Models;

namespace FirstAPI.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRequest, User>();
        CreateMap<User, UserRequest>();
    }
}
