using System.Collections.Generic;
using UnityEngine;

    public class Graph
    {
        public List<Node> Nodes { get; } = new List<Node>();

        public Node AddNode(GameObject owner)
        {
            var node = new Node(owner);
            Nodes.Add(node);
            return node;
        }

        public void AddEdge(Node a, Node b)
        {
            float distance = Vector3.Distance(a.Owner.transform.position, b.Owner.transform.position);
            var edge = new Edge(a, b, distance);
            a.Edges.Add(edge);
            b.Edges.Add(edge);
        }
    }

    public class Node
    {
        public GameObject Owner { get; }
        public List<Edge> Edges { get; } = new List<Edge>();

        public Node(GameObject owner) => Owner = owner;
    }

    public class Edge
    {
        public Node A { get; }
        public Node B { get; }
        public float Distance { get; }

        public Edge(Node a, Node b, float distance)
        {
            A = a;
            B = b;
            Distance = distance;
        }

        public Node GetOther(Node node) => node == A ? B : node == B ? A : null;
    }
