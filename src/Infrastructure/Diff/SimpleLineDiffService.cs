namespace DefaultNamespace;

public class SimpleLineDiffService : IDiffService
{
    public DiffResult BuildDiff(string original, string modified)
    {
        var result = new DiffResult();

        var origLines = (original ?? string.Empty).Replace("\r\n", "\n").Split('\n');
        var modLines  = (modified ?? string.Empty).Replace("\r\n", "\n").Split('\n');

        var max = Math.Max(origLines.Length, modLines.Length);

        for (int i = 0; i < max; i++)
        {
            var o = i < origLines.Length ? origLines[i] : null;
            var m = i < modLines.Length ? modLines[i] : null;

            if (o == m)
            {
                if (o != null)
                    result.Lines.Add(new DiffHunk { Line = o, Type = DiffLineType.Unchanged });
            }
            else
            {
                if (o != null && m == null)
                    result.Lines.Add(new DiffHunk { Line = o, Type = DiffLineType.Removed });
                else if (o == null && m != null)
                    result.Lines.Add(new DiffHunk { Line = m, Type = DiffLineType.Added });
                else
                {
                    result.Lines.Add(new DiffHunk { Line = o!, Type = DiffLineType.Removed });
                    result.Lines.Add(new DiffHunk { Line = m!, Type = DiffLineType.Added });
                }
            }
        }

        return result;
    }
}