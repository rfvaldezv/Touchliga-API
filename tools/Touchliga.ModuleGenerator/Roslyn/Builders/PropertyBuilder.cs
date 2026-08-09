using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn.Builders;

/// <summary>
/// Construye una propiedad autoimplementada.
/// Ejemplo:
///
/// public string Codigo { get; private set; }
/// </summary>
public sealed class PropertyBuilder
{
    public PropertyDeclarationSyntax Build(
        string type,
        string name,
        bool hasPrivateSetter = true)
    {
        var property =
            SyntaxFactory.PropertyDeclaration(
                SyntaxFactory.ParseTypeName(type),
                SyntaxFactory.Identifier(name))
            .AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        var getAccessor =
            SyntaxFactory.AccessorDeclaration(
                SyntaxKind.GetAccessorDeclaration)
            .WithSemicolonToken(
                SyntaxFactory.Token(SyntaxKind.SemicolonToken));

        var setAccessor =
            SyntaxFactory.AccessorDeclaration(
                SyntaxKind.SetAccessorDeclaration)
            .WithSemicolonToken(
                SyntaxFactory.Token(SyntaxKind.SemicolonToken));

        if (hasPrivateSetter)
        {
            setAccessor =
                setAccessor.AddModifiers(
                    SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
        }

        property =
            property.AddAccessorListAccessors(
                getAccessor,
                setAccessor);

        return property;
    }
}
