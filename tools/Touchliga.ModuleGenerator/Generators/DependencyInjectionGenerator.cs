using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class DependencyInjectionGenerator : IGenerator
{
    private readonly ISolutionLocator _locator = new SolutionLocator();

    private readonly IFileService _fileService = new FileService();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var file = Path.Combine(
            _locator.Persistence,
            "DependencyInjection.cs");

        if (!File.Exists(file))
            throw new FileNotFoundException(file);

        var source = await File.ReadAllTextAsync(file);

        var tree = CSharpSyntaxTree.ParseText(source);

        var root = await tree.GetRootAsync();

        var method = root
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .First(m => m.Identifier.Text == "AddPersistence");

        if (method.Body is null)
            throw new InvalidOperationException(
                "No se encontró el cuerpo del método AddPersistence.");

        var repositoryRegistration =
            SyntaxFactory.ParseStatement(
                $"services.AddScoped<I{module.Entity}Repository, {module.Entity}Repository>();");

        var statements = method.Body.Statements.ToList();

        // Ya existe el registro
        if (statements.Any(s =>
                s.ToString().Contains(
                    $"I{module.Entity}Repository")))
        {
            Console.WriteLine(
                $"✓ {module.Entity}Repository ya registrado.");

            return;
        }

        // Insertar antes del UnitOfWork
        var insertIndex = statements.FindIndex(s =>
            s.ToString().Contains(
                "IUnitOfWork"));

        if (insertIndex < 0)
            insertIndex = statements.Count;

        statements.Insert(
            insertIndex,
            (StatementSyntax)repositoryRegistration);

        var newBody = SyntaxFactory.Block(statements);

        var newMethod = method.WithBody(newBody);

        root = root.ReplaceNode(method, newMethod);

        await _fileService.WriteFileAsync(
            file,
            root.NormalizeWhitespace().ToFullString());

        Console.WriteLine(
            $"✓ DependencyInjection actualizado ({module.Entity})");
    }
}
