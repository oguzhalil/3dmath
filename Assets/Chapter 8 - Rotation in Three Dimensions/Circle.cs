using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public float radius;
    public int resolution = 36; // 360/36 = 10 line at total
    public bool showNumbers;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    // looking through of z+
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, .05f);
        Vector2 offset = transform.position;
        Vector2 from = (Vector2)transform.position + Vector2.right * radius;
        int len = resolution;
        // transform.DrawHandles(Vector3.right, Vector3.up, Vector3.forward);

        for (int i = 1; i <= len; i++)
        {
            float step = (i / (float)len);
            float theta = 0f + step * Mathf.PI * 2;

            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            Vector2 to = offset + new Vector2(x, y);
            Gizmos.color = Color.HSVToRGB(theta / (Mathf.PI * 2), 1f, 1f);
            //Gizmos.DrawWireSphere(to, radius * .1f);
            Gizmos.DrawLine(from, to);
            var font = GUI.skin.label;
            font.fontSize = 25;
            if (showNumbers)
            {
                Handles.Label(from, $"{i}", font);
            }


            from = to;
        }


        //Gizmos.DrawLine
    }
}
