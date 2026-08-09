using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn.Builders;

/// <summary>
/// Construye una relación ManyToOne.
///
/// Genera:
///
/// public long LigaId { get; private set; }
///
/// public Liga Liga { get; private set; } = null!;
/// </summary>
public sealed class RelationBuilder
{
    private readonly PropertyBuilder _propertyBuilder = new();

    public IEnumerable<MemberDeclarationSyntax> Build(
        string entity,
        string foreignKey)
    {
        yield return _propertyBuilder.Build(
            "long",
            foreignKey);

        var navigation =
            SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.ParseTypeName(entity),
                    SyntaxFactory.Identifier(entity))
                .AddModifiers(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(
                            SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(
                            SyntaxFactory.Token(
                                SyntaxKind.SemicolonToken)),

                    SyntaxFactory.AccessorDeclaration(
                            SyntaxKind.SetAccessorDeclaration)
                        .AddModifiers(
                            SyntaxFactory.Token(
                                SyntaxKind.PrivateKeyword))
                        .WithSemicolonToken(
                            SyntaxFactory.Token(
                                SyntaxKind.SemicolonToken)))
                .WithInitializer(
                    SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.PostfixUnaryExpression(
                            SyntaxKind.SuppressNullableWarningExpression,
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression))))
                .WithSemicolonToken(
                    SyntaxFactory.Token(
                        SyntaxKind.SemicolonToken));

        yield return navigation;
    }
}
