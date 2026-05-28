using AutoMapper;
using LifeCore.Application.Auth.Commands;
using LifeCore.Application.Auth.Dtos;

namespace LifeCore.Application.Auth.Profiles;

public sealed class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RefreshTokenRequestDto, RefreshTokenCommand>();
    }
}