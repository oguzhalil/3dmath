using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class BallisticDemo : MonoBehaviour
{
    public Transform launchLocation;
    public Transform launchLocation_Custom;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 force0 = new Vector3(0f, 5f, 20f);
            Vector3 force1 = new Vector3(0f, -2.5f, -10f);
            // Create unity physics projectile
            var sphere_0 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere_0.GetComponent<MeshRenderer>().material.color = Color.red;
            sphere_0.transform.position = launchLocation.transform.position;
            var rb = sphere_0.AddComponent<Rigidbody>();
            rb.AddForce(force0, ForceMode.Impulse);
            rb.AddForce(force1, ForceMode.Impulse);

            rb.angularDrag = 0f;
            rb.drag = 1f;
            rb.useGravity = true;

            // Create custom physics projectile
            var sphere_1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere_1.transform.position = launchLocation_Custom.transform.position;
            sphere_1.GetComponent<MeshRenderer>().material.color = Color.blue;

            var particle = sphere_1.AddComponent<Particle>();
            particle.SetPosition(launchLocation_Custom.transform.position);

            particle.SetMass(1f);
            particle.SetVelocity(force0 + force1);

            particle.SetAcceleration(Physics.gravity);
            particle.SetDamping(1f);
        }
    }
}
