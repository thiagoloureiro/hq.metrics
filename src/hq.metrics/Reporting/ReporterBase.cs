﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace hq.metrics.Reporting
{
    /// <summary>
    ///  A reporter that periodically prints out formatted application metrics to a specified <see cref="TextWriter" />
    /// </summary>
    public abstract class ReporterBase : IReporter
    {
        private CancellationTokenSource _token;
        protected TextWriter Out;
        protected readonly IReportFormatter Formatter;
        private long Runs { get; set; }

        protected ReporterBase(TextWriter writer, HealthChecks healthChecks) : this(writer, new HumanReadableReportFormatter(healthChecks))
        {
            Out = writer;
        }

        protected ReporterBase(TextWriter writer, Metrics metrics) : this(writer, new HumanReadableReportFormatter(metrics))
        {
            Out = writer;
        }

        protected ReporterBase(TextWriter writer, IReportFormatter formatter)
        {
            Out = writer;
            Formatter = formatter;
        }

        /// <summary>
        /// Starts the reporting task for periodic output
        /// </summary>
        /// <param name="period">The period between successive displays</param>
        /// <param name="unit">The period time unit</param>
        public virtual void Start(long period, TimeUnit unit)
        {
            var seconds = unit.Convert(period, TimeUnit.Seconds);
            var interval = TimeSpan.FromSeconds(seconds);

            _token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                OnStarted();
                while (!_token.IsCancellationRequested)
                {
                    await Task.Delay(interval, _token.Token);
	                if (!_token.IsCancellationRequested)
		                Run();
                }
            }, _token.Token);
        }

        public void Stop()
        {
            _token.Cancel();
            OnStopped();
        }

        public event EventHandler<EventArgs> Started;
        public void OnStarted()
        {
            var handler = Started;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> Stopped;
        public void OnStopped()
        {
            var handler = Stopped;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Run()
        {
            try
            {
                Out.Write(Formatter.GetSample());
                Out.Flush();
                Runs++;
            }
            catch (Exception e)
            {
                Out.WriteLine(e.StackTrace);
            }
        }

        public void Dispose()
        {
            _token?.Cancel();
            Out?.Dispose();
        }
    }
}
