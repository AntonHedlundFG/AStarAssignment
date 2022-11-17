using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Node _parentNode;
    private Vector2 _position;
    public Vector2Int indexes { get; private set; }
    public bool isReachable { get; set; }
    public float prioStart;
    public float prioEnd;
    public float prioTotal => prioStart + prioEnd;
    public Node(Vector2 position, Vector2Int indexes)
    {
        _parentNode = null;
        _position = position;
        this.indexes = indexes;
        isReachable = true;
    }
    public Node(Vector2 position, Vector2Int indexes,  bool isReachable)
    {
        _position = position;
        this.indexes = indexes;
        this.isReachable = isReachable;
    }
    public Vector2 GetVector2() => _position;
    public void SetParentNode(Node node) => _parentNode = node;
    public Node GetParentNode() => _parentNode;

    public Node[] GetParentPath()
    {
        if (_parentNode == null) { return new Node[] { this }; }
        Node[] parentPath = _parentNode.GetParentPath();
        Node[] newPath = new Node[parentPath.Length + 1];
        newPath[0] = this; 
        parentPath.CopyTo(newPath, 1);
        return newPath;
    }

}
