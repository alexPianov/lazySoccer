using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ScrollingZoom : MonoBehaviour {

    private float zoomModifier = 0.04f;
    private float zoomValue = 0.5f;
    private float zoomModifierSpeed = 0.01f;
    private float zoomValueMin = 0.5f;
    private float zoomValueMax = 0.75f;
    [SerializeField] private Transform transformObject;
    [SerializeField] private Slider _slider;
    [SerializeField] private ScrollRect _scrollRect;
    
    private Vector2 firstTouchPrevPos, secondTouchPrevPos;
    private float touchesPrevPosDifference, touchesCurPosDifference;

    private void Start()
    {
        _slider.value = zoomValue;
        _slider.minValue = zoomValueMin;
        _slider.maxValue = zoomValueMax;
        ZoomIn();
    }

    private float time;
    private void Update() 
    {
        if (Input.touchCount == 2)
        {
            if(_scrollRect) _scrollRect.enabled = false;
            
            var firstTouch = Input.GetTouch(0);
            var secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            //zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference >= touchesCurPosDifference)
            {
                ZoomOut();
            }

            if (touchesPrevPosDifference < touchesCurPosDifference)
            {
                ZoomIn();
            }
        }
        else
        {
            if(_scrollRect) _scrollRect.enabled = true;
        }
    }
    
    [Button]
    public void ZoomIn()
    {
        if(zoomValue >= zoomValueMax) return;
        zoomValue += zoomModifier;
        transformObject.localScale = new Vector3(zoomValue, zoomValue, 1);
        _slider.value = zoomValue;
        
        time += Time.deltaTime;

        if (time < 0.01f) return;

        time = 0;
        _scrollRect.enabled = true;
    }
    
    [Button]
    public void ZoomOut()
    {
        if(zoomValue <= zoomValueMin) return;
        zoomValue -= zoomModifier;
        transformObject.localScale = new Vector3(zoomValue, zoomValue, 1);
        _slider.value = zoomValue;
        
        time += Time.deltaTime;

        if (time < 0.01f) return;

        time = 0;
        _scrollRect.enabled = true;
    }
}