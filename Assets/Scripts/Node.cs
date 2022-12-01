using UnityEngine;

public class Node
{
    private Node _parentNode;
    private Vector2 _position;
    public Vector2Int Indexes { get; private set; }

    public bool IsReachable { get; set; }

    public float PrioF;
    public float PrioG;
    public float PrioTotal => (PrioF + PrioG);
    public Node(Vector2 position, Vector2Int indexes)
    {
        _parentNode = null;
        _position = position;
        this.Indexes = indexes;
        IsReachable = true;
    }
    public Vector2 GetVector2() => _position;
    public void SetParentNode(Node node) => _parentNode = node;

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
