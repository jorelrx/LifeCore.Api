using LifeCore.Application.Abstractions.Persistence;
using LifeCore.Domain.Entities;
using LifeCore.Infra.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LifeCore.Infra.Data.Repositories;

public sealed class UserRepository(LifeCoreDbContext context) : IUserRepository
{
    private readonly LifeCoreDbContext _context = context;

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