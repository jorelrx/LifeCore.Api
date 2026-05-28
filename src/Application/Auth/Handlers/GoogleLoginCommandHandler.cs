using AutoMapper;
using LifeCore.Application.Abstractions.Persistence;
using LifeCore.Application.Abstractions.Security;
using LifeCore.Application.Auth.Commands;
using LifeCore.Application.Auth.Dtos;
using LifeCore.Application.Common.Exceptions;
using LifeCore.Application.Users.Dtos;
using LifeCore.Domain.Entities;
using MediatR;

namespace LifeCore.Application.Auth.Handlers;

public sealed class GoogleLoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IGoogleAuthService googleAuthService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GoogleLoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IGoogleAuthService _googleAuthService = googleAuthService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<AuthResponseDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _googleAuthService.ValidateIdTokenAsync(request.IdToken, cancellationToken);
        var normalizedEmail = result.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByGoogleIdAsync(result.GoogleId, cancellationToken);

        user ??= await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                FullName = string.IsNullOrWhiteSpace(result.FullName) ? normalizedEmail : result.FullName,
                Email = normalizedEmail,
                PasswordHash = string.Empty,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                GoogleId = result.GoogleId,
                GoogleAccessToken = null,
                GoogleRefreshToken = null,
                GoogleTokenExpiresAt = null
            };

            await _userRepository.AddAsync(user, cancellationToken);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(user.GoogleId) && !string.Equals(user.GoogleId, result.GoogleId, StringComparison.Ordinal))
            {
                throw new ConflictException("This account is already linked to another Google identity.");
            }

            user.FullName = string.IsNullOrWhiteSpace(user.FullName) ? result.FullName : user.FullName;
            user.Email = normalizedEmail;
            user.IsActive = true;
            user.GoogleId = result.GoogleId;
            user.GoogleAccessToken = null;
            user.GoogleRefreshToken = null;
            user.GoogleTokenExpiresAt = null;
            user.UpdatedAt = DateTimeOffset.UtcNow;
        }

        var tokenPair = _tokenService.GenerateTokens(user);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = tokenPair.RefreshToken,
            ExpiresAt = tokenPair.RefreshTokenExpiresAt,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(
            _mapper.Map<UserDto>(user),
            tokenPair.AccessToken,
            tokenPair.AccessTokenExpiresAt,
            tokenPair.RefreshToken,
            tokenPair.RefreshTokenExpiresAt);
    }
}