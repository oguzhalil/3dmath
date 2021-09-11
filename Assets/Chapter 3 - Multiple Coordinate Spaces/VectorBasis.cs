using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3.3.3 Basis Vectors
// Basis Vectors are forward right up vecs v = x * world.right + y * world.up * + world.forward * z; // so we did translate x y z vec to world coordinate
// used for translation of one coordinate space to another
[ExecuteInEditMode]
public class VectorBasis : MonoBehaviour
{
    public Transform origin;
    public Transform arbitrary;

    public Transform upRightSpace;

    Dictionary<string, Vector3> orthogonal = new Dictionary<string, Vector3>()
    {
        { "a1" , new Vector3(1,0,0)  },
        { "a2" , new Vector3(0,0,4)  },
        { "a3" , new Vector3(0,2,0)  },

        { "b1" , new Vector3(1,2,3)  },
        { "b2" , new Vector3(-1,2,3)  },
        { "b3" , new Vector3(1-2,3)  },

        { "c1" , new Vector3(0,4,1)  },
        { "c2" , new Vector3(0,-1,4)  },
        { "c3" , new Vector3(8,0,0)  }

    };

    Dictionary<string, Vector3> orthonormal = new Dictionary<string, Vector3>()
    {
        { "a1" , new Vector3(1,0,0)  },
        { "a2" , new Vector3(0,0,4)  },
        { "a3" , new Vector3(0,2,0)  },

        { "b1" , new Vector3(1,2,3)  },
        { "b2" , new Vector3(-1,2,3)  },
        { "b3" , new Vector3(1-2,3)  },

        { "c1" , new Vector3(1,0,0)  },
        { "c2" , new Vector3(0,0,-1)  },
        { "c3" , new Vector3(0,1,0)  }

    };


    // Use this for initialization
    void Start()
    {
       

        print(IsOrthogonal(orthogonal["a1"] , orthogonal["a2"], orthogonal["a3"]));
        print(IsOrthogonal(orthogonal["b1"], orthogonal["b2"], orthogonal["b3"]));
        print(IsOrthogonal(orthogonal["c1"], orthogonal["c2"], orthogonal["c3"]));

        print("Orthonormal " + IsOrthonormal( orthonormal["a1"], orthonormal["a2"], orthonormal["a3"]));
        print("Orthonormal " + IsOrthonormal(orthonormal["b1"], orthonormal["b2"], orthonormal["b3"]));
        print("Orthonormal " + IsOrthonormal(orthonormal["c1"], orthonormal["c2"], orthonormal["c3"]));


        // w = o + bxp + byq

        //Vector3 arbitraryWorld = origin.position + (arbitrary.localPosition.x * origin.right) + (arbitrary.localPosition.y * origin.up) + (arbitrary.localPosition.z * origin.forward);

        //print(arbitraryWorld);
        //print(arbitrary.position);

        origin.up = new Vector3(0, 1, 0);
        origin.right = new Vector3(.866f, 0, -.5f);
        //origin.forward = new Vector3(.5f, .866f);

        Vector3 arbitraryWorld = origin.position + (-1 * origin.right) + (2 * origin.up) + (0 * origin.forward);

        print(origin.position);

        print(-1 * origin.right);
        print(2 * origin.up);

        Vector3 upRightSpace = origin.right * -1 + origin.up * 2;

        this.upRightSpace.position = origin.position + upRightSpace;

        print(arbitraryWorld);
        print(arbitrary.position);
        print(upRightSpace);

        Debug.LogFormat("{0} , {1} , {2} ", arbitraryWorld.x, arbitraryWorld.y, arbitraryWorld.z);

        //print(arbitrary.position);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // false dependent
    // true independent
    bool IsLinearlyIndependent(Vector3 a, Vector3 b, Vector3 c)
    {
        // if vecs cant expressed by x and y then linearly independent if can independent

        return false;
    }

    // false not orthogonal
    // true is orthogonal
    bool IsOrthogonal(Vector3 a, Vector3 b, Vector3 c)
    {
        // if vectors are perpendicular to each other
        // if dot product length 0 then they are perpendicular because there is no projection

        if (Vector3.Dot(a, b) == 0 && Vector3.Dot(a, c) == 0 && Vector3.Dot(b, c) == 0)
            return true;

        return false;
    }


    // false not orthonormal
    // true is orthonormal
    bool IsOrthonormal( Vector3 a , Vector3 b , Vector3 c )
    {
        // if vectors are perpendicular to each other and unit vectors
        if (!IsOrthogonal(a, b, c))
        {
            Debug.Log("Not perpendicular");
            return false;
        }

        if (a.magnitude == 1.0f && b.magnitude == 1.0f && c.magnitude == 1.0f)
            return true;

        return false;
    }


    void drawAxis( Transform origin )
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin.position, origin.position + origin.right * 5f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin.position, origin.position + origin.up * 5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin.position, origin.position + origin.forward * 5f);

        Gizmos.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(origin.position, .1f);
        Gizmos.DrawSphere(arbitrary.position, .1f);


        // X - Y - Z Axis

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(origin.position, origin.position + origin.right * 5f);

        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(origin.position, origin.position + origin.up * 5f);


        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(origin.position, origin.position + origin.forward* 5f);

        //Gizmos.color = Color.white;

        drawAxis(origin);
        drawAxis(arbitrary);

            drawAxis(upRightSpace);

    }
}
