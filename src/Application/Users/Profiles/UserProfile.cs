using AutoMapper;
using LifeCore.Application.Users.Commands;
using LifeCore.Application.Users.Dtos;
using LifeCore.Domain.Entities;

namespace LifeCore.Application.Users.Profiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequestDto, RegisterUserCommand>();
        CreateMap<User, UserDto>();
    }
}