namespace GlennDemo.Twitter
{
    public interface ITwitterClient
    {
        Task<Stream> GetStreamAsync(string? requestUri, CancellationToken cancellationToken);
    }
}
