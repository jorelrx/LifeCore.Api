using LifeCore.Application.Users.Dtos;

namespace LifeCore.Application.Auth.Dtos;

public sealed record AuthResponseDto(
    UserDto User,
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);