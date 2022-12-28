using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.Statistics
{
    public class HashtagWithCount
    {
        public HashtagWithCount(string hashtag, ulong occurrences)
        {
            Hashtag = hashtag;
            Occurrences = occurrences;
        }

        public string Hashtag { get; private set; }

        public ulong Occurrences { get; private set; }
    }
}
