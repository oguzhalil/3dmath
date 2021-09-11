using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhereIsNPC : MonoBehaviour
{
    public Transform npc;
    public Transform player;

    public Vector3 forward;

    void Start()
    {
        Where();
    }

    void Where()
    {
        // 3D Math Primer Chapter 2 2.7.3 vec from a to b = b - a
        Vector3 v = npc.position - player.position; // first the vector from me to npc

        float f = Vector3.Dot(forward , v); 

        print(f);

        if (f > 0)
        {
            print("NPC is in front of me");
        }
        else if (f == 0)
        {
            print("NPC is in pertincular to me");

        }
        else
            print("NPC is behind me");

    }

}
