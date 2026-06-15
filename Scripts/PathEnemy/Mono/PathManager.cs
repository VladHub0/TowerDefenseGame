using UnityEngine;
using System.Collections.Generic;

public class PathManager : MonoBehaviour, IPathProvider
{
    [SerializeField] private GameObject _endPoint;
    [SerializeField] private GameObject[] _startPoints;

    [Header("Strategies")]
    [SerializeField] private MonoBehaviour _pathfinderBehaviour;
    [SerializeField] private MonoBehaviour _graphBuilderBehaviour;

    private IPathfinder _pathfinder;
    private IGraphBuilder _graphBuilder;
    private Graph _graph;
    private Dictionary<GameObject, IPath> _paths = new Dictionary<GameObject, IPath>();

    public IReadOnlyDictionary<GameObject, IPath> Paths => _paths;

    private void Start()
    {
        _pathfinder = _pathfinderBehaviour as IPathfinder;
        _graphBuilder = _graphBuilderBehaviour as IGraphBuilder;

        if (_pathfinder == null) Debug.LogError("Pathfinder behaviour missing IPathfinder", this);
        if (_graphBuilder == null) Debug.LogError("GraphBuilder behaviour missing IGraphBuilder", this);

        BuildGraph();
        BuildPaths();
    }

    private void BuildGraph()
    {
        var allNodes = FindObjectsByType<NodeComponent>(FindObjectsSortMode.None);
        Debug.Log($"BuildGraph: найдено NodeComponent: {allNodes.Length}");

        _graph = _graphBuilder.BuildGraph(allNodes);
        Debug.Log($"BuildGraph: граф содержит {_graph.Nodes.Count} узлов");

        EnsureNodeInGraph(_endPoint);
        foreach (var sp in _startPoints) EnsureNodeInGraph(sp);
    }

    private void EnsureNodeInGraph(GameObject go)
    {
        if (go == null) return;
        if (!_graph.Nodes.Exists(n => n.Owner == go))
            _graph.AddNode(go);
    }

    private void BuildPaths()
    {
        var endNode = _graph.Nodes.Find(n => n.Owner == _endPoint);
        if (endNode == null) return;

        foreach (var startGO in _startPoints)
        {
            if (startGO == null) continue;
            var startNode = _graph.Nodes.Find(n => n.Owner == startGO);
            if (startNode == null) continue;

            var nodeList = _pathfinder.FindPath(_graph, startNode, endNode);
            if (nodeList != null)
                _paths[startGO] = new Path(nodeList);
            else
                Debug.LogWarning($"Path from {startGO.name} to {_endPoint.name} not found", this);
        }
    }

    public IPath GetPathForSpawn(GameObject spawnPoint)
    {
        if (_paths.TryGetValue(spawnPoint, out var path))
        {
            var nodes = new List<GameObject>(path.Nodes);
            return new Path(nodes);
        }
        return null;
    }

#if UNITY_EDITOR
    public void BuildPathsInEditor()
    {
        if (_paths != null && _paths.Count > 0)
            return;

        _pathfinder = _pathfinder ?? (_pathfinderBehaviour as IPathfinder);
        _graphBuilder = _graphBuilder ?? (_graphBuilderBehaviour as IGraphBuilder);

        if (_pathfinder == null)
        {
            Debug.LogError("Pathfinder behaviour missing IPathfinder", this);
            return;
        }
        if (_graphBuilder == null)
        {
            Debug.LogError("GraphBuilder behaviour missing IGraphBuilder", this);
            return;
        }

        BuildGraph();
        BuildPaths();
    }
#endif
}