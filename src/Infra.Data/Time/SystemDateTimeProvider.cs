using LifeOS.Application.Abstractions.Time;

namespace LifeOS.Infra.Data.Time;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}