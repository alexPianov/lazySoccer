using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomMap : MonoBehaviour
{
    [SerializeField] private RectTransform map;
    void Update()
    {
        if (Input.touchCount < 2)
        {
            f0start = Vector2.zero;
            f1start = Vector2.zero;
        }

        if (Input.touchCount == 2) Zoom();
    }
    private Vector3 MinZoom = new Vector3(0.5f, 0.5f);
    private Vector3 MaxZoom = new Vector3(0.9f, 0.9f);
    private Vector3 paramZoom = new Vector3(0.01f, 0.01f);

    [Button]
    private void UpZoom()
    {
        if (map.transform.localScale.x < MaxZoom.x)
            map.transform.localScale += paramZoom;
    }
    [Button]
    private void DownZoom()
    {
        if (map.transform.localScale.x > MinZoom.x)
            map.transform.localScale -= paramZoom;
    }
    public float sensitivity;

    Vector2 f0start;

    Vector2 f1start;

    void Zoom()

    {
        if (f0start == Vector2.zero && f1start == Vector2.zero)
        {
            f0start = Input.GetTouch(0).position;
            f1start = Input.GetTouch(1).position;
        }

        Vector2 f0position = Input.GetTouch(0).position;
        Vector2 f1position = Input.GetTouch(1).position;

        float dir = Mathf.Sign(Vector2.Distance(f1start, f0start) - Vector2.Distance(f0position, f1position));
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, dir * sensitivity * Time.deltaTime * Vector3.Distance(f0position, f1position));

    }
    
    
}
