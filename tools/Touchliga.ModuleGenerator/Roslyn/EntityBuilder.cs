using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn.Builders;
using Touchliga.ModuleGenerator.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Touchliga.ModuleGenerator.Roslyn;

public sealed class EntityBuilder
{
    private readonly string _namespace;
    private readonly string _className;

    private readonly List<MemberDeclarationSyntax> _members = [];

    private readonly PropertyBuilder _propertyBuilder = new();
    private readonly ConstructorBuilder _constructorBuilder = new();
    private readonly RelationBuilder _relationBuilder = new();

    private string? _baseClass;

    // NUEVO
    private bool _generatePrivateConstructor = true;
    private bool _generatePublicConstructor;

    public EntityBuilder(
        string @namespace,
        string className)
    {
        _namespace = @namespace;
        _className = className;
    }

    public EntityBuilder Inherits(string baseClass)
    {
        _baseClass = baseClass;
        return this;
    }

    // NUEVO
    public EntityBuilder WithPrivateConstructor()
    {
        _generatePrivateConstructor = true;
        _generatePublicConstructor = false;
        return this;
    }

    // NUEVO
    public EntityBuilder WithPublicConstructor()
    {
        _generatePrivateConstructor = false;
        _generatePublicConstructor = true;
        return this;
    }

    public EntityBuilder AddProperty(
        string type,
        string name)
    {
        _members.Add(
            _propertyBuilder.Build(type, name));

        return this;
    }

    public EntityBuilder AddProperties(
        IEnumerable<FieldDefinition> fields)
    {
        var mapper = new TypeMapper();

        foreach (var field in fields)
        {
            AddProperty(
                mapper.GetCSharpType(field.Type),
                field.Name);
        }

        return this;
    }

    public EntityBuilder AddRelations(
        IEnumerable<RelationDefinition> relations)
    {
        foreach (var relation in relations)
        {
            _members.AddRange(
                _relationBuilder.Build(
                    relation.Entity,
                    relation.ForeignKey));
        }

        return this;
    }

    public CompilationUnitSyntax Build()
    {
        var usingBuilder = new UsingBuilder();
        var classBuilder = new ClassBuilder();
        var namespaceBuilder = new NamespaceBuilder(_namespace);

        var entityClass = classBuilder.Build(
            _className,
            _baseClass);

        if (_members.Count > 0)
        {
            entityClass = entityClass.AddMembers(
                _members.ToArray());
        }

        if (_generatePrivateConstructor)
        {
            entityClass = entityClass.AddMembers(
                _constructorBuilder.Private(_className));
        }

        if (_generatePublicConstructor)
        {
            entityClass = entityClass.AddMembers(
                _constructorBuilder.Public(_className));
        }

        var entityNamespace = namespaceBuilder.Build(entityClass);

        return SyntaxFactory
            .CompilationUnit()
            .AddUsings(
                usingBuilder.Build("Touchliga.Domain.Common"))
            .AddMembers(entityNamespace)
            .NormalizeWhitespace();
    }
}
