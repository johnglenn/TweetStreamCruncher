# TweetStreamCruncher
This is a demo project that I built in a few hours.  The main purpose is to demonstrate clean, performant code.

# How to Run
1. Store your Twitter bearer token in your User Secrets store on the ConsoleRunner project.  You can read more about the User Secrets store [here](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#set-a-secret).
```
cd ConsoleRunner
dotnet user-secrets set "Twitter:BearerToken" "your_secret_goes_here"
```
2. Run the app!

# Customization
The ITweetSamplerService allows a custom StatisticsReportingIntervalInSeconds and ErrorHandler to be configured on the service.  Most of the dependencies can be extended and the replacement dependency used.

# Performance
This system delivers consistent memory utilization (expected between 20 - 30 MB).  Garbage collection is expected a few times per minute; if it occurs every second or two consistently, please raise an issue.  On my laptop (Intel i7 8th Gen, 16 GB RAM), CPU utilization was consistently less than 1%.

Memory utilization grows roughly 1 MB per 10 minutes based on my testing.

## JSON Serialization
With every statistics report, the system also outputs performance metrics for the two competing JSON parsers - one using native .NET JSON deserialization and one using Newtonsoft's library.  Are these components truly parsers?  Meh... it felt more succinct than calling them Deserializers.  Feel free to fork and rename if it will make your coffee stronger.

# Known Bugs
1. Hashtags displayed to the console are limited to ASCII characters, so some hashtags appear as question marks in the console.  A custom ITweetStatisticsLogger could output this data to file to display the actual values; a custom logger could just as easily store the data in an in-memory EF context or server-side cache to build a quick API for accessing the data.

# Future Work
I chose not to adhere to Test-Driven Development while working on this project to balance other time constraints.  In a professional setting, work is not done unless it is tested; ergo, this work should not be on the main or development trunk of a real-world project.  A few hours writing additional test cases (end to end, proof of thread safety, parsing cases, etc.) would be time well spent.

There is some non-production code, particularly around monitoring the task queue and JSON parser performance, that should be removed before production use.

For continuous use, a better data persistence strategy is required.  Retaining the raw hashtag data and rebuilding statistics on demand is a simpler strategy to implement, but it will not scale long-term as the raw hashtag data set will grow in size much faster than the statistics set would.

Additionally, Twitter's API documentation lists several error conditions that should be handled by a client of their stream endpoints.  Only minimal efforts were made to cover those conditions (e.g. dumb re-starting of the stream service if the task completes unexpectedly).

Finally, this project lacks documentation of the architecture, design decisions, and guidance for contributors.  This is on the backlog, as well.