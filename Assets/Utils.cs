using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Utils
{
    public static void DrawHandles(this Transform t, Vector3 right, Vector3 up, Vector3 forward)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(t.position, t.position + right);
        Gizmos.DrawWireSphere(t.position + right, .01f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(t.position, t.position + up);
        Gizmos.DrawWireSphere(t.position + up, .01f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(t.position, t.position + forward);
        Gizmos.DrawWireSphere(t.position + forward, .01f);
    }

    public static void DrawName(this Transform t)
    {
        var font = GUI.skin.label;
        font.fontSize = 15;
        Handles.Label(t.position + Vector3.up - Vector3.right * .5f, $"{t.name}", font);
    }

}
