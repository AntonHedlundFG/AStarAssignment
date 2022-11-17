using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid
{
    private Vector2 _xConstraint;
    private Vector2 _yConstraint;
    public int gridSize { get; private set; }
    public Node[][] grid { get; private set; }
    public NodeGrid(Vector2 xConstraint, Vector2 yConstraint, int gridSize)
    {
        _xConstraint = InvertIfXGTY(xConstraint);
        _yConstraint = InvertIfXGTY(yConstraint);
        this.gridSize = Mathf.Max(gridSize, 1);
        GenerateGrid();
    }

    public void SetPriorities(Vector2Int startIndex, Vector2Int endIndex)
    {
        if (!AStarPathfind.IndexWithinGrid(gridSize, startIndex) || !AStarPathfind.IndexWithinGrid(gridSize, endIndex)) { return; }
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[i][j].prioStart = Vector2.Distance(grid[i][j].GetVector2(), grid[startIndex.x][startIndex.y].GetVector2());
                grid[i][j].prioEnd = Vector2.Distance(grid[i][j].GetVector2(), grid[endIndex.x][endIndex.y].GetVector2());
            }
        }
    }

    private Vector2 InvertIfXGTY(Vector2 vect)
    {
        return (vect.x <= vect.y) ? vect : new Vector2(vect.y, vect.x);
    }
    private void GenerateGrid()
    {
        grid = new Node[gridSize][];
        for (int i = 0; i < gridSize; i++)
        {
            grid[i] = new Node[gridSize];
            for (int j = 0; j < gridSize; j++)
            {
                grid[i][j] = new Node(GetVector2FromGridPos(i, j), new Vector2Int(i, j));
                CheckCollisionForNode(grid[i][j]);
            }
        }
    }
    public Vector2 GetVector2FromGridPos(int x, int y)
    {
        x = Mathf.Clamp(x, 0, gridSize - 1);
        y = Mathf.Clamp(y, 0, gridSize - 1);
        float gridWidth = _xConstraint.y - _xConstraint.x;
        float gridHeight = _yConstraint.y - _yConstraint.x;
        float nodeWidth = gridWidth / gridSize;
        float nodeHeight = gridHeight / gridSize;
        float vectX = _xConstraint.x + (x + 0.5f) * nodeWidth;
        float vectY = _yConstraint.x + (y + 0.5f) * nodeHeight;
        return new Vector2(vectX, vectY);
    }
    public Vector2Int GetGridPosFromVector2(Vector2 position)
    {
        if (position.x < _xConstraint.x || position.x >= _xConstraint.y 
            || position.y < _yConstraint.x || position.y >= _yConstraint.y)
        {
            return new Vector2Int(-1, -1);
        }
        float gridWidth = _xConstraint.y - _xConstraint.x;
        float gridHeight = _yConstraint.y - _yConstraint.x;
        float nodeWidth = gridWidth / gridSize;
        float nodeHeight = gridHeight / gridSize;
        float xPos = (position.x - _xConstraint.x) / nodeWidth;
        float yPos = (position.y - _yConstraint.x) / nodeHeight;
        return new Vector2Int((int)xPos, (int)yPos);
    }
    public void ResetGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[i][j].SetParentNode(null);
            }
        }
    }
    private void CheckCollisionForNode(Node node)
    {
        if(Physics.BoxCast(new Vector3(node.GetVector2().x, node.GetVector2().y, -100), GetNodeExtents() / 2, Vector3.forward))
        {
            node.isReachable = false;
        }
    }
    private Vector3 GetNodeExtents()
    {
        float gridWidth = _xConstraint.y - _xConstraint.x;
        float gridHeight = _yConstraint.y - _yConstraint.x;
        float nodeWidth = gridWidth / gridSize;
        float nodeHeight = gridHeight / gridSize;
        return new Vector3(nodeWidth, nodeHeight, 1);
    }
    




    //DEBUG METHODS
    public void DrawDebugNodeMarkers()
    {
            for (int i = 0; i < gridSize - 1; i++)
            {
                for (int j = 0; j < gridSize - 1; j++)
                {
                    Vector2 pos2 = grid[i][j].GetVector2();
                    Vector2 pos2r = grid[i + 1][j].GetVector2();
                    Vector2 pos2d = grid[i][j + 1].GetVector2();

                    Vector3 pos3 = new Vector3(pos2.x, pos2.y, 0);
                    Vector3 pos3r = new Vector3(pos2r.x, pos2r.y, 0);
                    Vector3 pos3d = new Vector3(pos2d.x, pos2d.y, 0);
                    Debug.DrawLine(pos3, pos3r, Color.blue);
                    Debug.DrawLine(pos3, pos3d, Color.blue);
                }

            Vector2 pos2A = grid[i][gridSize - 1].GetVector2();
            Vector2 pos2Ar = grid[i + 1][gridSize - 1].GetVector2();
            Debug.DrawLine(pos2A, pos2Ar, Color.blue);

            Vector2 pos2B = grid[gridSize - 1][i].GetVector2();
            Vector2 pos2Bd = grid[gridSize - 1][i + 1].GetVector2();
            Debug.DrawLine(pos2B, pos2Bd, Color.blue);
        }
    }
    public void DrawDebugBorders()
    {
        Vector3 topRight = new Vector3(_xConstraint.y, _yConstraint.y, 0);
        Vector3 topLeft = new Vector3(_xConstraint.x, _yConstraint.y, 0);
        Vector3 botRight = new Vector3(_xConstraint.y, _yConstraint.x, 0);
        Vector3 botLeft = new Vector3(_xConstraint.x, _yConstraint.x, 0);
        Debug.DrawLine(topRight, topLeft, Color.red);
        Debug.DrawLine(topLeft, botLeft, Color.red);
        Debug.DrawLine(botLeft, botRight, Color.red);
        Debug.DrawLine(botRight, topRight, Color.red);
    }
}
