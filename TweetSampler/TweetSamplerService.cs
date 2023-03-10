using GlennDemo.Hashtag;
using GlennDemo.JsonParser;
using GlennDemo.Statistics;
using GlennDemo.Twitter;
using GlennDemo.Twitter.Models;
using GlennDemo.Utilities;

namespace GlennDemo.TweetSampler
{
    public class TweetSamplerService : ITweetSamplerService
    {
        private ITwitterClient _client;
        private IStreamTaskMachine _streamingService;
        private IStatisticsService _statisticsService;
        private IHashtagExtractor _hashtagExtractor;
        private ITweetStatisticsLogger _statisticsLogger;
        private System.Timers.Timer _statisticsTimer;

        private Action<Exception> _errorHandler = exception => throw exception;
        private CancellationTokenSource _cancellationTokenSource = new ();

        private Task? _streamingTask;

        // TODO: downselect to a single parser once performance has been evaluated

        // constructor for running without a DI container
        public TweetSamplerService(TwitterClientConfig config)
        {
            _client = new TwitterClient(config);
            _streamingService = new StreamingService(new RuntimeTaskTracker());
            _hashtagExtractor = new HashtagExtractor();
            _statisticsService = new StatisticsService();
            _statisticsLogger = new ConsoleStatisticsLogger();

            _statisticsTimer = BuildStatisticsTimer();
        }

        public TweetSamplerService(ITwitterClient client, IStreamTaskMachine streamingService,
            IStatisticsService statisticsService, IHashtagExtractor hashtagExtractor,
            ITweetStatisticsLogger statisticsLogger)
        {
            _client = client;
            _streamingService = streamingService;
            _statisticsService = statisticsService;
            _hashtagExtractor = hashtagExtractor;
            _statisticsLogger = statisticsLogger;

            _statisticsTimer = BuildStatisticsTimer();
        }

        private System.Timers.Timer BuildStatisticsTimer()
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += ReportStatisticsTimerEventHandler;
            timer.Interval = 10000;
            return timer;
        }

        public Action<Exception> ErrorHandler { get => _errorHandler; set => _errorHandler = value; }
        public int StatisticsReportingIntervalInSeconds
        { 
            get => (int)(_statisticsTimer.Interval / 1000);
            set
            {
                if (value < 10)
                    throw new Exception("reporting interval is too short");
                else if (value > 600)
                    throw new Exception("reporting interval is too long");
                else
                    _statisticsTimer.Interval = value * 1000;
            }
        }
        public ushort TopHashtagsToReport { get; set; } = 10;

        public ushort TweetStreamLoadMultiplier { get; set; } = 1;

        public bool IsStreamTaskRunning()
        {
            return _streamingTask?.Status == TaskStatus.Running;
        }

        public async Task StartSampling()
        {
            _statisticsTimer.Start();

            await StartStreamingTask();
        }

        // TODO: this is incredibly rudimentary connection repair; refactor
        private async Task StartStreamingTask()
        {
            // TODO: handle error conditions in the API connection
            // TODO: handle disconnects and reconnect automatically
            // NOTE regarding error handling: uncaught API exceptions currently cause the task
            // .. to fault which triggers a restart of the stream; don't add error handling
            // .. without also implementing disconnect handling
            var stream = await _client.GetStreamAsync("/2/tweets/sample/stream", _cancellationTokenSource.Token);


            bool streamEndedUnexpectedly = _streamingTask?.Status == TaskStatus.RanToCompletion;
            bool streamShouldStart = _streamingTask == null || streamEndedUnexpectedly;

            if (streamShouldStart == false)
                return;

            _streamingTask = _streamingService.MakeTasksFromLinesInStream(
                stream,
                ExtractAndStoreHashtagsFromStreamedTweetJson,
                _errorHandler,
                _cancellationTokenSource.Token
                )
                // rerun this function if the streaming task ends
                .ContinueWith((_, _) => StartStreamingTask(), _cancellationTokenSource.Token, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void StopSampling()
        {
            _cancellationTokenSource.Cancel();
        }

        private IEnumerable<Task> ExtractAndStoreHashtagsFromStreamedTweetJson(string? json)
        {
            var parser = GetJsonParser();

            List<Task> tasksSpawned = new List<Task>();
            for (int i = 0; i < TweetStreamLoadMultiplier; i++)
            {
                tasksSpawned.Add(Task.Run(() =>
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(json))
                            return;

                        var tweet = parser.DeserializeJsonStringToModel(json);

                        var hashtags = _hashtagExtractor.ExtractHashtags(tweet.Data.Text);

                        _statisticsService.RecordTweetThreadSafe(hashtags);
                    }

                    // rather than degrading performane with null checks, we'll just catch the exception
                    catch (NullReferenceException) { }
                    catch (JsonParserException) { }
                }));
            }

            return tasksSpawned;
        }

        private IJsonParser<TweetStreamDataWrapper> GetJsonParser()
        {
            // alternate between the native and newtsonsoft parser while measuring performance
            if (Random.Shared.Next(2) == 1)
                return Parsers.NewtonsoftParser;
            else
                return Parsers.NativeParser;
        }

        private void ReportStatisticsTimerEventHandler(object? sender, System.Timers.ElapsedEventArgs e)
        {
            // TODO: strip out debug logging before production
            _streamingService.LogTaskStatisticsToConsole();

            Parsers.LogTimesAndResetTimeCollections();

            Task statsBuilder = _statisticsService.BuildStatistics(_cancellationTokenSource.Token);
            statsBuilder.Wait();

            if (statsBuilder.IsFaulted)
                throw statsBuilder.Exception ?? new Exception("statistics failed to build");

            var topHashtags = _statisticsService.GetTopHashtagsFromBuiltStatistics(TopHashtagsToReport);
            var tweetCount = _statisticsService.GetTweetCount();

            _statisticsLogger.LogTweetCount(tweetCount);
            _statisticsLogger.LogTopHashtags(topHashtags);
        }
    }
}
