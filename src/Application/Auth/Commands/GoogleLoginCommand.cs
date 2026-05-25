using LifeOS.Application.Auth.Dtos;
using MediatR;

namespace LifeOS.Application.Auth.Commands;

public sealed record GoogleLoginCommand(string IdToken) : IRequest<AuthResponseDto>;