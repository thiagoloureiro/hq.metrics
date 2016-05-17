using System;

namespace hq.metrics.Stats
{
    public interface IDateTimeOffsetProvider
    {
        DateTimeOffset UtcNow { get; }
    }
}