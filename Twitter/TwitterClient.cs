using GlennDemo.Twitter.Models;

namespace GlennDemo.Twitter
{
    public class TwitterClient : HttpClient, ITwitterClient
    {
        public TwitterClient(TwitterClientConfig config) 
        {
            this.BaseAddress = new Uri("https://api.twitter.com/");
            var authorizationHeaderValue = String.Format("Bearer {0}", config.BearerToken);
            this.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);
        }
    }
}
