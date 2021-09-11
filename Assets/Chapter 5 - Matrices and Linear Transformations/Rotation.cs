using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform Origin2D;

    Vector2 defaultP;
    Vector2 defaultQ;

    Vector2 rotatedP;
    Vector2 rotatedQ;

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




        Rotation2D(30); // 2d rotation clockwise rotation (negative) or counterclockwise(positive)

    }

    void Rotation2D(float rotate)
    {
        Vector2 p = Vector2.up;
        Vector2 q = Vector2.right;

        defaultP = p;
        defaultQ = q;

        if (rotate > 0) // counterclockwise
        {
            p = new Vector2(Mathf.Cos(rotate * Mathf.Deg2Rad), Mathf.Sin(rotate * Mathf.Deg2Rad));
            q = new Vector2(-Mathf.Sin(rotate * Mathf.Deg2Rad), Mathf.Cos(rotate * Mathf.Deg2Rad));
        }


        rotatedP = p;
        rotatedQ = q;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // red is distance formula
        Gizmos.DrawLine(Origin2D.position, defaultP);
        Gizmos.DrawLine(Origin2D.position, defaultQ);

        Gizmos.color = Color.yellow; // yellow is dot product of unit vec
        Gizmos.DrawLine(Origin2D.position, rotatedP);
        Gizmos.DrawLine(Origin2D.position, rotatedQ);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
