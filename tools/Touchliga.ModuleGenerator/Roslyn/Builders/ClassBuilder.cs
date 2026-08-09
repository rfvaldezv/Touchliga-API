using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn.Builders;

/// <summary>
/// Construye declaraciones de clases.
/// </summary>
public sealed class ClassBuilder
{
    public ClassDeclarationSyntax Build(
        string className,
        string? baseClass = null)
    {
        var declaration = SyntaxFactory.ClassDeclaration(className)
            .AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                SyntaxFactory.Token(SyntaxKind.SealedKeyword));

        if (!string.IsNullOrWhiteSpace(baseClass))
        {
            declaration = declaration.WithBaseList(
                SyntaxFactory.BaseList(
                    SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                        SyntaxFactory.SimpleBaseType(
                            SyntaxFactory.ParseTypeName(baseClass)))));
        }

        return declaration;
    }
}
