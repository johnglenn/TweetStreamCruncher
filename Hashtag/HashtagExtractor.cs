using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.Hashtag
{
    public class HashtagExtractor : IHashtagExtractor
    {
        private static char[] _separators = new char[] { ' ', '\r', '\n' };

        public IEnumerable<string> ExtractHashtags(string tweetText)
        {
            var words = tweetText.Split(_separators, StringSplitOptions.TrimEntries);

            var hashtags = words
                .Where(w => w.StartsWith('#'))
                .Where(w => w.ToCharArray().Any(c => c != '#')) // at least one character is not a hashtag
                .Select(w => w.ToLower());

            return hashtags;
        }
    }
}
