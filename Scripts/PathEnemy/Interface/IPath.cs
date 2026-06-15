using System.Collections.Generic;
using UnityEngine;

public interface IPath
{
    GameObject GetCurrentNode();
    GameObject GetNextNode();
    bool IsFinished();
    void Reset();
    IReadOnlyList<GameObject> Nodes { get; }
}