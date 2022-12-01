using System.Collections;
using UnityEngine;

public class ClickMarker : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 2f)] private float _duration;
    [SerializeField] private Material[] _materials;

    [SerializeField] [Range(0.1f, 2f)] private float _maxSize = 1f;
    private float _startSize = 0.01f;

    private Renderer _renderer;

    public void Setup(int clickType, Vector2 pos)
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = _materials[clickType];
        transform.position = new Vector3(pos.x, pos.y, -3);
        StartCoroutine(Decay());
    }

    private IEnumerator Decay()
    {
        float curDuration = 0f;
        Material material = _renderer.material;
        Color startColor = material.color;
        Color endColor = startColor;
        endColor.a = 0;

        while (curDuration < _duration)
        {
            float t = curDuration / _duration;
            transform.localScale = Vector3.Lerp(Vector3.one * _startSize, Vector3.one * _maxSize, t);
            material.color = Color.Lerp(startColor, endColor, t - 0.2f);

            curDuration += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);


    }
}
