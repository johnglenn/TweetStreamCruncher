using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
