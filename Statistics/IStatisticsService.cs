namespace GlennDemo.Statistics
{
    public interface IStatisticsService
    {
        public void RecordTweetThreadSafe(IEnumerable<string> hashtags);

        public Task BuildStatistics(CancellationToken cancellationToken);

        public IEnumerable<HashtagWithCount> GetTopHashtagsFromBuiltStatistics(int numberOfResults);

        public ulong GetTweetCount();
    }
}
