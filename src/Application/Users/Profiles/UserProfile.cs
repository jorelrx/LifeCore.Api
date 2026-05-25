using AutoMapper;
using LifeOS.Application.Users.Commands;
using LifeOS.Application.Users.Dtos;
using LifeOS.Domain.Entities;

namespace LifeOS.Application.Users.Profiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequestDto, RegisterUserCommand>();
        CreateMap<User, UserDto>();
    }
}