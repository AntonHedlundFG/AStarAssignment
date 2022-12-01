using System.Collections.Generic;
using UnityEngine;

public class PathMarkerManager : MonoBehaviour
{
    [SerializeField] private GameObject _markerPrefab;

    private List<GameObject> _markers = new List<GameObject>();

    public void ClearMarkers()
    {
        if (_markers == null) { return; }
        for (int i = 0; i < _markers.Count; i++)
        {
            Destroy(_markers[i]);
        }
        _markers.Clear();
    }

    public void DeleteNextMarker()
    {
        if (_markers == null || _markers.Count == 0) { return; }
        Destroy(_markers[0]);
        _markers.RemoveAt(0);
    }

    public void GenerateMarkers(Node[] nodes)
    {
        ClearMarkers();
        for (int i = 0; i < nodes.Length; i++)
        {
            SpawnMarker(nodes[i]);
        }
    }

    private void SpawnMarker(Node node)
    {
        Vector2 nodePos = node.GetVector2();
        Vector3 spawnPos = new Vector3(nodePos.x, nodePos.y, 0f);
        GameObject newObject = Instantiate(_markerPrefab, spawnPos, Quaternion.identity, transform);
        _markers.Add(newObject);
    }

}
