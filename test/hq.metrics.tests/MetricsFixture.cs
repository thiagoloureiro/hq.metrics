using System;

namespace hq.metrics.tests
{
    public class MetricsFixture : IDisposable
    {
        public MetricsFixture()
        {
            M = new Metrics();
        }

        public void Dispose()
        {
            M.Dispose();
        }

        public Metrics M { get; }
    }
}