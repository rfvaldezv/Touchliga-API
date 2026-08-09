using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn.Builders;

/// <summary>
/// Construye constructores.
/// </summary>
public sealed class ConstructorBuilder
{
    public ConstructorDeclarationSyntax Private(string className)
    {
        return SyntaxFactory
            .ConstructorDeclaration(className)
            .AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PrivateKeyword))
            .WithBody(
                SyntaxFactory.Block());
    }

    public ConstructorDeclarationSyntax Public(string className)
    {
        return SyntaxFactory
            .ConstructorDeclaration(className)
            .AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .WithBody(
                SyntaxFactory.Block());
    }
}
