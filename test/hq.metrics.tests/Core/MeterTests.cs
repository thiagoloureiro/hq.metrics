using System.Diagnostics;
using System.Threading;
using Xunit;

namespace hq.metrics.tests.Core
{
    public class MeterTests : IClassFixture<MetricsFixture>
    {
        private readonly MetricsFixture _fixture;

        public MeterTests(MetricsFixture fixture)
        {
            _fixture = fixture;
        }

 
        [Fact]
        public void Can_count()
        {
            var meter = _fixture.M.Meter(typeof(MeterTests), "Can_count", "test", TimeUnit.Seconds);
            meter.Mark(3);
            Assert.Equal(3, meter.Count);
        }

        [Fact]
        public void Can_meter()
        {
            const int count = 100000;
            var block = new ManualResetEvent(false);
            var meter = _fixture.M.Meter(typeof(MeterTests), "Can_meter", "test", TimeUnit.Seconds);
            Assert.NotNull(meter);

            var i = 0;
            ThreadPool.QueueUserWorkItem(s => 
            {
                while (i < count)
                {
                    meter.Mark();
                    i++;
                }
                Thread.Sleep(5000); // Wait for at least one EWMA rate tick
                block.Set();
            });
            block.WaitOne();

            Assert.Equal(count, meter.Count);

            var oneMinuteRate = meter.OneMinuteRate;
            var fiveMinuteRate = meter.FiveMinuteRate;
            var fifteenMinuteRate = meter.FifteenMinuteRate;
            var meanRate = meter.MeanRate;

            Assert.True(oneMinuteRate > 0);
            Trace.WriteLine("One minute rate:" + meter.OneMinuteRate);

            Assert.True(fiveMinuteRate > 0);
            Trace.WriteLine("Five minute rate:" + meter.FiveMinuteRate);
            
            Assert.True(fifteenMinuteRate > 0);
            Trace.WriteLine("Fifteen minute rate:" + meter.FifteenMinuteRate);

            Assert.True(meanRate > 0);
        }
    }
}