namespace GlennDemo.TweetSampler
{
    public interface ITweetSamplerService
    {
        Action<Exception> ErrorHandler { get; set; }

        int StatisticsReportingIntervalInSeconds { get; set; }

        Task StartSampling();

        void StopSampling();

        bool IsStreamTaskRunning();
    }
}
