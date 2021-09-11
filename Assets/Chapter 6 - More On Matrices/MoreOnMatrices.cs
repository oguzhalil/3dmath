using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreOnMatrices : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Vector3 r1, r2, r3;

        r1 = new Vector3(-0.1495f, -0.1986f, -0.9685f);
        r2 = new Vector3(-0.8256f, 0.5640f, 0.0017f);
        r3 = new Vector3(-0.5439f, -0.8015f, 0.2484f);

        print(Magnitude(r1));
        print(Magnitude(r2));
        print(Magnitude(r3));

        bool b = Magnitude(r1) == 1;

        print(b);
        print(Mathf.Approximately(Magnitude(r1), 1f));

        print(Vector3.Dot(r1, r2).ToString("0.0"));
        print(Vector3.Dot(r1, r3).ToString("0.0"));
        print(Vector3.Dot(r3, r2).ToString("0.0"));

    }

    float Magnitude(Vector3 v)
    {
        return (Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
