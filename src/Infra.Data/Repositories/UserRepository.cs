using LifeOS.Application.Abstractions.Persistence;
using LifeOS.Domain.Entities;
using LifeOS.Infra.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Infra.Data.Repositories;

public sealed class UserRepository(LifeOSDbContext context) : IUserRepository
{
    private readonly LifeOSDbContext _context = context;

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        return _context.Users.AddAsync(user, cancellationToken).AsTask();
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _context.Users
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken)
    {
        return _context.Users
            .FirstOrDefaultAsync(user => user.GoogleId == googleId, cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Users
            .Include(user => user.RefreshTokens)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
}