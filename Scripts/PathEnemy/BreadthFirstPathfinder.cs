using System.Collections.Generic;
using UnityEngine;
public class BreadthFirstPathfinder : IPathfinder
{
    public List<GameObject> FindPath(Graph graph, Node start, Node end)
    {
        if (start == null || end == null) return null;

        var queue = new Queue<Node>();
        var cameFrom = new Dictionary<Node, Node>();
        var visited = new HashSet<Node>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == end) break;

            foreach (var edge in current.Edges)
            {
                var neighbor = edge.GetOther(current);
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        if (!cameFrom.ContainsKey(end) && start != end) return null;

        var path = new List<GameObject>();
        var node = end;
        while (node != null)
        {
            path.Add(node.Owner);
            node = cameFrom.ContainsKey(node) ? cameFrom[node] : null;
        }
        path.Reverse();

        return path.Count > 0 && path[0] == start.Owner ? path : null;
    }
}