using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn.Builders;

/// <summary>
/// Construye el método Configure de IEntityTypeConfiguration.
/// </summary>
public sealed class ConfigureMethodBuilder
{
    public MethodDeclarationSyntax Build(string entityName)
    {
        var code =
$@"public void Configure(EntityTypeBuilder<{entityName}> builder)
{{
}}";

        return (MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(code)!;
    }
}
