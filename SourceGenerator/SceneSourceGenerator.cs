using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Levivc.GodotUtils.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class SceneSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource("SceneAttribute.g.cs", AttributeSource.Scene));
        context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource("NodeAttribute.g.cs", AttributeSource.Node));

        var sceneDataProvider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "SceneAttribute",
                predicate: static (sn, _) => true,
                transform: static (ctx, _) =>
                    GetSceneGenerationData((INamedTypeSymbol)ctx.TargetSymbol));

        context.RegisterSourceOutput(sceneDataProvider, (context, sceneData) =>
            context.AddSource($"{sceneData.Identifier}.g.cs", SceneSource.Build(sceneData)));
    }

    private static SceneDataModel GetSceneGenerationData(INamedTypeSymbol scene)
    {
        var nodesData = new List<NodeDataModel>();

        foreach (var sceneMember in scene.GetMembers())
        {
            if (sceneMember is not IFieldSymbol sceneField)
                continue;

            foreach (var attribute in sceneMember.GetAttributes())
            {
                if (attribute.AttributeClass is null ||
                    !attribute.AttributeClass.MetadataName.Equals(
                        "NodeAttribute", StringComparison.Ordinal))
                {
                    continue;
                }

                string? path = (string?)attribute.ConstructorArguments[0].Value;

                nodesData.Add(new NodeDataModel(
                    sceneField.MetadataName,
                    sceneField.Type.MetadataName,
                    sceneField.Type.ContainingNamespace.ToDisplayString(
                        SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(
                            SymbolDisplayGlobalNamespaceStyle.Omitted)),
                    path));
            }
        }

        return new SceneDataModel(
            scene.MetadataName,
            scene.ContainingNamespace.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(
                    SymbolDisplayGlobalNamespaceStyle.Omitted)),
            nodesData.ToArray());
    }
}
