using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float inverseMass = 1f;
    private float damping = 1f;
    private Vector3 velocity;
    private Vector3 position;
    private Vector3 forceAccum;
    private Vector3 acceleration;

    public void SetMass(float f)
    {
        if (f == 0)
            return;

        inverseMass = 1f / f;
    }

    public void SetPosition(Vector3 pos)
    {
        position = pos;
    }

    public void SetAcceleration(Vector3 acc)
    {
        acceleration = acc;
    }

    public void SetDamping(float d)
    {
        damping = d;
    }

    // refer to 3.3 THE INTEGRATOR 
    // First Order Newton Euler Integration
    // we have 2 things to consider
    // velocity (change in position) 
    // acceleration (change in velocity) 
    public void Integrate(float dt)
    {
#if false
        // Mathematics & Physics For Programmers DannyKodicek PART II. 
        Vector3 s = velocity * dt + acceleration * dt * dt * 0.5f;
        position += s;
        velocity = velocity + dt * acceleration;
#endif
        // POSITION UPDATE : 
        position += velocity * dt;

        // ACCELERATION UPDATE : 
        Vector3 resultingAcc = acceleration;
        resultingAcc += forceAccum * inverseMass;

        // VELOCITY UPDATE : 
        velocity += resultingAcc * dt;

        velocity *= Mathf.Pow(damping, dt);

        ClearAccumulator();
    }

    public void AddForce(Vector3 force)
    {
        forceAccum += force;
    }

    public void ClearAccumulator()
    {
        forceAccum = Vector3.zero;
    }

    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    private void Update()
    {
        Integrate(Time.deltaTime);
        transform.position = position;
    }
}
