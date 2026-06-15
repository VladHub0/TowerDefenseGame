using UnityEngine;
using System.Collections.Generic;


public class BreadthFirstPathfinderWrapper : MonoBehaviour, IPathfinder
{
    private BreadthFirstPathfinder _inner;

    private BreadthFirstPathfinder Inner => _inner ?? (_inner = new BreadthFirstPathfinder());

    public List<GameObject> FindPath(Graph graph, Node start, Node end) =>
        Inner.FindPath(graph, start, end);
}