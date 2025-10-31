namespace DefaultNamespace;

public interface IPromptBuilder
{
    string Build(string originalCode, string modifiedCode, DiffResult diff, string fileName);
}