namespace Touchliga.ModuleGenerator.Services;

public interface ISolutionLocator
{
    string Root { get; }

    string Src { get; }

    string Domain { get; }

    string Application { get; }

    string Persistence { get; }

    string Api { get; }

    string Infrastructure { get; }

    string Contracts { get; }

    string Shared { get; }

    string Tests { get; }
}
