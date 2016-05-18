using System;
using System.Collections.Generic;
using hq.metrics.Core;
using hq.metrics.Util;

namespace hq.metrics.Reporting
{
    public class JsonReportFormatter : IReportFormatter
    {
        private readonly Func<IDictionary<MetricName, IMetric>> _producer;

        public JsonReportFormatter(Metrics metrics) : this(() => metrics.AllSorted) { }
        
        public JsonReportFormatter(HealthChecks healthChecks) : this(healthChecks.RunHealthChecks) { }

        public JsonReportFormatter(Func<IDictionary<MetricName, IMetric>> producer)
        {
            _producer = producer;
        }

        public string GetSample()
        {
            return Serializer.Serialize(_producer());
        }
    }
}