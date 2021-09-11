using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematics : MonoBehaviour
{
    public Transform origin;
    private float startTime;
    private Rigidbody rigidbody;
    private bool firstUpdate;
    private float timer;
    public Calculation calculation = Calculation.FreeFall;
    private Vector3 startPos;

    public enum Calculation
    {
        FreeFall = 0,
        ConstantVelocity = 1
    }

    // Start is called before the first frame update
    void Start()
    {

        float t = 1.339999f;
        float a = Physics.gravity.y;
        // position at t 
        float pos = transform.position.y + (a * t * t) * .5f;
        Debug.Log(pos);

        rigidbody = GetComponent<Rigidbody>();
        startPos = rigidbody.position;
    }
    private void OnCollisionEnter(Collision col)
    {


        // reset the timer
        calculation = Calculation.ConstantVelocity;
        rigidbody.useGravity = false;
        rigidbody.detectCollisions = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.velocity = Vector3.right;
        timer = 0;
    }

    void FixedUpdate()
    {
        // mass should be 1 bc f = m*a
        // but gravity does not care about the mass? right?
        // find the exact moment when sphere hits the origin
        timer += Time.fixedDeltaTime;

        if (timer >= 1.0f)
        {
            Debug.Log(transform.position);
        }

        return;
        // free fall equation no wind etc.
        if (calculation == Calculation.FreeFall)
        {
            float t = timer; // time starts at 0
            float a = Physics.gravity.y; // constant acceleration
            float v0 = 0; // velocity at time = 00
            // free fall equation - velocity at t under constant acceleration
            float velocity = v0 + a * t;

            // total distance travelled under constant acceleration
            // area of triangle base * height / 2
            float totalDistanceTravelled = (t * a * t) * .5f;

            // time of arrival formula when we plug in distance as totalDistanceTravelled its syncs???
            // NOTE BEAWARE OF THE SIGNS SQRT MINUS IS NAN!!!!
            float distance = Vector3.Distance(transform.position, startPos);
            distance = totalDistanceTravelled;
            // optimized formula t2 a2 = t * a anyway?
            // float timeOfArrive = Mathf.Sqrt((distance * 2) / a);
            float timeOfArrive = Mathf.Sqrt((distance * 2 * a)) / a;
            Debug.Log($"Time of Arrival: { timeOfArrive}");
            Debug.Log($"Current time : {t} Current velocity : {rigidbody.velocity.y} Calculated velocity is {velocity} Total Distance Travelled : {totalDistanceTravelled} UnityDistance{distance}");
        }


        // position at constant velocity
        if (calculation == Calculation.ConstantVelocity)
        {
            // motion under constant velocity
            float x0 = 0;
            float x_at_t_ = x0 + 1f * timer; // Vector.Right
            Debug.Log($" Current Position : {rigidbody.position.x} Calculated Position y :{x_at_t_}");
        }

        // IMPORTANT NOTE FIRST timer starts at 0 so calculate should start at 0
        timer += Time.fixedDeltaTime;
    }
}
