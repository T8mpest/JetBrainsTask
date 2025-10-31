namespace DefaultNamespace;

public interface ILLMClient
{
    Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default);
}