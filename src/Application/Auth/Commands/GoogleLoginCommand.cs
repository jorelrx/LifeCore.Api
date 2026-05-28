using LifeCore.Application.Auth.Dtos;
using MediatR;

namespace LifeCore.Application.Auth.Commands;

public sealed record GoogleLoginCommand(string IdToken) : IRequest<AuthResponseDto>;