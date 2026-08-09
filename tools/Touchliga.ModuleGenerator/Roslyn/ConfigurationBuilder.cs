using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn.Builders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn;

public sealed class ConfigurationBuilder
{
    private readonly ModuleDefinition _module;

    public ConfigurationBuilder(ModuleDefinition module)
    {
        _module = module;
    }

    public CompilationUnitSyntax Build()
    {
        var usingBuilder = new UsingBuilder();

        var namespaceBuilder =
            new NamespaceBuilder("Touchliga.Persistence.Configurations");

        var classDeclaration =
            SyntaxFactory.ClassDeclaration($"{_module.Entity}Configuration")
                .AddModifiers(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.SealedKeyword));

        return SyntaxFactory
            .CompilationUnit()
            .AddUsings(
                usingBuilder.Build("Touchliga.Domain.Entities"),
                usingBuilder.Build("Microsoft.EntityFrameworkCore"),
                usingBuilder.Build("Microsoft.EntityFrameworkCore.Metadata.Builders"))
            .AddMembers(
                namespaceBuilder.Build(classDeclaration))
            .NormalizeWhitespace();
    }
}
