namespace Touchliga.ModuleGenerator.Services;

public interface IFileService
{
    Task<FileWriteResult> WriteFileAsync(
        string path,
        string content,
        CancellationToken cancellationToken = default);

    void WriteFile(
        string path,
        string content);
}
