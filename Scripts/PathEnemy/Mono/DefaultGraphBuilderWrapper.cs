using UnityEngine;


public class DefaultGraphBuilderWrapper : MonoBehaviour, IGraphBuilder
{
    private DefaultGraphBuilder _inner;

    private DefaultGraphBuilder Inner => _inner ?? (_inner = new DefaultGraphBuilder());

    public Graph BuildGraph(NodeComponent[] allNodes) => Inner.BuildGraph(allNodes);
}