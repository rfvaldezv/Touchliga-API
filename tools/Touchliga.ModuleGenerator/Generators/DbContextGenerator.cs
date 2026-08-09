using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class DbContextGenerator : IGenerator
{
    private readonly ISolutionLocator _locator = new SolutionLocator();
    private readonly IFileService _fileService = new FileService();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var file = Path.Combine(
            _locator.Persistence,
            "Context",
            "TouchligaDbContext.cs");

        if (!File.Exists(file))
            throw new FileNotFoundException(file);

        var source = await File.ReadAllTextAsync(file);

        var tree = CSharpSyntaxTree.ParseText(source);

        var root = await tree.GetRootAsync();

        var dbContext = root
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .First(c => c.Identifier.Text == "TouchligaDbContext");

        var propertyName = module.EntityPlural;

        if (dbContext.Members
            .OfType<PropertyDeclarationSyntax>()
            .Any(p => p.Identifier.Text == propertyName))
        {
            Console.WriteLine(
                $"✓ DbSet {propertyName} ya existe.");

            return;
        }

        var property =
            SyntaxFactory.ParseMemberDeclaration(
                $"public DbSet<{module.Entity}> {propertyName} => Set<{module.Entity}>();")
            as PropertyDeclarationSyntax;

        if (property is null)
            throw new InvalidOperationException(
                "No fue posible crear el DbSet.");

        var members = dbContext.Members.ToList();

        var insertIndex = members.FindIndex(m =>
            m is MethodDeclarationSyntax method &&
            method.Identifier.Text == "OnModelCreating");

        if (insertIndex < 0)
            insertIndex = members.Count;

        members.Insert(insertIndex, property);

        var newClass = dbContext.WithMembers(
            SyntaxFactory.List(members));

        root = root.ReplaceNode(dbContext, newClass);

        await _fileService.WriteFileAsync(
            file,
            root.NormalizeWhitespace().ToFullString());

        Console.WriteLine(
            $"✓ DbContext actualizado ({module.Entity})");
    }
}
