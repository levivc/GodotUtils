using System.Collections.Generic;
using System.Text;

namespace Levivc.GodotUtils.SourceGenerator;

internal class SceneSource
{
    public static string Build(SceneDataModel sceneData)
    {
        (string usings, string nodes) = GetNodeAndUsingLines(sceneData);

        var sourceBuilder = new StringBuilder();

        if (usings.Length > 0)
            sourceBuilder.AppendLine(usings);

        if (sceneData.Namespace.Length > 0)
        {
            sourceBuilder.AppendLine($$"""
                namespace {{sceneData.Namespace}};

                """);
        }

        sourceBuilder.AppendLine($$"""
            public partial class {{sceneData.Identifier}}
            {
                private void GetNodes()
                {
            """);

        sourceBuilder.Append(nodes);

        sourceBuilder.AppendLine("""
                }
            }
            """);

        return sourceBuilder.ToString();
    }

    private static (string usings, string nodes)
        GetNodeAndUsingLines(SceneDataModel sceneData)
    {
        var usingsBuilder = new StringBuilder();
        var nodesBuilder = new StringBuilder();

        var distinctUsings = new HashSet<string>();

        if (sceneData.Namespace.Length > 0)
            distinctUsings.Add(sceneData.Namespace);

        foreach (var node in sceneData.NodesData)
        {
            if (node.TypeNamespace.Length > 0 && distinctUsings.Add(node.TypeNamespace))
                usingsBuilder.AppendLine($"using {node.TypeNamespace};");

            nodesBuilder.AppendLine(
                $"        {node.Identifier} = GetNode<{node.Type}>(\"{node.Path}\");");
        }

        return (usingsBuilder.ToString(), nodesBuilder.ToString());
    }
}
