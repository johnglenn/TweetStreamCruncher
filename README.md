# What is the TweetStreamCruncher?
This is a demo project that I built in a few hours.  My main purpose is to demonstrate clean, performant code.  The prompt was to build a system that ingests data from [Twitter's sampled stream API](https://developer.twitter.com/en/docs/twitter-api/tweets/volume-streams/introduction) and periodically reports the top ten hashtags.  The sampled stream API delivers a random sample of 1% of Twitters tweet volume.  The system was intentionally constructed using in-memory storage for the ingested data for purposes of this demo.  The modular design of the system allows for easy extension at a later date to implement a different data persistence strategy (by creating a new IStatisticsService).

# How to Run
1. Store your Twitter bearer token in your User Secrets store on the ConsoleRunner project.  [This documentation](https://developer.twitter.com/en/docs/authentication/oauth-2-0/bearer-tokens) explains how to obtain an app-only bearer token from the Twitter Developer Portal.
```
cd ConsoleRunner
dotnet user-secrets set "Twitter:BearerToken" "your_secret_goes_here"
```
2. Run the app!

# Performance
This system delivers consistent memory utilization (expected between 20 - 30 MB when running in 1x mode - more on that in a moment).  Garbage collection is expected a few times per minute; if it occurs every second or two consistently, please raise an issue.  On my laptop (Intel i7 8th Gen, 16 GB RAM), CPU utilization was consistently less than 1%.

Memory utilization grows roughly 1 MB per 10 minutes due to the use of in-memory data persistence.  As 1000x and 2000x testing below will show, the system exhibits memory stability at scale.

NOTE: The RuntimeTaskTracker is the debugging harness for keeping track of the pending JSON parsing tasks.  This tracker represents the largest memory leak vulnerability as the tracker only clears the task queue when task statistics are built.  Task continuation could be used to build these statistics on task completion without maintaining a collection of spawned tasks.  This is future work.

## JSON Serialization
With every statistics report, the system also outputs performance metrics for the two competing JSON parsers - one using native .NET JSON deserialization and one using Newtonsoft's library.  Are these components truly parsers?  Meh... it felt more succinct than calling them Deserializers.  Feel free to fork and rename if it will make your coffee stronger.

# Generate Load
To generate artificial load on the system, set the TweetStreamLoadMultiplier of the TweetSamplerService to cause each line of JSON from the sampled stream to be processed multiple times.

In my testing on my laptop with a 1000x load multiple (aka: 10x the real-time tweet volume on Twitter), the system was able to process 2.5 million tweets in 90 seconds while consuming a relatively stable 120 MB of memory and trending around 30% CPU utilization.  An initial backlog of over 1,500 parsing tasks built up while the runtime was optimizing the execution of the JSON parser.  However, the bottleneck was eliminated after 15 - 20 seconds and did not reappear.

Running under 2000x load, memory utilization was between 180 - 220 MB with very frequent garbage collection.  After processing 5 million tweets in 90 seconds, the non-blocking statistics building had a noticeable delay of at least 1 second.  I am satisfied with the performance and stability of the system.

# Customization
The ITweetSamplerService allows a custom StatisticsReportingIntervalInSeconds and ErrorHandler to be configured on the service.  The structure is in place to inject dependencies from a DI container.

# Known Bugs
1. Hashtags displayed to the console are limited to ASCII characters, so some hashtags appear as question marks in the console despite retaining the actual hashtag values within the system.  A custom ITweetStatisticsLogger could output this data to file to display the actual values; a custom logger could just as easily store the data in an in-memory EF context or server-side cache to build a quick API for accessing the data.

# Future Work
I chose not to adhere to Test-Driven Development while working on this project to balance other time constraints.  In a professional setting, work is not done unless it is tested; ergo, this work should not be on the main or development trunk of a real-world project.  A few hours writing additional test cases (end to end, proof of thread safety, parsing cases, etc.) would be time well spent.

There is some non-production code, particularly around monitoring the task queue and JSON parser performance, that should be removed before production use.  It is useful for measuring the performance and stability of the system while in testing.  Perhaps a dedicated test harness that could either be injected (when testing) or replaced with a dummy do-no-work version when not would be a cleaner way to have our cake and eat it, too.  This may look like an IParserTimingService.

For continuous use, a better data persistence strategy is obviously required.  Retaining the raw hashtag data and rebuilding statistics on demand was a simpler strategy to implement, but it will not scale long-term as the raw hashtag data set will grow in size much faster than the statistics set would.

Twitter's API documentation lists several error conditions that should be handled by a client of their stream endpoints.  Only minimal efforts were made to cover those conditions (e.g. dumb re-starting of the stream service if the task completes unexpectedly).

Finally, this project lacks documentation of the architecture, design decisions, and guidance for contributors.  This is on the backlog, as well.
