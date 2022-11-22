using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarPathfind
{
    public static Node[] PathFind(NodeGrid nodeGrid, Vector2Int startIndex, Vector2Int endIndex)
    {
        nodeGrid.ResetGrid();
        nodeGrid.SetPriorities(startIndex, endIndex);
        if (!IndexWithinGrid(nodeGrid.gridSize, startIndex) || !IndexWithinGrid(nodeGrid.gridSize, endIndex)) 
        { 
            return new Node[0]; 
        } //Confirm index is OK, otherwise return empty array
        if (startIndex == endIndex) { return new Node[] { nodeGrid.grid[startIndex.x][startIndex.y] }; } //If start node = end node, return only that node.

        //Set up queues and sets
        Node startNode = nodeGrid.grid[startIndex.x][startIndex.y];
        startNode.PrioF = 0;
        Node endNode = nodeGrid.grid[endIndex.x][endIndex.y];
        if (!startNode.isReachable || !endNode.isReachable) { return null; }

        PriorityQueue<Node> q = new PriorityQueue<Node>();
        q.Insert(startNode, 0f);

        HashSet<Node> visitedNodes = new HashSet<Node>();


        while (!q.IsEmpty())
        {
            Node currentNode = q.Pop();
            Vector2Int[] currentNeighbours = NeighbouringIndexes(nodeGrid.gridSize, currentNode.indexes);

            for (int i = 0; i < currentNeighbours.Length; i++)
            {
                Node nextNode = nodeGrid.grid[currentNeighbours[i].x][currentNeighbours[i].y];

                if (visitedNodes.Contains(nextNode)) { continue; }

                if (nextNode.indexes == endIndex)
                {
                    nextNode.SetParentNode(currentNode);
                    Node[] returnPath = nextNode.GetParentPath();
                    Array.Reverse(returnPath);
                    return returnPath;
                }

                if (!nextNode.isReachable) { continue; } 

                float tempF = currentNode.PrioF + Vector2.Distance(currentNode.GetVector2(), nextNode.GetVector2());
                if (nextNode.PrioF > tempF)
                {
                    q.Remove(nextNode);
                    nextNode.PrioF = tempF;
                    nextNode.SetParentNode(currentNode);
                    q.Insert(nextNode, nextNode.PrioTotal);
                }
            }
            visitedNodes.Add(currentNode);

        }
        return null;
    }

    public static bool IndexWithinGrid(int gridSize, Vector2Int index)
    {
        return !(index.x < 0 || index.y < 0
            || index.x >= gridSize || index.y >= gridSize);
    }

    private static Vector2Int[] NeighbouringIndexes(int gridSize, Vector2Int index)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (j == 0 && i == 0) { continue; }
                Vector2Int newIndex = new Vector2Int(index.x + i, index.y + j);
                if (IndexWithinGrid(gridSize, newIndex)) { list.Add(newIndex); }
            }
        }

        return list.ToArray();
    }
}
