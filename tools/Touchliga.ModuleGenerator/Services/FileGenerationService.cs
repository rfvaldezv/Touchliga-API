using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Services;

/// <summary>
/// Escribe en disco los archivos generados por el FMG.
/// </summary>
public sealed class FileGenerationService
{
    public void Save(GenerationResult result)
    {
        foreach (var file in result.Files)
        {
            var directory = Path.GetDirectoryName(file.FileName);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var existed = File.Exists(file.FileName);

            File.WriteAllText(file.FileName, file.Content);

            Console.WriteLine(
                existed
                    ? $"↺ {Path.GetFileName(file.FileName)} actualizado"
                    : $"✓ {Path.GetFileName(file.FileName)} creado");
        }
    }
}
