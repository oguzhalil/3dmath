using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CartesianSpace : MonoBehaviour
{
    public delegate int func(int a);

    func f;

    /*
        bool comp(int i, int j) { return i > j; }
        sort(numbers.begin(), numbers.end(), comp);
    */

    int formula(int i)
    {
        return i * 2;
    }

    float toDegree( float radian )
    {
        return radian * (180.0f / Mathf.PI);
    }

    float toRadian( float degree )
    {
        return degree * (Mathf.PI / 180.0f);
    }

    // Use this for initialization
    void Start()
    {
        //Func<int,int> f = formula; C# library
        // 3D Math 1.4.3
        print(Mathf.Rad2Deg * 1f);
        print(toDegree(1f));
        print(toRadian(90f));
        print(Mathf.Deg2Rad * 90f);

        int r = ProductNotation(0, 4, abc);
        int r2 = SummaryNotation(1, 5, formula);

        print(r);
        print(r2);

    } // Pythagorean theorem

    int abc(int a)
    {
        return 7 * (a + 1);
    }

    int ProductNotation(int i, int n , func f)
    {
        int result = 1;

        for (int j = i; j <= n; j++)
        {
            result *= f(j);
        }

        return result;
    }

    int SummaryNotation(int i, int n  , func f)
    {
        int result = 0;

        for (int j = i; j <= n; j++)
        {
            result += f(j);
        }

        return result;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
