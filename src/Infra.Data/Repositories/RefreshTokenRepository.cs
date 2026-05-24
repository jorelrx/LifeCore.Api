using LifeOS.Application.Abstractions.Persistence;
using LifeOS.Domain.Entities;
using LifeOS.Infra.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Infra.Data.Repositories;

public sealed class RefreshTokenRepository(LifeOSDbContext context) : IRefreshTokenRepository
{
    private readonly LifeOSDbContext _context = context;

    public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        return _context.RefreshTokens.AddAsync(refreshToken, cancellationToken).AsTask();
    }

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return _context.RefreshTokens
            .Include(refreshToken => refreshToken.User)
            .FirstOrDefaultAsync(refreshToken => refreshToken.Token == token, cancellationToken);
    }
}