using Touchliga.ModuleGenerator.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn;

public sealed class ConfigurationClassBuilder
{
    private readonly ModuleDefinition _module;

    public ConfigurationClassBuilder(ModuleDefinition module)
    {
        _module = module;
    }

    public ClassDeclarationSyntax Build()
    {
        return SyntaxFactory
            .ClassDeclaration($"{_module.Entity}Configuration")
            .AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                SyntaxFactory.Token(SyntaxKind.SealedKeyword));
    }
}
