using System.Collections.Generic;
using UnityEngine;
public interface IPathfinder
{
    List<GameObject> FindPath(Graph graph, Node start, Node end);
}