using Infrastructure.shared.Services.Abstractions;

namespace Infrastructure.shared.Services.Implementations;

public class CurrentDateProvider : ICurrentDateProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTimeOffset NowUtc => DateTimeOffset.UtcNow;
}
