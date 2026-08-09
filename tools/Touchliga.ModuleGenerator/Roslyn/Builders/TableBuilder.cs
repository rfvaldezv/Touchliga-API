using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn.Builders;

/// <summary>
/// Construye:
///
/// builder.ToTable("Tabla","Esquema");
/// </summary>
public sealed class TableBuilder
{
    public StatementSyntax Build(
        string table,
        string schema)
    {
        return SyntaxFactory.ParseStatement(
            $"builder.ToTable(\"{table}\", \"{schema}\");");
    }
}
