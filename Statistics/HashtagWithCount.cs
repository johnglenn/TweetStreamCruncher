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
