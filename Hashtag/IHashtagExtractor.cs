namespace GlennDemo.Hashtag
{
    public interface IHashtagExtractor
    {
        IEnumerable<string> ExtractHashtags(string tweetText);
    }
}
