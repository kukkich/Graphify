namespace Graphify.Geometry.Attaching;

public interface IDependencyNode
{
    public string Id { get; }
    public void Update();
}
