namespace DefaultNamespace;

public enum DiffLineType
{
    Unchanged,
    Added,
    Removed
}

public class DiffHunk
{
    public string Line { get; set; } = "";
    public DiffLineType Type { get; set; }
}

public class DiffResult
{
    public List<DiffHunk> Lines { get; set; } = new();
}