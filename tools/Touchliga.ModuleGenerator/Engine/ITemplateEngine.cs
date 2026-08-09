namespace Touchliga.ModuleGenerator.Engine;

public interface ITemplateEngine
{
    string Render(
        string template,
        Dictionary<string, string> values);
}
