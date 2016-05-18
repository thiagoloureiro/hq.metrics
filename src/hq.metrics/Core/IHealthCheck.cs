namespace hq.metrics.Core
{
    public interface IHealthCheck
    {
        string Name { get; }
        IMetric Execute();
    }
}