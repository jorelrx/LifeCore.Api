using LifeCore.Application.Auth.Dtos;
using MediatR;

namespace LifeCore.Application.Users.Commands;

public sealed record RegisterUserCommand(string FullName, string Email, string Password) : IRequest<AuthResponseDto>;