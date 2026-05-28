using LifeCore.Application.Abstractions.Persistence;
using LifeCore.Domain.Entities;
using LifeCore.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LifeCore.Infra.Data.Persistence;

public sealed class LifeCoreDbContext(DbContextOptions<LifeCoreDbContext> options) : DbContext(options), IUnitOfWork
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