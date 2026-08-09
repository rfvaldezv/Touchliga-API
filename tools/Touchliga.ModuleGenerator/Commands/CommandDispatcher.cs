namespace Touchliga.ModuleGenerator.Commands;

public sealed class CommandDispatcher
{
    private readonly Dictionary<string, Func<string[], Task<int>>> _commands;

    public CommandDispatcher()
    {
        _commands = new(StringComparer.OrdinalIgnoreCase)
        {
            ["new"] = ExecuteNewAsync,
            ["help"] = _ => Task.FromResult(ShowHelp()),
            ["--help"] = _ => Task.FromResult(ShowHelp()),
            ["-h"] = _ => Task.FromResult(ShowHelp())
        };
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0)
            return ShowHelp();

        if (_commands.TryGetValue(args[0], out var command))
            return await command(args);

        return UnknownCommand(args[0]);
    }

    private static async Task<int> ExecuteNewAsync(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Debe especificar el tipo de elemento.");
            return -1;
        }

        var newCommands = new Dictionary<string, Func<Task<int>>>(
            StringComparer.OrdinalIgnoreCase)
        {
            ["catalog"] = () => new NewCatalogCommand().ExecuteAsync(args)
        };

        if (newCommands.TryGetValue(args[1], out var command))
            return await command();

        return UnknownCommand(args[1]);
    }

    private static int ShowHelp()
    {
        Console.WriteLine();
        Console.WriteLine("Touchliga Module Generator");
        Console.WriteLine();

        Console.WriteLine("Uso:");
        Console.WriteLine();

        Console.WriteLine("  fmg new catalog Liga");
        Console.WriteLine("  fmg help");
        Console.WriteLine();

        return 0;
    }

    private static int UnknownCommand(string command)
    {
        Console.WriteLine($"Comando '{command}' no reconocido.");
        Console.WriteLine();
        Console.WriteLine("Use:");
        Console.WriteLine("  fmg help");

        return -1;
    }
}
