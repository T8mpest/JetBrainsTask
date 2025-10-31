using System.Text;


namespace DefaultNamespace;

public class DefaultPromptBuilder : IPromptBuilder
{
    public string Build(string originalCode, string modifiedCode, DiffResult diff, string fileName)
    {
        var sb = new StringBuilder();

        sb.AppendLine("You are an AI code reviewer inside a JetBrains-like inspection system.");
        sb.AppendLine("Analyze the following change and describe potential issues or improvements.");
        sb.AppendLine();

        sb.AppendLine("== File ==");
        sb.AppendLine(fileName);
        sb.AppendLine();

        sb.AppendLine("== Original code ==");
        sb.AppendLine("```csharp");
        sb.AppendLine(originalCode);
        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("== Modified code ==");
        sb.AppendLine("```csharp");
        sb.AppendLine(modifiedCode);
        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("== Diff ==");
        sb.AppendLine("```diff");
        foreach (var line in diff.Lines)
        {
            var prefix = line.Type switch
            {
                DiffLineType.Added => "+ ",
                DiffLineType.Removed => "- ",
                _ => "  "
            };
            sb.AppendLine(prefix + line.Line);
        }
        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("== Instructions ==");
        sb.AppendLine("- Explain what changed.");
        sb.AppendLine("- Identify possible bugs or regressions.");
        sb.AppendLine("- Suggest improvements or code style fixes.");

        return sb.ToString();
    }
}