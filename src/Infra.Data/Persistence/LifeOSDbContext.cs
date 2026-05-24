using LifeOS.Application.Abstractions.Persistence;
using LifeOS.Domain.Entities;
using LifeOS.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Infra.Data.Persistence;

public sealed class LifeOSDbContext(DbContextOptions<LifeOSDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}