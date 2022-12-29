using System.Collections.Concurrent;

namespace GlennDemo.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private ConcurrentBag<string> _hashtagsRaw = new();
        private ConcurrentDictionary<string, uint> _hashtagsWithCount = new();
        private ulong _tweetCount = 0;

        public IEnumerable<HashtagWithCount> GetTopHashtagsFromBuiltStatistics(int numberOfResults)
        {
            var topHashtags = _hashtagsWithCount
                .OrderByDescending(x => x.Value)
                .Take(numberOfResults);

            var results = topHashtags
                .Select(x => new HashtagWithCount(x.Key, x.Value));

            return results;
        }

        public ulong GetTweetCount()
        {
            return Interlocked.Read(ref _tweetCount);
        }

        public void RecordTweetThreadSafe(IEnumerable<string> hashtags)
        {
            foreach (var hashtag in hashtags)
                _hashtagsRaw.Add(hashtag);

            Interlocked.Increment(ref _tweetCount);
        }

        public Task BuildStatistics(CancellationToken cancellationToken)
        {
            var parallelConfig = new ParallelOptions()
            {
                CancellationToken = cancellationToken,
            };

            var result = Parallel.ForEach(_hashtagsRaw, parallelConfig,
                (hashtag, token) => IncrementHashtagCount(hashtag));

            return Task.CompletedTask;
        }

        private void IncrementHashtagCount(string hashtag)
        {
            _hashtagsWithCount.AddOrUpdate(
                hashtag, // key to find
                1, // value to add if key not found
                (_, existingCount) => existingCount + 1 // function to set value based on existing value if key found
                );
        }
    }
}
