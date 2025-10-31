namespace DefaultNamespace;

public interface IDiffService
{
    DiffResult BuildDiff(string original, string modified);
}