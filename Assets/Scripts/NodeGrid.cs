using System;
using UnityEngine;

public class NodeGrid
{
    private Vector2 _xConstraint;
    private Vector2 _yConstraint;
    public int GridSize { get; private set; }
    public Node[][] Grid { get; private set; }
    public NodeGrid(Vector2 xConstraint, Vector2 yConstraint, int gridSize)
    {
        _xConstraint = InvertIfXGTY(xConstraint);
        _yConstraint = InvertIfXGTY(yConstraint);
        this.GridSize = Mathf.Max(gridSize, 10);
        GenerateGrid();
    }

    public void SetupPriorities(Vector2Int startIndex, Vector2Int endIndex)
    {
        if (!AStarPathfind.IndexWithinGrid(GridSize, startIndex) || !AStarPathfind.IndexWithinGrid(GridSize, endIndex)) { return; }
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                Grid[i][j].PrioF = float.MaxValue;
                Grid[i][j].PrioG = Vector2.Distance(Grid[i][j].GetVector2(), Grid[endIndex.x][endIndex.y].GetVector2());
            }
        }
    }

    private Vector2 InvertIfXGTY(Vector2 vect)
    {
        return (vect.x <= vect.y) ? vect : new Vector2(vect.y, vect.x);
    }
    private void GenerateGrid()
    {
        Grid = new Node[GridSize][];
        for (int i = 0; i < GridSize; i++)
        {
            Grid[i] = new Node[GridSize];
            for (int j = 0; j < GridSize; j++)
            {
                Grid[i][j] = new Node(GetVector2FromGridPos(i, j), new Vector2Int(i, j));
                CheckCollisionForNode(Grid[i][j]);
            }
        }
    }
    public Vector2 GetVector2FromGridPos(int x, int y)
    {
        x = Mathf.Clamp(x, 0, GridSize - 1);
        y = Mathf.Clamp(y, 0, GridSize - 1);
        float gridWidth = _xConstraint.y - _xConstraint.x;
        float gridHeight = _yConstraint.y - _yConstraint.x;
        float nodeWidth = gridWidth / GridSize;
        float nodeHeight = gridHeight / GridSize;
        float vectX = _xConstraint.x + (x + 0.5f) * nodeWidth;
        float vectY = _yConstraint.x + (y + 0.5f) * nodeHeight;
        return new Vector2(vectX, vectY);
    }
    public Vector2Int GetGridPosFromVector2(Vector2 position)
    {
        if (position.x < _xConstraint.x || position.x >= _xConstraint.y 
            || position.y < _yConstraint.x || position.y >= _yConstraint.y)
        {
            throw new ArgumentOutOfRangeException();
        }
        float gridWidth = _xConstraint.y - _xConstraint.x;
        float gridHeight = _yConstraint.y - _yConstraint.x;
        float nodeWidth = gridWidth / GridSize;
        float nodeHeight = gridHeight / GridSize;
        float xPos = (position.x - _xConstraint.x) / nodeWidth;
        float yPos = (position.y - _yConstraint.x) / nodeHeight;
        return new Vector2Int((int)xPos, (int)yPos);
    }
    public Vector2 GetAlignedVector2(Vector2 inVector)
    {
        Vector2Int coords = GetGridPosFromVector2(inVector);
        return GetVector2FromGridPos(coords.x, coords.y);
    }
    public void ResetGrid()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                Grid[i][j].SetParentNode(null);
                Grid[i][j].PrioF = float.MaxValue;
                Grid[i][j].PrioG = 0;
            }
        }
    }
    private void CheckCollisionForNode(Node node)
    {
        LayerMask layerMask = ~LayerMask.GetMask("Ground");
        if(Physics.BoxCast(new Vector3(node.GetVector2().x, node.GetVector2().y, -100), GetNodeExtents() / 2, Vector3.forward, Quaternion.identity, float.MaxValue, layerMask))
        {
            node.IsReachable = false;
        }
    }
    private Vector3 GetNodeExtents()
    {
        float gridWidth = _xConstraint.y - _xConstraint.x;
        float gridHeight = _yConstraint.y - _yConstraint.x;
        float nodeWidth = gridWidth / GridSize;
        float nodeHeight = gridHeight / GridSize;
        return new Vector3(nodeWidth, nodeHeight, 1);
    }
}
