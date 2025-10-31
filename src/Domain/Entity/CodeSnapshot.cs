namespace DefaultNamespace;

public class CodeSnapshot
{
    public string Name { get; }
    public string Content { get; }

    public CodeSnapshot(string name, string content)
    {
        Name = name;
        Content = content;
    }
}