namespace Touchliga.ModuleGenerator.Services;

public sealed class FileService : IFileService
{
    public async Task<FileWriteResult> WriteFileAsync(
        string path,
        string content,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(content);

        var directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(path))
        {
            var current = await File.ReadAllTextAsync(
                path,
                cancellationToken);

            if (current == content)
            {
                return FileWriteResult.Unchanged;
            }
        }

        await File.WriteAllTextAsync(
            path,
            content,
            cancellationToken);

        return File.Exists(path)
            ? FileWriteResult.Updated
            : FileWriteResult.Created;
    }

    // Compatibilidad con el código existente
    public void WriteFile(
        string path,
        string content)
    {
        WriteFileAsync(path, content)
            .GetAwaiter()
            .GetResult();
    }
}
