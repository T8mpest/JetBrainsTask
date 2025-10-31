using System;
using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using OpenAI;
using OpenAI.Chat;

namespace JetBrainsTask.Infrastructure.Llm
{
    public class OpenAiLlmClient : ILLMClient
    {
        private readonly ChatClient _chatClient;

        public OpenAiLlmClient(string? apiKey = null, string model = "gpt-4o-mini")
        {
            apiKey ??= Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                       ?? throw new InvalidOperationException("OPENAI_API_KEY is not set.");

            // новый openai .NET SDK
            _chatClient = new ChatClient(model, apiKey);
        }

        public async Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default)
        {
            ChatMessage[] messages = new ChatMessage[]
            {
                ChatMessage.CreateSystemMessage("You are a helpful AI code reviewer."),
                ChatMessage.CreateUserMessage(prompt)
            };

            var response = await _chatClient.CompleteChatAsync(
                messages,
                cancellationToken: cancellationToken
            );
            
            var content = response.Value.Content;
            if (content.Count == 0)
                return string.Empty;

            return content[0].Text ?? string.Empty;
        }
    }
}