using System;

namespace hq.metrics.Reporting
{
    public class ConsoleReporter : ReporterBase
    {
        public ConsoleReporter(HealthChecks healthChecks) : base(Console.Out, healthChecks) { }
        public ConsoleReporter(Metrics metrics) : base(Console.Out, metrics) { }
        public ConsoleReporter(IReportFormatter formatter) : base(Console.Out, formatter) { }
    }
}
