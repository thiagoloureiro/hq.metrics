using Xunit;
using hq.metrics.Core;

namespace hq.metrics.tests
{
    public class HealthChecksTests : IClassFixture<MetricsFixture>
    {
        private readonly MetricsFixture _fixture;

        public HealthChecksTests(MetricsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Registered_checks_are_visible_externally()
        {
            _fixture.H.Register("test-health-check", () => HealthCheck.Result.Healthy);
            Assert.True(_fixture.H.HasHealthChecks);
        }
    }
}
