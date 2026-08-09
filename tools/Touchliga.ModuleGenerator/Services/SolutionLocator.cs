namespace Touchliga.ModuleGenerator.Services;

public sealed class SolutionLocator : ISolutionLocator
{
    public string Root { get; }

    public string Src => Path.Combine(Root, "src");

    public string Domain => Path.Combine(Src, "Touchliga.Domain");

    public string Application => Path.Combine(Src, "Touchliga.Application");

    public string Persistence => Path.Combine(Src, "Touchliga.Persistence");

    public string Api => Path.Combine(Src, "Touchliga.Api");

    public string Infrastructure => Path.Combine(Src, "Touchliga.Infrastructure");

    public string Contracts => Path.Combine(Src, "Touchliga.Contracts");

    public string Shared => Path.Combine(Src, "Touchliga.Shared");

    public string Tests => Path.Combine(Root, "tests");

    public SolutionLocator()
    {
        Root = FindRoot();
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory != null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, "src")) &&
                Directory.Exists(Path.Combine(directory.FullName, "tools")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "No fue posible localizar la solución Touchliga.");
    }
}
