using LifeOS.Application.Auth.Dtos;
using MediatR;

namespace LifeOS.Application.Users.Commands;

public sealed record RegisterUserCommand(string FullName, string Email, string Password) : IRequest<AuthResponseDto>;