using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject _topLeftCornerObject;
    [SerializeField] private GameObject _botRightCornerObject;

    private Vector2 _xConstraint;
    private Vector2 _yConstraint;

    [SerializeField] private int NODESIZE;
    private NodeGrid _nodeGrid;

    [SerializeField] Vector2Int TESTA;
    [SerializeField] Vector2Int TESTB;
    private Node[] TESTNODEPATH;

    private void Awake()
    {
        SetConstraints();
        GenerateGrid();
        

    }
    private void SetConstraints()
    {
        _xConstraint = new Vector2(_topLeftCornerObject.transform.position.x, _botRightCornerObject.transform.position.x);
        _yConstraint = new Vector2(_botRightCornerObject.transform.position.y, _topLeftCornerObject.transform.position.y);
    }
    private void GenerateGrid()
    {
        _nodeGrid = new NodeGrid(_xConstraint, _yConstraint, NODESIZE);
    }

    private void Update()
    {
        //DebugMethods();
    }
    private void DebugMethods()
    {
        _nodeGrid.DrawDebugNodeMarkers();
        _nodeGrid.DrawDebugBorders();
        TESTNODEPATH = AStarPathfind.PathFind(_nodeGrid, TESTA, TESTB);
        if (TESTNODEPATH == null) { return; }
        for (int i = 0; i < TESTNODEPATH.Length - 1; i++)
        {
            Vector2 vec2A = TESTNODEPATH[i].GetVector2();
            Vector2 vec2B = TESTNODEPATH[i + 1].GetVector2();
            Vector3 vec3A = new Vector3(vec2A.x, vec2A.y, 0);
            Vector3 vec3B = new Vector3(vec2B.x, vec2B.y, 0);
            Debug.DrawLine(vec3A, vec3B, Color.green);
        }
    }

    public NodeGrid GetNodeGrid() => _nodeGrid;
}
