using DefaultNamespace;
using OpenAI.Chat;

namespace JetBrainsTask.Infrastructure.Llm;

public class OpenAiLlmClient : ILLMClient
{
    private readonly ChatClient _chatClient;

    public OpenAiLlmClient(string? apiKey = null, string model = "gpt-4o")
    {
        apiKey ??= Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                   ?? throw new InvalidOperationException("OPENAI_API_KEY is not set.");

        // official OpenAI .NET SDK usage
        _chatClient = new ChatClient(model: model, apiKey: apiKey);
    }

    public async Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var completion = await _chatClient.CompleteChatAsync(prompt, cancellationToken);
        return completion.Content[0].Text;
    }
}