namespace GlennDemo.Twitter.Models
{
    public class TwitterClientConfig
    {
        public TwitterClientConfig(string bearerToken)
        {
            BearerToken = bearerToken;
        }

        public string BearerToken { get; set; }
    }
}
