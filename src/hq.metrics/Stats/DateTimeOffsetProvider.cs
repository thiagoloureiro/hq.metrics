using System;

namespace hq.metrics.Stats
{
    internal class DateTimeOffsetProvider : IDateTimeOffsetProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}