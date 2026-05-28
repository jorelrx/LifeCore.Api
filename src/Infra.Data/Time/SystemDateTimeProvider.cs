using LifeCore.Application.Abstractions.Time;

namespace LifeCore.Infra.Data.Time;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}