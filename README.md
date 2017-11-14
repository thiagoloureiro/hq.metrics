[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fhq-io%2Fmetrics.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fhq-io%2Fmetrics?ref=badge_shield)

HQ.io Metrics
=============

How To Use
----------
**First**, specify Metrics as a dependency:

    PM> Install-Package hq.metrics    

**Second**, instrument your classes:

```csharp
using Metrics;
public class ThingFinder
{
	// Measure the # of records per second returned
    private IMetric _resultsMeter;
  
    // Measure the # of milliseconds each query takes and the number of queries per second being performed
    private IMetric _dbTimer;

	public ThingFinder(Metrics metrics)
	{
		_resultsMeter = metrics.Meter(typeof(ThingFinder), "results", TimeUnit.Seconds);
		_dbTimer = metrics.Timer(typeof(ThingFinder), "database", TimeUnit.Milliseconds, TimeUnit.Seconds);
	}
	      
    public void FindThings()
    {
        // Perform an action which gets timed
        var results = _dbTimer.Time(() => {                            
            Database.Query("SELECT Unicorns FROM Awesome");
        }

        // Calculate the rate of new things found
        _resultsMeter.Mark(results.Count)                
    
        // etc.
    }
}
```

Metrics comes with five types of metrics:

* **Gauges** are instantaneous readings of values (e.g., a queue depth).
* **Counters** are 64-bit integers which can be incremented or decremented.
* **Meters** are increment-only counters which keep track of the rate of events.
  They provide mean rates, plus exponentially-weighted moving averages which
  use the same formula that the UNIX 1-, 5-, and 15-minute load averages use.
* **Histograms** capture distribution measurements about a metric: the count,
  maximum, minimum, mean, standard deviation, median, 75th percentile, 95th
  percentile, 98th percentile, 99th percentile, and 99.9th percentile of the
  recorded values. (They do so using a method called reservoir sampling which
  allows them to efficiently keep a small, statistically representative sample
  of all the measurements.)
* **Timers** record the duration as well as the rate of events. In addition to
  the rate information that meters provide, timers also provide the same metrics
  as histograms about the recorded durations. (The samples that timers keep in
  order to calculate percentiles and such are biased towards more recent data,
  since you probably care more about how your application is doing *now* as
  opposed to how it's done historically.)

Metrics also has support for health checks:
```csharp
HealthChecks.Register("database", () =>
{
    if (Database.IsConnected)
    {
        return HealthCheck.Healthy;
    }
    else
    {
        return HealthCheck.Unhealthy("Not connected to database");
    }
});
```
  
**Third**, start collecting your metrics.

If you're simply running a benchmark, you can print registered metrics to 
standard output, every 10 seconds like this:

```csharp
// Print to Console.Out every 10 seconds
Metrics.EnableConsoleReporting(10, TimeUnit.Seconds) 
```
    
License
-------
The original Metrics project is Copyright 2010-2012 Coda Hale and Yammer, Inc.

Both works are published under an Apache 2.0 License, see LICENSE and https://github.com/dropwizard/metrics/blob/3.1-maintenance/LICENSE


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fhq-io%2Fmetrics.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fhq-io%2Fmetrics?ref=badge_large)