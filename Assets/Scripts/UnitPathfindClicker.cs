using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPathfindClicker : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private GameObject _unit;
    private NodeGrid _nodeGrid;

    private float _delayPerNode;
    private int _lerpIntervals = 10;
    private bool _isMoving;
    private Coroutine _moveRoutine;
    

    private void Start()
    {
        _nodeGrid = _gridManager?.GetNodeGrid();
        if(_nodeGrid == null) { gameObject.SetActive(false); }
        _delayPerNode = 1f / (float) _nodeGrid.gridSize;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) //Leftclick
        {
            CancelMove();

            Vector2 clickPos;
            if (GetClickPos(out clickPos))
            {
                clickPos = _nodeGrid.GetAlignedVector2(clickPos);
                _unit.transform.position = new Vector3(clickPos.x, clickPos.y, _unit.transform.position.z);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) //Rightclick
        {
            CancelMove();

            Vector2 clickPos;
            if (GetClickPos(out clickPos))
            {
                Vector2Int targetIndexes = _nodeGrid.GetGridPosFromVector2(clickPos);
                Node[] path = AStarPathfind.PathFind(_nodeGrid, _nodeGrid.GetGridPosFromVector2(_unit.transform.position), targetIndexes);
                if (path != null) { _moveRoutine = StartCoroutine(MoveAlongPath(path)); }
                
            }
            
        }


    }
    private void CancelMove()
    {
        if (_isMoving)
        {
            StopCoroutine(_moveRoutine);
            _isMoving = false;
        }
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
                Vector2 newPos = Vector2.Lerp(curPos, nextPos, (float) j / (float) _lerpIntervals);
                UpdateObjectPosition(_unit, newPos);
                yield return new WaitForSeconds(_delayPerNode / (float)_lerpIntervals);
            }
        }
        _isMoving = false;
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

        pos = Vector2.positiveInfinity;
        return false;
    }
}
