using UnityEngine;
using System.Collections.Generic;

public class PathDebugRenderer : MonoBehaviour
{
    [SerializeField] private PathManager _pathManager;
    [SerializeField] private Color[] _pathColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta };
    [SerializeField] private float _nodeRadius = 0.2f;
    [SerializeField] private bool _drawNodes = true;

    private void OnDrawGizmos()
    {
        if (_pathManager == null) return;

#if UNITY_EDITOR
        _pathManager.BuildPathsInEditor();
#endif

        var paths = _pathManager.Paths;
        if (paths == null || paths.Count == 0) return;

        int colorIndex = 0;
        foreach (var kvp in paths)
        {
            var path = kvp.Value;
            if (path == null) continue;

            var nodes = path.Nodes;
            if (nodes == null || nodes.Count < 2) continue;

            Gizmos.color = _pathColors[colorIndex % _pathColors.Length];

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                if (nodes[i] != null && nodes[i + 1] != null)
                {
                    Gizmos.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position);
                }
            }

            if (_drawNodes)
            {
                Gizmos.color = Color.white;
                foreach (var node in nodes)
                {
                    if (node != null)
                        Gizmos.DrawSphere(node.transform.position, _nodeRadius);
                }
            }

            colorIndex++;
        }
    }
}