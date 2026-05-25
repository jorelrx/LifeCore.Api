using AutoMapper;
using LifeOS.Application.Abstractions.Persistence;
using LifeOS.Application.Abstractions.Security;
using LifeOS.Application.Common.Exceptions;
using LifeOS.Application.Auth.Commands;
using LifeOS.Application.Auth.Dtos;
using LifeOS.Application.Users.Dtos;
using LifeOS.Domain.Entities;
using MediatR;

namespace LifeOS.Application.Auth.Handlers;

public sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken.Trim(), cancellationToken);

        if (storedRefreshToken is null || storedRefreshToken.IsRevoked || storedRefreshToken.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        var user = await _userRepository.GetByIdAsync(storedRefreshToken.UserId, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        var tokenPair = _tokenService.GenerateTokens(user);

        storedRefreshToken.IsRevoked = true;
        storedRefreshToken.RevokedAt = DateTimeOffset.UtcNow;
        storedRefreshToken.ReplacedByToken = tokenPair.RefreshToken;

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = tokenPair.RefreshToken,
            ExpiresAt = tokenPair.RefreshTokenExpiresAt,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(
            _mapper.Map<UserDto>(user),
            tokenPair.AccessToken,
            tokenPair.AccessTokenExpiresAt,
            tokenPair.RefreshToken,
            tokenPair.RefreshTokenExpiresAt);
    }
}