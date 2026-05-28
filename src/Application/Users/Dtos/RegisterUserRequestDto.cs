namespace LifeCore.Application.Users.Dtos;

public sealed record RegisterUserRequestDto(string FullName, string Email, string Password);