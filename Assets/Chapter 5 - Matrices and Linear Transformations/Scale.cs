using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// continously scaling relative to point

public class Scale : MonoBehaviour
{
    public Transform point;
    public Transform pivot;

    [Range(0f, 2f)]
    public float scale = 1f;

    public Vector3 defaultLocation = Vector3.zero;
    public float scrollSensitivity = 5f;
    public Vector3 lastPoint = Vector3.zero;

    private void Update()
    {
        var mousePos = Input.mousePosition;
        var worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        worldPoint.z = 0f;
        point.position = worldPoint;

        //if(Vector3.Distance(worldPoint, lastPoint) > 1f)
        //{
        //    defaultLocation = pivot.position;
        //}

        // we are trying to scale the grid relative to mousePos
        // first we find unscaled value of displacement (like scale was 1.0f)
        // then apply current scale
        // then find the offset
        // then apply new scale

        if(Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            scale += Mathf.Sign(Input.mouseScrollDelta.y) * .1f;
            Vector3 displacement = point.position - pivot.position;
            float inverseScale = 1f / pivot.transform.localScale.x;
            displacement *= inverseScale;
            Vector3 offset = displacement * (pivot.transform.localScale.x - scale);
            pivot.position = pivot.position + offset;
            pivot.localScale = Vector3.one * scale;
            lastPoint = worldPoint;
        }

        // the problem is our point.position - defaultLocation
        // is scaled value :) 
        if(Input.GetKeyDown(KeyCode.S))
        {
            Vector3 displacement = point.position - pivot.position;
            float inverScale = 1f / pivot.transform.localScale.x;
            displacement *= inverScale;
            Vector3 offset = displacement * (pivot.transform.localScale.x - scale);
            pivot.position = pivot.position + offset;
            pivot.localScale = Vector3.one * scale;
            UpdateDefaultLocation();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            UpdateDefaultLocation();
        }
    }

    private void UpdateDefaultLocation()
    {
        defaultLocation = pivot.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(point.position, .25f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(defaultLocation, .25f);

    }

}
