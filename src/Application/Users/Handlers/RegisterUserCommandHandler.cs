using AutoMapper;
using LifeOS.Application.Abstractions.Persistence;
using LifeOS.Application.Abstractions.Security;
using LifeOS.Application.Auth.Dtos;
using LifeOS.Application.Common.Exceptions;
using LifeOS.Application.Users.Commands;
using LifeOS.Application.Users.Dtos;
using LifeOS.Domain.Entities;
using MediatR;

namespace LifeOS.Application.Users.Handlers;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (existingUser is not null)
        {
            throw new ConflictException("An account with this email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = _passwordHasher.Hash(request.Password),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            IsActive = true
        };

        await _userRepository.AddAsync(user, cancellationToken);

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