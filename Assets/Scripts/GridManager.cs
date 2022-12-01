using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject _topLeftCornerObject;
    [SerializeField] private GameObject _botRightCornerObject;

    private Vector2 _xConstraint;
    private Vector2 _yConstraint;

    [SerializeField][Range(10, 200)] private int _nodeCount = 100;
    private NodeGrid _nodeGrid;

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
        _nodeGrid = new NodeGrid(_xConstraint, _yConstraint, _nodeCount);
    }

    public NodeGrid GetNodeGrid() => _nodeGrid;
}
