using System.Text;

namespace hq.metrics.Core
{
    public interface IMetric : ICopyable<IMetric>
    {
        void LogJson(StringBuilder sb);
    }
}

        