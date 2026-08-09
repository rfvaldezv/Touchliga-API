namespace Touchliga.ModuleGenerator.Services;

public sealed class CodeModifier : ICodeModifier
{
    private readonly IFileService _fileService = new FileService();

    public async Task<bool> ContainsAsync(
        string file,
        string code)
    {
        if (!File.Exists(file))
            return false;

        var source = await File.ReadAllTextAsync(file);

        return source.Contains(code);
    }

    public async Task<bool> InsertAfterAsync(
        string file,
        string marker,
        string code)
    {
        var source = await File.ReadAllTextAsync(file);

        if (source.Contains(code))
            return false;

        var index = source.IndexOf(marker, StringComparison.Ordinal);

        if (index < 0)
            throw new InvalidOperationException(
                $"No se encontró el marcador '{marker}'.");

        index += marker.Length;

        source = source.Insert(
            index,
            Environment.NewLine + code);

        await _fileService.WriteFileAsync(file, source);

        return true;
    }

    public async Task<bool> InsertBeforeAsync(
        string file,
        string marker,
        string code)
    {
        var source = await File.ReadAllTextAsync(file);

        if (source.Contains(code))
            return false;

        var index = source.IndexOf(marker, StringComparison.Ordinal);

        if (index < 0)
            throw new InvalidOperationException(
                $"No se encontró el marcador '{marker}'.");

        source = source.Insert(
            index,
            code + Environment.NewLine);

        await _fileService.WriteFileAsync(file, source);

        return true;
    }
}
