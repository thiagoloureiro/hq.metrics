using System.Runtime.Serialization;
using hq.metrics.Support;

namespace hq.metrics.Core
{
    /// <summary> A marker for types that can copy themselves to another type </summary>
    public interface ICopyable<out T>
    {
        [IgnoreDataMember]
        T Copy { get; }
    }
}