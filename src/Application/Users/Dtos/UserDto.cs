namespace LifeCore.Application.Users.Dtos;

public sealed record UserDto(
    Guid Id,
    string FullName,
    string Email,
    DateTimeOffset CreatedAt);