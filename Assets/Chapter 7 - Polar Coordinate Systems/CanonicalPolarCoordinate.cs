using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spherical coordinates
// cylinderical coordinates
// 2d polar coordinates



public class CanonicalPolarCoordinate : MonoBehaviour
{
    Polar2D polar;

    public Transform origin;

    // Use this for initialization
    void Start()
    {
        polar = new Polar2D();
        //polar.r = 5;
        //polar.theta = -270f * Mathf.Deg2Rad;

        polar.r = .5f;
        polar.theta = -65f * Mathf.Deg2Rad;


        polar = ToCanonical(polar);

        print("Le" + new Vector2(1, 0).magnitude);
        print(ToCanonical(polar).theta * Mathf.Rad2Deg);
    }

    Polar2D ToCanonical(Polar2D polar)
    {
        if (polar.r == 0.0f)
        {
            polar.theta = 0;
        }
        else
        {
            if (polar.r < 0.0f)
            {
                polar.r = -polar.r;
                polar.theta += Mathf.PI;
            }

            if (Mathf.Abs(polar.theta) > Mathf.PI)
            {
                // @NOTE: Mathf.Floor( -0.25) returns largest integer smaller than f

                polar.theta += Mathf.PI;
                polar.theta -= Mathf.Floor(polar.theta / Mathf.PI * 2) * (Mathf.PI * 2);
                polar.theta -= Mathf.PI;
            }
        }

        return polar;
    }

    private void OnDrawGizmos()
    {
        float radius = .1f;
        Gizmos.color = Color.red;
        float theta = 0;

        float x = radius * Mathf.Cos(theta); // x
        float y = radius * Mathf.Sin(theta); // y

        Vector3 pos = new Vector3(x, y, 0);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;

        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
            newPos += new Vector3(x, y, 0);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }

        Gizmos.DrawLine(pos, lastPos);

        // cos - sin == adj - opp // remember the order
        // cos = adj / hyp
        // sin = opp / hyp

        float nx = polar.r * Mathf.Cos(-polar.theta); // cos(a) = x / r -> x = cos(a) * r;
        float ny = polar.r * Mathf.Sin(polar.theta); // sin(a) = y / r -> sin(a) * r;

        Gizmos.DrawLine(origin.position, origin.position + new Vector3(nx, ny , 0));
    }

    // Update is called once per frame
    void Update()
    {

    }
}

struct Polar2D
{
    public float r;
    public float theta;
}
