namespace Touchliga.ModuleGenerator.Services;

public sealed class TemplateService : ITemplateService
{
    public string Load(string template)
    {
        if (!File.Exists(template))
        {
            throw new FileNotFoundException(
                $"No se encontró la plantilla: {template}");
        }

        return File.ReadAllText(template);
    }
}
