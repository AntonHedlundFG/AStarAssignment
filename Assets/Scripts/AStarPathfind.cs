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
        Node endNode = nodeGrid.grid[endIndex.x][endIndex.y];

        PriorityQueue<Node> qStart = new PriorityQueue<Node>();
        qStart.Insert(startNode, 0f);

        PriorityQueue<Node> qEnd = new PriorityQueue<Node>();
        qEnd.Insert(endNode, 0f);

        HashSet<Node> visitedNodesStart = new HashSet<Node>();
        visitedNodesStart.Add(startNode);

        HashSet<Node> visitedNodesEnd = new HashSet<Node>();
        visitedNodesEnd.Add(endNode);

        bool fromStart; //This keeps track of in which direction we're iterating
        while (!qStart.IsEmpty() && !qEnd.IsEmpty())
        {
            fromStart = qStart.HasBetterPriority(qEnd);
            PriorityQueue<Node> currentQ = fromStart ? qStart : qEnd;
            HashSet<Node> currentVisitedNodes = fromStart ? visitedNodesStart : visitedNodesEnd;
            HashSet<Node> otherVisitedNodes = fromStart ? visitedNodesEnd : visitedNodesStart;

            Node currentNode = currentQ.Pop();
            Vector2Int[] currentNeighbours = NeighbouringIndexes(nodeGrid.gridSize, currentNode.indexes);

            for (int i = 0; i < currentNeighbours.Length; i++)
            {
                Node nextNode = nodeGrid.grid[currentNeighbours[i].x][currentNeighbours[i].y];

                if (currentVisitedNodes.Contains(nextNode)) { continue; } //Already visited

                if (otherVisitedNodes.Contains(nextNode))
                {
                    Node[] currentPath = currentNode.GetParentPath();
                    Node[] nextPath = nextNode.GetParentPath();
                    Node[] returnPath = new Node[currentPath.Length + nextPath.Length];
                    if (fromStart)
                    {
                        Array.Reverse(currentPath);
                        currentPath.CopyTo(returnPath, 0);
                        nextPath.CopyTo(returnPath, currentPath.Length);
                    } else
                    {
                        Array.Reverse(nextPath);
                        nextPath.CopyTo(returnPath, 0);
                        currentPath.CopyTo(returnPath, nextPath.Length);
                    }
                    return returnPath;
                }

                if (!nextNode.isReachable) { continue; } //NOT SURE IF THIS IS RIGHT PLACE FOR REACHABLE CHECK
                nextNode.SetParentNode(currentNode);
                currentVisitedNodes.Add(nextNode);
                if (fromStart)
                {
                    nextNode.prioStart = currentNode.prioStart + Vector2.Distance(currentNode.GetVector2(), nextNode.GetVector2());
                } else
                {
                    nextNode.prioEnd = currentNode.prioEnd + Vector2.Distance(currentNode.GetVector2(), nextNode.GetVector2());
                }
                currentQ.Insert(nextNode, nextNode.prioTotal);
            }

        }
        Debug.Log("No Path");
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
