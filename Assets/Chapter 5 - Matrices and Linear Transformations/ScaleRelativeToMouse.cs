using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRelativeToMouse : MonoBehaviour
{
    public RectTransform MapIcon;
    public RectTransform FreeMapIcon;
    public Vector2 _MapParentDefPos;
    public Vector2 lastMousePosition;
    public float ZoomSensitivity = .025f;
    public ScrollState _scrollState = ScrollState.None;

    [Range(0f,1f)]
    public float currentScale = 1f;
    private Vector2 mousePos;
    public Transform point;
    private Vector2 offset;
    public enum ScrollState
    {
        None = 0,
        Scrolling = 1,
        End = 2
    }

    private Vector3 unityScale = Vector3.one;
    public Vector3 scaleVector = Vector3.one;


    // Start is called before the first frame update
    void Start()
    {
        _MapParentDefPos = MapIcon.transform.position;

        offset = point.position - MapIcon.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var scrollValue = Input.mouseScrollDelta.y;
        mousePos = Input.mousePosition;
        point.position = mousePos;
        var scaled = (scrollValue / 120f) * ZoomSensitivity;
        currentScale = Mathf.Clamp(currentScale + scaled, .5f, 2f);
        unityScale = Vector3.one  * currentScale;

        FreeMapIcon.localScale = Vector3.one * currentScale;
        FreeMapIcon.transform.position = Chapter5.ScaleAroundPivot(_MapParentDefPos, point.position, Vector3.one - FreeMapIcon.localScale);
        //MapIcon.transform.position = (Vector3)offset * -currentScale + Chapter5.ScaleAroundPivot(_MapParentDefPos, point.position, currentScale * Vector3.one);

        // update default position to scaled value
        if(Mathf.Abs(scrollValue) > 0f && _scrollState == ScrollState.None )
        {
            _scrollState = ScrollState.Scrolling;
            //Vector2 newPos = Vector2.Scale(MapIcon.position, MapIcon.localScale) + Vector2.Scale(mousePos, (Vector3.one - MapIcon.localScale));
            //Vector2 off = (Vector2)MapIcon.position - newPos;
            var newPos = Chapter5.ScaleAroundPivot(_MapParentDefPos, mousePos, Vector3.one - MapIcon.localScale);
            var oldPos = Chapter5.ScaleAroundPivot(_MapParentDefPos, lastMousePosition, Vector3.one - MapIcon.localScale);
            //offset = oldPos - newPos;
            //_MapParentDefPos = Chapter5.ScaleAroundPivot(_MapParentDefPos, lastMousePosition, Vector3.one - MapIcon.localScale);
            _MapParentDefPos = MapIcon.transform.position;
            //offset = (_MapParentDefPos - (Vector2)oldPos);

            lastMousePosition = mousePos;
            scaleVector = Vector3.one;
        }

        if(_scrollState == ScrollState.Scrolling)
        {
            scaleVector += Vector3.one * scaled;
            if(Vector2.Distance(mousePos, lastMousePosition) > 0f || scaleVector.x >= 2.0f || scaleVector.x <= .5f)
            {
                _scrollState = ScrollState.None;
                return;
            }

            //var scaled = (scrollValue / 120f) * ZoomSensitivity;
            //currentScale = Mathf.Clamp(currentScale + scaled, .75f, 1.5f);
            //var oldScale = MapIcon.localScale;
            MapIcon.localScale = unityScale;
            MapIcon.transform.position = /*(Vector3)offset + */Chapter5.ScaleAroundPivot(_MapParentDefPos, lastMousePosition, Vector3.one - scaleVector);
        }

        //// update default position to scaled value
        //if(Mathf.Abs(scrollValue) > 0f && _scrollState == ScrollState.None)
        //{
        //    _scrollState = ScrollState.Scrolling;
        //    Vector2 newPos = Vector2.Scale(MapIcon.position, MapIcon.localScale) + Vector2.Scale(mousePos, (Vector3.one - MapIcon.localScale));
        //    Vector2 off = (Vector2)MapIcon.position - newPos;
        //    _MapParentDefPos = (Vector2)MapIcon.position + off;
        //    lastMousePosition = mousePos;
        //}

        //if(_scrollState == ScrollState.Scrolling)
        //{

        //    // zoom in or out relatively mouse position

        //    Vector2 offset = mousePos - (Vector2)MapIcon.transform.position;

        //    var scaled = (scrollValue / 120f) * ZoomSensitivity;
        //    currentScale = Mathf.Clamp(currentScale + scaled, .75f, 1.5f);
        //    MapIcon.localScale = Vector3.one * currentScale;
        //    MapIcon.position = Vector2.Scale(_MapParentDefPos, MapIcon.localScale) + Vector2.Scale(mousePos, (Vector3.one - MapIcon.localScale));
        //    if(Vector2.Distance(mousePos, lastMousePosition) > 2f)
        //    {
        //        _scrollState = ScrollState.None;
        //    }
        //}
    }

    static Vector3 Scale(Vector3 a, Vector3 b)
    {
        return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
    }
 
    static Vector3 Div(Vector3 a, Vector3 b)
    {
        return new Vector3 (b.x == 0f ? 0 : a.x / b.x, b.y == 0f ? 0 : a.y / b.y, b.z == 0f ? 0 : a.z / b.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(MapIcon.position, Vector2.Scale(MapIcon.position, MapIcon.localScale) + Vector2.Scale(mousePos, (Vector3.one - MapIcon.localScale)));
    }

}
