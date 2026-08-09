using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Generators;

namespace Touchliga.ModuleGenerator.Commands;

public sealed class NewCatalogCommand : ICommand
{
    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine(
                "Uso: fmg new catalog <Entidad>");

            return -1;
        }

        var module =
            ModuleFactory.CreateCatalog(args[2]);

        var generator = new CatalogGenerator();

        await generator.GenerateAsync(module);

        return 0;
    }
}
