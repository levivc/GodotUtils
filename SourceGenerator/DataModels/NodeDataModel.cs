namespace Levivc.GodotUtils.SourceGenerator;

internal record struct NodeDataModel
{
    public readonly string Identifier;
    public readonly string Type;
    public readonly string TypeNamespace;
    public readonly string? Path;

    public NodeDataModel(string identifier, string type, string typeNamespace, string? path)
    {
        Identifier = identifier;
        Type = type;
        TypeNamespace = typeNamespace;
        Path = path;
    }
}