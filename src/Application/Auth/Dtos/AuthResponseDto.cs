using LifeOS.Application.Users.Dtos;

namespace LifeOS.Application.Auth.Dtos;

public sealed record AuthResponseDto(
    UserDto User,
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);