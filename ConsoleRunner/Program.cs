using GlennDemo.TweetSampler;

// read twitter bearer token from user secrets
var config = ConsoleRunner.SecretAccessor.GetTwitterSecretsAndBuildConfig();

TweetSamplerService samplerService = new TweetSamplerService(config);

Console.WriteLine("Beginning to sample the tweet feed");
samplerService.StartSampling();
Console.WriteLine("- Done.");
Console.WriteLine("Expect your first statistics report in {0} seconds", samplerService.StatisticsReportingIntervalInSeconds);
Console.WriteLine("\r\nPress any key to end sampling.\r\n\r\n");

Console.ReadLine();

samplerService.StopSampling();