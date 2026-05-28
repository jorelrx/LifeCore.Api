using LifeCore.Application.Abstractions.Persistence;
using LifeCore.Domain.Entities;
using LifeCore.Infra.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LifeCore.Infra.Data.Repositories;

public sealed class RefreshTokenRepository(LifeCoreDbContext context) : IRefreshTokenRepository
{
    private readonly LifeCoreDbContext _context = context;

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