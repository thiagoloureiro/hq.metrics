using System.Collections.Generic;
using hq.metrics.Core;

namespace hq.metrics.Stats
{
    /// <summary>
    ///  A statistically representative sample of a data stream
    /// </summary>
    public interface ISample<out T> : ISample, ICopyable<T> { }

    /// <summary>
    ///  A statistically representative sample of a data stream
    /// </summary>
    public interface ISample
    {
        void Clear();
        int Count { get; }
        void Update(long value);
        ICollection<long> Values { get; }
    }
}


