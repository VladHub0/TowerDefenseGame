using System.Collections.Generic;

public class DefaultGraphBuilder : IGraphBuilder
{
    public Graph BuildGraph(NodeComponent[] allNodes)
    {
        var graph = new Graph();
        var map = new Dictionary<NodeComponent, Node>();

        foreach (var comp in allNodes)
        {
            map[comp] = graph.AddNode(comp.gameObject);
        }

        foreach (var comp in allNodes)
        {
            var current = map[comp];
            foreach (var neighborComp in comp.Neighbors)
            {
                if (map.TryGetValue(neighborComp, out var neighbor))
                {
                    graph.AddEdge(current, neighbor);
                }
            }
        }

        return graph;
    }
}