namespace Touchliga.ModuleGenerator.Services;

public interface ICodeModifier
{
    Task<bool> InsertAfterAsync(
        string file,
        string marker,
        string code);

    Task<bool> InsertBeforeAsync(
        string file,
        string marker,
        string code);

    Task<bool> ContainsAsync(
        string file,
        string code);
}
