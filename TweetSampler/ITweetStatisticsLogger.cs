using GlennDemo.Statistics;

namespace GlennDemo.TweetSampler
{
    public interface ITweetStatisticsLogger
    {
        void LogTopHashtags(IEnumerable<HashtagWithCount> hashtags);

        void LogTweetCount(ulong tweetCount);
    }
}
