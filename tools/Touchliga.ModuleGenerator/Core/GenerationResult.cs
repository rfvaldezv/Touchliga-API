namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Resultado de la generación de un módulo.
/// </summary>
public sealed class GenerationResult
{
    public IList<GeneratedFile> Files { get; }
        = new List<GeneratedFile>();

    public void Add(
        string fileName,
        string content)
    {
        Files.Add(
            new GeneratedFile
            {
                FileName = fileName,
                Content = content
            });
    }
}
