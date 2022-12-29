using GlennDemo.Hashtag;
using System.Collections.Generic;
using Xunit;

namespace HashtagExtractorTests
{
    public class HashtagExtractorTest
    {
        private HashtagExtractor _extractor;
        public static IEnumerable<object[]> _dataWithoutHashtags => Data.TweetsWithoutHashtags;

        public HashtagExtractorTest()
        {
            _extractor = new HashtagExtractor();
        }

        [Fact]
        public void WhenDuplicateHashtags_ThenResultContainsDuplicates()
        {
            string tweet = "The most important thing in real estate is #Location #Location #Location";

            var hashtags = _extractor.ExtractHashtags(tweet);

            Assert.Collection(hashtags,
                h => h.Equals("#location"),
                h => h.Equals("#location"),
                h => h.Equals("#location")
                );

        }

        [Theory]
        [MemberData(nameof(_dataWithoutHashtags))]
        public void WhenNoHashtagsInTweet_ThenResultIsEmptyCollection(string tweet)
        {
            var hashtags = _extractor.ExtractHashtags(tweet);

            Assert.Empty(hashtags);
        }

        [Fact]
        public void WhenImproperCompoundHashtag_ThenOnlyOneResultReturned()
        {
            string tweet = "There should always be a space #Between#Hashtags";

            var hashtags = _extractor.ExtractHashtags(tweet);

            Assert.Collection(hashtags,
                x => x.Equals("#between#hashtags")
                );
        }
    }
}