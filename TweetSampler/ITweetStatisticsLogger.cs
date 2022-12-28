using GlennDemo.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.TweetSampler
{
    public interface ITweetStatisticsLogger
    {
        void LogTopHashtags(IEnumerable<HashtagWithCount> hashtags);

        void LogTweetCount(ulong tweetCount);
    }
}
