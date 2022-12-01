using System.Collections;
using UnityEngine;

public class UnitClicker : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private GameObject _unit;
    private NodeGrid _nodeGrid;

    [SerializeField] private PathMarkerManager _pathMarker;
    [SerializeField] private GameObject _clickMarkerPrefab;

    private float _delayPerNode;
    private int _lerpIntervals = 10;
    private bool _isMoving;
    private Coroutine _moveRoutine;
    

    private void Start()
    {
        _nodeGrid = _gridManager?.GetNodeGrid();
        if(_nodeGrid == null) { gameObject.SetActive(false); }
        _delayPerNode = 1f / (float) _nodeGrid.GridSize;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //Leftclick
        {
            Teleport();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) //Rightclick
        {
            Move();          
        }
    }

    private void Teleport()
    {
        CancelMove();

        Vector2 clickPos;
        if (GetClickPos(out clickPos))
        {
            SpawnClickMarker(0, clickPos);
            Vector2Int coords = _nodeGrid.GetGridPosFromVector2(clickPos);

            if (_nodeGrid.Grid[coords.x][coords.y].IsReachable)
            {
                clickPos = _nodeGrid.GetAlignedVector2(clickPos);
                _unit.transform.position = new Vector3(clickPos.x, clickPos.y, _unit.transform.position.z);
            }

            _pathMarker?.ClearMarkers();
        }
    }
    private void Move()
    {
        CancelMove();

        Vector2 clickPos;
        if (GetClickPos(out clickPos))
        {
            SpawnClickMarker(1, clickPos);
            Vector2Int targetIndexes = _nodeGrid.GetGridPosFromVector2(clickPos);
            Node[] path = AStarPathfind.PathFind(_nodeGrid, _nodeGrid.GetGridPosFromVector2(_unit.transform.position), targetIndexes);
            if (path != null)
            {
                _pathMarker?.GenerateMarkers(path);
                _moveRoutine = StartCoroutine(MoveAlongPath(path));
            }

        }
    }
    private void CancelMove()
    {
        if (_isMoving)
        {
            StopCoroutine(_moveRoutine);
            _isMoving = false;
            _pathMarker?.ClearMarkers();
        }
    }

    private void SpawnClickMarker(int clickType, Vector2 position)
    {
        GameObject marker = Instantiate(_clickMarkerPrefab);
        marker.GetComponent<ClickMarker>().Setup(clickType, position);
    }

    private IEnumerator MoveAlongPath(Node[] path)
    {
        _isMoving = true;
        for (int i = 1; i < path.Length; i++)
        {
            Vector2 curPos = new Vector2(_unit.transform.position.x, _unit.transform.position.y);
            Vector2 nextPos = path[i].GetVector2();
            for (int j = 0; j < _lerpIntervals; j++)
            {
                if (j == _lerpIntervals / 2) { _pathMarker?.DeleteNextMarker(); } //Delete the next path marker halfway there, it looks better.

                Vector2 newPos = Vector2.Lerp(curPos, nextPos, (float) j / (float) _lerpIntervals);
                UpdateObjectPosition(_unit, newPos);
                yield return new WaitForSeconds(_delayPerNode / (float)_lerpIntervals);
            }
        }
        _isMoving = false;
        _pathMarker?.ClearMarkers();
    }

    private void UpdateObjectPosition(GameObject obj, Vector2 XYPos)
    {
        obj.transform.position = new Vector3(XYPos.x, XYPos.y, obj.transform.position.z);
    }

    private bool GetClickPos(out Vector2 pos)
    {
        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, layerMask))
        {
            pos = new Vector2(hit.point.x, hit.point.y);
            return true;
        }

        pos = Vector2.zero;
        return false;
    }
}
