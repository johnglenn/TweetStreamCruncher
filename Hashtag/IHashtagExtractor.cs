using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.Hashtag
{
    public interface IHashtagExtractor
    {
        IEnumerable<string> ExtractHashtags(string tweetText);
    }
}
