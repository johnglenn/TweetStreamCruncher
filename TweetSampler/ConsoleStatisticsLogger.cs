using GlennDemo.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.TweetSampler
{
    internal class ConsoleStatisticsLogger : ITweetStatisticsLogger
    {
        public void LogTopHashtags(IEnumerable<HashtagWithCount> hashtags)
        {
            Console.WriteLine("TOP {0} HASHTAGS:", hashtags.Count());
            foreach (var hashtag in hashtags)
                Console.WriteLine("\t{0} with {1} occurrences", hashtag.Hashtag, hashtag.Occurrences);
            Console.WriteLine();
        }

        public void LogTweetCount(ulong tweetCount)
        {
            Console.WriteLine("TOTAL TWEETS: {0}", tweetCount);
        }
    }
}
