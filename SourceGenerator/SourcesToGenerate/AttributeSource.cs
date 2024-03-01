namespace Levivc.GodotUtils.SourceGenerator;

internal static class AttributeSource
{
    public const string Node = $$"""
        using System;

        [AttributeUsage(AttributeTargets.Field)]
        public class NodeAttribute : Attribute
        {
            public string Path;

            public NodeAttribute(string path)
            {
                Path = path;
            }
        }

        """;

    public const string Scene = $$"""
        using System;

        [AttributeUsage(AttributeTargets.Class)]
        public class SceneAttribute : Attribute
        {
        }

        """;
}
