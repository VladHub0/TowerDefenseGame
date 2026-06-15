using System.Collections.Generic;
using UnityEngine;

public class Path : IPath
{
    private List<GameObject> _nodes;
    private int _currentIndex;

    public IReadOnlyList<GameObject> Nodes => _nodes;

    public Path(List<GameObject> nodes)
    {
        _nodes = nodes ?? new List<GameObject>();
        _currentIndex = 0;
    }

    public GameObject GetCurrentNode() =>
        _currentIndex < _nodes.Count ? _nodes[_currentIndex] : null;

    public GameObject GetNextNode()
    {
        _currentIndex++;
        return _currentIndex < _nodes.Count ? _nodes[_currentIndex] : null;
    }

    public bool IsFinished() => _currentIndex >= _nodes.Count - 1;

    public void Reset() => _currentIndex = 0;
}