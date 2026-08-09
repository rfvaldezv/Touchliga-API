using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn.Builders;

/// <summary>
/// Construye un namespace y permite agregar miembros.
/// </summary>
public sealed class NamespaceBuilder
{
    private readonly NamespaceDeclarationSyntax _namespace;

    public NamespaceBuilder(string namespaceName)
    {
        _namespace = SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.ParseName(namespaceName));
    }

    public NamespaceDeclarationSyntax Build(params MemberDeclarationSyntax[] members)
    {
        return _namespace.AddMembers(members);
    }
}
