using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Room room;
    private Vector2 position;

    private List<NodeConnection> connections;

    public Node(Vector2 position)
    {
        this.position = position;
        connections = new List<NodeConnection>();
    }

    public int GetDistance(Node node)
    {
        int x = (int) Mathf.Abs(node.GetPosition().x - position.x);
        int y = (int) Mathf.Abs(node.GetPosition().y - position.y);

        return x + y;
    }

    public int GetDistance(Vector2 position)
    {
        int x = (int) Mathf.Abs(position.x - this.position.x);
        int y = (int) Mathf.Abs(position.y - this.position.y);

        return x + y;
    }

    public int GetDirectDistance(Node node)
    {
        return (int) Vector2.Distance(node.GetPosition(), position);
    }

    public int GetDirectDistance(Vector2 position)
    {
        return (int) Vector2.Distance(position, this.position);
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public void AddConnection(NodeConnection newConnection)
    {
        connections.Add(newConnection);
    }

    public List<NodeConnection> GetConnections()
    {
        return connections;
    }
}

public class NodeConnection
{
    Node a;
    Node b;

    public NodeConnection(Node nodeA, Node nodeB)
    {
        a = nodeA;
        b = nodeB;
    }

    public Node[] GetNodes()
    {
        return new Node[2] { a, b };
    }

    //Future set up for more than 2 nodes in 1 connection?
    public Node GetNode(int nodeIndex)
    {
        switch (nodeIndex)
        {
            case 0:
                return a;
            case 1:
                return b;
            default:
                return a;
        }
    }

    public int GetLength()
    {
        return a.GetDistance(b);
    }
}