using GlennDemo.Twitter.Models;
using Microsoft.Extensions.Configuration;

namespace ConsoleRunner
{
    internal static class SecretAccessor
    {
        internal static TwitterClientConfig GetTwitterSecretsAndBuildConfig()
        {
            try
            {
                var configRoot = new ConfigurationBuilder()
                    .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly())
                    .Build();

                string token = configRoot.GetRequiredSection("Twitter").GetRequiredSection("BearerToken").Value;

                TwitterClientConfig twitterConfig = new TwitterClientConfig(token);
                return twitterConfig;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred getting your Twitter API token.");
                Console.WriteLine("You must set your Twitter API bearer token in your User Secrets " +
                    "store following the schema in appsettings.json.");

                throw ex;
            }
        }
    }
}
