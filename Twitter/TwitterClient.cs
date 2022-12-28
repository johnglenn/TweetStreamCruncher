using GlennDemo.Twitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.Twitter
{
    public class TwitterClient : HttpClient
    {
        public TwitterClient(TwitterClientConfig config) 
        {
            this.BaseAddress = new Uri("https://api.twitter.com/");
            var authorizationHeaderValue = String.Format("Bearer {0}", config.BearerToken);
            this.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);
        }
    }
}
