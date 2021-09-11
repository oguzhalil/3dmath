using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vectors : MonoBehaviour
{
    public Transform pointOne;
    public Transform pointTwo;
    public Transform unitVec;
    public Transform cross1;
    public Transform cross2;

    List<System.Action<Vector3, Vector3>> drawLines;

    float length;

    Vector3 cross;

    public Transform crossOrigin;

    private void Start()
    {
        DistanceBetweenTwoPoint();

        ProjectionOntoUnitVec();

        CrossProduct();

        print(angle(pointOne.position, pointTwo.position));
        print(Vector3.Angle(pointOne.position, pointTwo.position));

        print(Dot(Vector2.one, Vector2.up));
        print(Dot(Vector3.one, Vector3.up));

        print(Vector2.Dot(Vector2.one, Vector2.up));
        print(Vector3.Dot(Vector3.one, Vector3.up));

    }

    // distance between the point

    float Dot(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    float Dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public void DistanceBetweenTwoPoint()
    {
        // distance = |p1 - p2|
        //Vector3 v = pointOne.position + pointTwo.position; // minus or plus gives same result 
        Vector3 v = pointOne.position - pointTwo.position;
        //Vector3 vec = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        float f = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
        //float f = Mathf.Sqrt( v.x * v.x + v.y * v.y + v.z * v.z;


        print(f);
        print(Vector3.Distance(pointOne.position, pointTwo.position));
        v[0] = 1; // v1
    }

    public void ProjectionOntoUnitVec()
    {
        length = Vector3.Dot(unitVec.position, pointOne.position);
    }

    public void CrossProduct() // 2.12 Figure 2.30 
    {
        // a x b = cross2 x cross1

        cross = Vector3.Cross(cross2.localPosition, cross1.localPosition);
    }

    public const float kEpsilonNormalSqrt = 1e-15F; // ??

    // from a.b = ||a|| . ||b|| cosθ
    float angle(Vector3 a, Vector3 b)
    {
        // denominator = payda
        // angle = arccos( a . b / ||a|| . ||b|| )
        float denominator = Mathf.Sqrt((a.x * a.x + a.y * a.y + a.z * a.z) * (b.x * b.x + b.y * b.y + b.z * b.z));
        float f = Vector3.Dot(a, b);

        if (denominator < kEpsilonNormalSqrt)
            return 0f;

        float angle = Mathf.Acos(f / denominator);

        return angle * Mathf.Rad2Deg;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // red is distance formula
        Gizmos.DrawLine(pointOne.position, pointTwo.position);
        Gizmos.DrawSphere(pointOne.position, .1f);
        Gizmos.DrawSphere(pointTwo.position, .1f);

        Gizmos.color = Color.yellow; // yellow is dot product of unit vec
        Gizmos.DrawLine(Vector3.zero, unitVec.position * length);

        Gizmos.color = Color.blue; // blue cross product of vecs
        Gizmos.DrawLine(crossOrigin.position, crossOrigin.position + cross);
        Gizmos.DrawLine(crossOrigin.position, cross1.position);
        Gizmos.DrawLine(crossOrigin.position, cross2.position);

    }
}
