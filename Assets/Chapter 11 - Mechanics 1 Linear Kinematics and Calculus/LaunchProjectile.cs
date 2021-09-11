using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Vector3 startPos;
    private LinkedList<Vector3> positions = new LinkedList<Vector3>();
    public Vector3 landingPosition;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // reset the position and velocity
            positions.Clear();
            rigidbody.velocity = Vector3.zero;
            rigidbody.useGravity = true;
            transform.position = startPos;
            rigidbody.isKinematic = false;

            // apply every single equations in the book!!!
            Vector3 a = Physics.gravity;
            float speed = Random.Range(7f, 15f);
            float angle = Random.Range(Mathf.PI * .25f, Mathf.PI * .5f);
            Vector3 v0 = new Vector3(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed, 0f);

            // we launched projectile with custom parameters
            // find time of arrival 
            // landing positions
            // apex

            float te = (-2 * v0.y) / a.y;
            landingPosition = new Vector3(te * v0.x, (te * v0.y) + (1f / 2f) * a.y * te * te, 0f);

            Debug.Log($"Time of end {te} Landing Position {landingPosition}");
            timer = Time.time + te;
            //x = p · a, v = v · a, and a = a · a.
            //landingPosition = 
            rigidbody.velocity = v0;
        }

        if (Time.time >= timer)
        {
            rigidbody.isKinematic = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            startPos = transform.position;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(landingPosition, .5f);

        if (Application.isPlaying)
        {
            if (Time.frameCount % 2 == 0)
                positions.AddLast(transform.position);

            foreach (var item in positions)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(item, .1f);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //rigidbody.isKinematic = true;
    }


}
