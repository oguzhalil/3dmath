using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter5 : MonoBehaviour
{
    public Transform Origin2D;

    Vector2 defaultP;
    Vector2 defaultQ;

    Vector2 rotatedP;
    Vector2 rotatedQ;

    // scale a point around custom point
    public Transform cube;
    private Vector3 targetpoint;
    private Vector3 defPos;
    [Range(.5f, 3f)]
    public float scale = 1f;

    public Transform unity;
    public Transform point;

    public Transform somePoint;
    public Transform arbitraryPoint;

    // Use this for initialization
    void Start()
    {
        Vector3 n = new Vector3(0.267f, -.535f, .802f);


        // nx ^2 (1 − cos θ) + cos θ
        print("cos" + ((n.x * n.x) * (1.0f - Mathf.Cos(15f * Mathf.Deg2Rad)) + Mathf.Cos(15f * Mathf.Deg2Rad)));

        //// nxny (1 − cos θ) + nz sin θ
        ////print("y1 " + ((n.x * n.y) * (0 - Mathf.Cos(-15)) + (n.z * Mathf.Sin(-15) )) );

        //print(-Mathf.Cos(-15) + n.x);
        //print(Mathf.Sin(-15));

        targetpoint = point.position;
        defPos = point.parent.transform.position;
        //Debug.Log(pivot.transform.position + "  " + unityTransformScale.transform.position);

        Debug.Log($"Point {point.transform.position} Unity {unity.transform.position}");

        Rotation2D(30); // 2d rotation clockwise rotation (negative) or counterclockwise(positive)

    }

    // Update is called once per frame
    void Update()
    {
        float animation = Mathf.Cos(Mathf.PI * Time.time * .2f);
        Vector3 scale = Vector3.LerpUnclamped(Vector3.one * 0.5f, Vector3.one, 1.0f + animation);
        scale.z = 1f;

        unity.localScale = scale;

        cube.transform.localScale = scale;
        cube.transform.position = Vector3.Scale(defPos, scale) + Vector3.Scale(targetpoint, (Vector3.one - scale));

        //cube.transform.position = defPos + offset;

        // scale a point relatively arbitrary point
    }

    void Rotation2D(float rotate)
    {
        Vector2 p = Vector2.up;
        Vector2 q = Vector2.right;

        defaultP = p;
        defaultQ = q;

        if(rotate > 0) // counterclockwise
        {
            p = new Vector2(Mathf.Cos(rotate * Mathf.Deg2Rad), Mathf.Sin(rotate * Mathf.Deg2Rad));
            q = new Vector2(-Mathf.Sin(rotate * Mathf.Deg2Rad), Mathf.Cos(rotate * Mathf.Deg2Rad));
        }


        rotatedP = p;
        rotatedQ = q;
    }

    private void OnDrawGizmos()
    {
        DrawPoint(somePoint.position, Color.red);
        DrawPoint(somePoint.position - arbitraryPoint.position, Color.green);
        DrawPoint(arbitraryPoint.position, Color.blue);

        DrawPoint((somePoint.position - arbitraryPoint.position) * .5f, Color.green);

        DrawPoint(((somePoint.position - arbitraryPoint.position) * .5f) + arbitraryPoint.position, Color.green);

        DrawPoint(ScaleAroundPivot(arbitraryPoint.position, somePoint.position, Vector3.one * .75f), Color.red);



        //Gizmos.color = Color.red; // red is distance formula
        //Gizmos.DrawLine(Origin2D.position, defaultP);
        //Gizmos.DrawLine(Origin2D.position, defaultQ);

        //Gizmos.color = Color.yellow; // yellow is dot product of unit vec
        //Gizmos.DrawLine(Origin2D.position, rotatedP);
        //Gizmos.DrawLine(Origin2D.position, rotatedQ);

        ////DrawVertices(unityTransformScale.GetComponent<MeshFilter>() , unityTransformScale.GetComponent<MeshRenderer>());
        //DrawVertices(cube.GetComponent<MeshFilter>(), cube.GetComponent<MeshRenderer>());
    }

    // scale this point relative to pivot
    public static Vector3 ScaleAroundPivot(Vector3 pivot, Vector3 point, Vector3 scale)
    {
        Vector3 displacement = point - pivot;
        displacement = Vector3.Scale(displacement, scale);
        return pivot + displacement;
    }

    public void DrawVertices(MeshFilter meshFilter, MeshRenderer meshRenderer)
    {
        var vertices = meshFilter.sharedMesh.vertices;

        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(.5f, .5f, .5f) + vertices[i];
        }

        var absVertices = vertices;

        // Jonh vince's book 9.5.6 2D Scaling
        // translate to origin (-px,-py) -> scale -> translate to point (px,px)
        // reverse order if matrix operations...
        foreach(var item in absVertices)
        {
            Vector3 position = item * scale + (new Vector3(.5f, .5f, .5f) * (1f - scale));

            //Vector3 position = item;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(position, .1f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, position);
        }
    }

    private void DrawPoint(Vector3 position, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(Vector3.zero, position);
        Gizmos.DrawWireSphere(position, .1f);
    }
}
