namespace Touchliga.ModuleGenerator.Abstractions;

public interface IOutputWriter
{
    Task WriteAsync(
        string path,
        string content);
}
