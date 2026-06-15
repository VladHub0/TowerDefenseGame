using UnityEngine;
using System.Collections.Generic;

    public class NodeComponent : MonoBehaviour
    {
        [SerializeField] private List<NodeComponent> _neighbors = new List<NodeComponent>();

        public IReadOnlyList<NodeComponent> Neighbors => _neighbors;
        public Vector3 Position => transform.position;
    }