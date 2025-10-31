namespace DefaultNamespace;

public class BuildPromptService
{
    private readonly IDiffService _diffService;
    private readonly IPromptBuilder _promptBuilder;

    public BuildPromptService(IDiffService diffService, IPromptBuilder promptBuilder)
    {
        _diffService = diffService;
        _promptBuilder = promptBuilder;
    }

    public string Build(string original, string modified, string fileName)
    {
        var diff = _diffService.BuildDiff(original, modified);
        return _promptBuilder.Build(original, modified, diff, fileName);
    }
}