using System;
using System.Text;

namespace hq.metrics.Core
{
    /// <summary> A template class for an encapsulated service health check </summary>
    public class HealthCheck : IHealthCheck
    {
        public static Result Healthy => Result.Healthy;
        public static Result Unhealthy(string message) { return Result.Unhealthy(message); }
        public static Result Unhealthy(Exception error) { return Result.Unhealthy(error); } 

        private readonly Func<Result> _check;

        public string Name { get; }

        public HealthCheck(string name, Func<Result> check)
        {
            Name = name;
            _check = check;
        }

        public IMetric Execute()
        {
            try
            {
                return _check();
            }
            catch (Exception e)
            {
                return Result.Unhealthy(e);
            }
        }

        public void LogJson(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public sealed class Result : IMetric 
        {
            private static readonly Result _healthy = new Result(true, null, null);

            public static Result Healthy => _healthy;

            public static Result Unhealthy(string errorMessage)
            {
                return new Result(false, errorMessage, null);
            }

            public static Result Unhealthy(Exception error)
            {
                return new Result(false, error.Message, error);
            }

            public string Message { get; }
            
            public Exception Error { get; }

            public bool IsHealthy { get; }
            
            private Result(bool isHealthy, string message, Exception error)
            {
                IsHealthy = isHealthy;
                Message = message;
                Error = error;
            }

            public IMetric Copy => new Result(IsHealthy, Message, Error);

            public void LogJson(StringBuilder sb)
            {
                sb.Append("{")
                    .Append("\"is_healthy\": \"").Append(IsHealthy).Append("\"");
                if (!string.IsNullOrEmpty(Message))
                {
                    sb.Append(",");
                    sb.Append("\"message\":\"").Append(Message).Append("\"");
                }
                if (!string.IsNullOrEmpty(Error?.Message))
                {
                    sb.Append(",");
                    sb.Append("\"error\":\"").Append(Error.Message).Append("\"");
                }
                sb.Append("}");
            }
        }
    }
}