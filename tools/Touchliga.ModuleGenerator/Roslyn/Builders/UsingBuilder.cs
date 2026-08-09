using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn.Builders;

/// <summary>
/// Construye directivas using.
/// </summary>
public sealed class UsingBuilder
{
    public UsingDirectiveSyntax Build(string namespaceName)
    {
        return SyntaxFactory.UsingDirective(
            SyntaxFactory.ParseName(namespaceName));
    }
}
