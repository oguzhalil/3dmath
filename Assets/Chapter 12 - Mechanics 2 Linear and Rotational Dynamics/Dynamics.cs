using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamics : MonoBehaviour
{
    // find the required momentum to stop the cube.
    public Rigidbody momentumCube;


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {


        // find the momentum of the cube and reverse it to stop the cube
        // momentum = mass * velocity;
        // velocity = a * delta_time;
        // accelaration = v / t * t 
        momentumCube.AddForce(Vector3.forward * 5f, ForceMode.Force);
        momentumCube.AddForce(-Vector3.forward * 5f, ForceMode.Force);
    }
}
