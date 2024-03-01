namespace Levivc.GodotUtils.SourceGenerator;

internal record struct SceneDataModel
{
    public readonly string Identifier;
    public readonly string Namespace;
    public readonly EquatableArray<NodeDataModel> NodesData;

    public SceneDataModel(
        string identifier, string @namespace, NodeDataModel[] nodesData)
    {
        Identifier = identifier;
        Namespace = @namespace;
        NodesData = new(nodesData);
    }
}
