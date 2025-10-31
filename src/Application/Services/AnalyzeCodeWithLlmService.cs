namespace DefaultNamespace;

public class AnalyzeCodeWithLlmService
{
    private readonly BuildPromptService _buildPromptService;
    private readonly ILLMClient _llmClient;

    public AnalyzeCodeWithLlmService(BuildPromptService buildPromptService, ILLMClient llmClient)
    {
        _buildPromptService = buildPromptService;
        _llmClient = llmClient;
    }

    public async Task<(string prompt, string llmResponse)> AnalyzeAsync(
        string originalCode,
        string modifiedCode,
        string fileName,
        CancellationToken ct = default)
    {
        var prompt = _buildPromptService.Build(originalCode, modifiedCode, fileName);
        var answer = await _llmClient.SendAsync(prompt, ct);
        return (prompt, answer);
    }
}