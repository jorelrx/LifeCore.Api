using LifeCore.Application.Auth.Dtos;
using MediatR;

namespace LifeCore.Application.Auth.Commands;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;