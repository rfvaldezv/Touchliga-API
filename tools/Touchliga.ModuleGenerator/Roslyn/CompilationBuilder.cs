using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn;

/// <summary>
/// Construye una unidad de compilación (archivo .cs completo).
/// </summary>
public sealed class CompilationBuilder
{
    public CompilationUnitSyntax Create()
    {
        return SyntaxFactory.CompilationUnit();
    }
}
