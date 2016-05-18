using System;

namespace hq.metrics.tests
{
    public class MetricsFixture : IDisposable
    {
        public MetricsFixture()
        {
            M = new Metrics();
            H = new HealthChecks();
        }

        public void Dispose()
        {
            M.Dispose();
            H.Dispose();
        }

        public Metrics M { get; }
        public HealthChecks H { get; }
    }
}