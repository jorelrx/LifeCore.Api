using AutoMapper;
using LifeOS.Application.Auth.Commands;
using LifeOS.Application.Auth.Dtos;

namespace LifeOS.Application.Auth.Profiles;

public sealed class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RefreshTokenRequestDto, RefreshTokenCommand>();
    }
}