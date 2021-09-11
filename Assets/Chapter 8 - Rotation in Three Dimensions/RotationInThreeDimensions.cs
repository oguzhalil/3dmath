using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
 * 
 * // easy to hard, from known to unknown.
    https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Transform/ScriptBindings/Transform.bindings.cs
    Breakdown unity's transform.cs file 
    By exploring these we are saving tons of hours, better we know 
    faster we are...

    // ----- DONE 
    // implement pitch(seperate), yaw(seperate), roll(seperate) functions
    // compare slerp (spherical linear interpolation) and lerp slerpquaternion,lerpquatertion,lerp_euler (afak there is no euler angle slerp well look into it.)    
    // emphasis different between rotation order lhs*rhs vs rhs*lhs
    // find localrotation of some child object by using parent's worldRotation and child's localRotation
    // determine angular displacement is clockwise or not
    // find delta angle between two orientation.

    // ----- TODO
    // utilization of unity's rotation related functions for instance : quatertion.lookrotation and such
    // try to use Lerp for both quaternions and euler angles
    // form a rotation matrix (cosine direction matrix)
    // perform a yaw rotation around pivot point with given space use matrices (you have too)
    // implement custom quaternions library and their usages
    // rotate an object-or-point around custom axis.
    // rotate an object-or-point around custom point.
    // try to replace unity's rotation system (try to copy it)

 */

public class RotationInThreeDimensions : MonoBehaviour
{
    public Transform[] transforms;
    public Circle circle;

    [Header("Pitch,Yaw,Roll Related")]
    public Cube yawCube;
    public Cube pitchCube;
    public Cube rollCube;
    public Matrix3x3 yawMatrix;
    public Matrix3x3 pitchMatrix;
    public Matrix3x3 rollMatrix;
    public Vector3[] yawAlteredVertices;
    public Vector3[] pitchAlteredVertices;
    public Vector3[] rollAlteredVertices;

    [Header("Quaternion vs Euler Lerp (Gimbal Lock)")]
    public Transform sphereQuaternion;
    public Transform sphereEuler;
    public float sphereInterpolateTime;
    public Quaternion quatStart;
    public Quaternion quatEnd;
    public Vector3 eulerStart;
    public Vector3 eulerEnd;
    public int interpolationDir = 1;

    [Header("Find Child Rotation")]
    public Transform parentRot;
    public Transform childRot;
    public Quaternion absRotation;

    [Header("Rotation Order")]
    public Transform rotOrderPitchYaw;
    public Transform rotOrderYawPitch;

    public Transform visualizeQuatSphere;

    [Header("Sign (Direction) of rotation")]
    public Transform clockwiseRot;
    public Transform counterclockwiseRot;

    [Header("Difference of Quaternion")]
    public Transform diffQuaternionA;
    public Transform diffQuaternionB;
    public Transform diffQuaternionD;

    public enum Space
    {
        Upright, // identity rotation
        Local // orientation from upright space
    }

    private void Start()
    {
        // working
        Perform2dRotation(circle, 35);

        PerformYaw(yawCube, 35, Space.Upright);
        PerformPitch(pitchCube, 45, Space.Upright);
        PerformRoll(rollCube, 168, Space.Upright);

        quatStart = quatEnd = Quaternion.identity;
        eulerStart = eulerEnd = Vector3.zero;

        eulerEnd = new Vector3(180f, 0f, 0f);
        quatEnd = Quaternion.Euler(eulerEnd);

        sphereQuaternion.rotation = quatEnd;
        sphereEuler.rotation = quatEnd;

        FindAbsoluteOrientation(parentRot.rotation, childRot.localRotation);

        // ---- Emphasis importance of order.
        // In Unity these rotations are performed around the Z axis, the X axis, and the Y axis, in that order.
        // order is important specially in fixed-frame (world rotation)
        var pitch = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        var yaw = Quaternion.Euler(new Vector3(0f, 90f, 0f));

        rotOrderPitchYaw.rotation *= pitch * yaw;
        rotOrderYawPitch.rotation *= yaw * pitch;

        DetermineRotationDirection();


        // math primer suggest that quaternion multiplication order should be read right to left but it seems like unity swapped the order
        // d*a = b
        // d = b*inv(a)
        
        // unity carry outs left to right so
        // a*d = b
        // d = inv(a)*b
        // find difference between two quaternions such that it rotates a to b
        Quaternion a = diffQuaternionA.rotation;
        Quaternion b = diffQuaternionB.rotation;

#if false
        // left to right vers
        Quaternion d = Quaternion.Inverse(a) * b;
        Quaternion desiredRot = a * d; // orientation of d is equal to b
        diffQuaternionD.rotation = desiredRot;
#else
        // PS if you use right to left notations you can keep using them
        Quaternion d = b * Quaternion.Inverse(a); // find a to b
        Quaternion desiredRot = d * a; // rotate a to b
        diffQuaternionD.rotation = desiredRot;
#endif
    }

    // find sign of rotation around given axis.
    private void DetermineRotationDirection()
    {
        /*
         * cross product gives the sign 
         * dot product gives the angle
        // The smaller of the two possible angles between the two vectors is returned, therefore the result will never be greater than 180 degrees or smaller than -180 degrees.
        // If you imagine the from and to vectors as lines on a piece of paper, both originating from the same point, then the /axis/ vector would point up out of the paper.
        // The measured angle between the two vectors would be positive in a clockwise direction and negative in an anti-clockwise direction.
        public static float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
        {
            float unsignedAngle = Angle(from, to);
            float cross_x = from.y * to.z - from.z * to.y;
            float cross_y = from.z * to.x - from.x * to.z;
            float cross_z = from.x * to.y - from.y * to.x;
            float sign = Mathf.Sign(axis.x * cross_x + axis.y * cross_y + axis.z * cross_z);
            return unsignedAngle * sign;
        } 

        */

        // normal is clockwise rotation (left-hand coordinate system)
        // find direction of pitch angular displacement
        float counterToNormal = Vector3.SignedAngle(counterclockwiseRot.up, clockwiseRot.up, clockwiseRot.right);
        float normalToCounter = Vector3.SignedAngle(clockwiseRot.up, counterclockwiseRot.up, counterclockwiseRot.right);

        Debug.Log($"Counter to normal {counterToNormal}");
        Debug.Log($"Normal to counter {normalToCounter}");
    }

    private void FindAbsoluteOrientation(Quaternion parentRotation, Quaternion localRotation)
    {
        //Quaternion rotation1 = parentRot.rotation;
        //Quaternion rotation2 = childRot.localRotation;

        Quaternion absRotation = parentRotation * localRotation;

        Vector3 x = absRotation * Vector3.right;
        Vector3 y = absRotation * Vector3.up;
        Vector3 z = absRotation * Vector3.forward;
    }

    private void Perform2dRotation(Circle circle, float angle)
    {
        var offset = new Vector3(1f, 1f).normalized * circle.radius;
        Vector3 posInCircle = Vector3.zero + offset;

        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = posInCircle;
        sphere.transform.localScale = .1f * Vector3.one;

#if true
        // this is from John Vince's book Mathematics for Computer Graphics
        // we dont need to know, previous angle (theta) in order to find target angle (theta + beta)
        // since we are storing our rotation in cartesian space. This is way quicker and smarter <3 John Vince.
        float cos = Mathf.Cos(Mathf.Deg2Rad * angle);
        float sin = Mathf.Sin(Mathf.Deg2Rad * angle);

        float x = (cos * posInCircle.x) - (sin * posInCircle.y);
        float y = (sin * posInCircle.x) + (cos * posInCircle.y);

        sphere.transform.position = circle.transform.position + new Vector3(x, y, 0f);
        sphere.name = "Sphere_ManualRotate";
#else
        // this is not working...
        // because we dont have any information about current (theta)rotation of 
        // the point this is just rotation about origin from zero. in order to 
        // rotate with given angle this way has to know (theta + beta)
        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * offset.magnitude;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * offset.magnitude;
        sphere.transform.position = circle.transform.position + new Vector3(x, y, 0f);
#endif
        // unity premade equivalent function 
        // unity did an extra thing for us he even calculate the facing orientation of the sphere
        // unity rotates orientation by given angle and axis
        // RotateAroundInternal(axis, angle * Mathf.Deg2Rad);

        var sphereSecond = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        sphereSecond.transform.position = circle.transform.position + posInCircle;
        sphereSecond.transform.localScale = .1f * Vector3.one;
        sphereSecond.transform.RotateAround(circle.transform.position, Vector3.forward, angle);
        sphereSecond.name = "Sphere_RotateAround";
    }

    private void PerformYaw(Cube cube, float angle, Space space)
    {
        // unity's equivalent of yaw rotation 
        cube.transform.Rotate(new Vector3(0f, angle, 0f), space == Space.Local ? UnityEngine.Space.Self : UnityEngine.Space.World);

        // transform these vertices around given point
        var verts = cube.originalVertices;
        yawAlteredVertices = new Vector3[verts.Length];

        // unitys rotation system starts from top ( sin(90) = 1 )
        float cos = Mathf.Cos((90 + angle) * Mathf.Deg2Rad);
        float sin = Mathf.Sin((90 + angle) * Mathf.Deg2Rad);

        Vector3 row01 = new Vector3(cos, 0f, sin);
        Vector3 row02 = new Vector3(0f, 1f, 0f);
        Vector3 row03 = new Vector3(-sin, 0f, cos);

        yawMatrix = new Matrix3x3(row01, row02, row03);

        // vertices stored at local space and the origin (0,0) (usually center)
        for (int i = 0; i < verts.Length; i++)
        {
            var point = verts[i];
            Vector3 newPoint = yawMatrix * point;
            yawAlteredVertices[i] = newPoint + cube.transform.position;
        }
    }

    private void PerformPitch(Cube cube, float angle, Space space)
    {
        // unity's equivalent of yaw rotation 
        cube.transform.Rotate(new Vector3(angle, 0f, 0f), space == Space.Local ? UnityEngine.Space.Self : UnityEngine.Space.World);

        // transform these vertices around given point
        var verts = cube.originalVertices;
        pitchAlteredVertices = new Vector3[verts.Length];

        float cos = Mathf.Cos((90 + angle) * Mathf.Deg2Rad);
        float sin = Mathf.Sin((90 + angle) * Mathf.Deg2Rad);

        Vector3 row01 = new Vector3(1f, 0f, 0f);
        Vector3 row02 = new Vector3(0f, cos, sin);
        Vector3 row03 = new Vector3(0f, -sin, cos);

        pitchMatrix = new Matrix3x3(row01, row02, row03);

        // vertices stored at local space and the origin (0,0) (usually center)
        for (int i = 0; i < verts.Length; i++)
        {
            var point = verts[i];
            Vector3 newPoint = pitchMatrix * point;
            pitchAlteredVertices[i] = newPoint + cube.transform.position;
        }
    }

    private void PerformRoll(Cube cube, float angle, Space space)
    {
        // unity's equivalent of yaw rotation 
        cube.transform.Rotate(new Vector3(0f, 0f, angle), space == Space.Local ? UnityEngine.Space.Self : UnityEngine.Space.World);

        // transform these vertices around given point
        var verts = cube.originalVertices;
        rollAlteredVertices = new Vector3[verts.Length];

        float cos = Mathf.Cos((90 + angle) * Mathf.Deg2Rad);
        float sin = Mathf.Sin((90 + angle) * Mathf.Deg2Rad);
#if true
        // roll matrix
        Vector3 row01 = new Vector3(cos, -sin, 0f);
        Vector3 row02 = new Vector3(sin, cos, 0f);
        Vector3 row03 = new Vector3(0f, 0f, 1f);
#else
        // by negating sin I achieved counterclockwise rotation 
        Vector3 row01 = new Vector3(cos, sin, 0f);
        Vector3 row02 = new Vector3(-sin, cos, 0f);
        Vector3 row03 = new Vector3(0f, 0f, 1f);
#endif

        rollMatrix = new Matrix3x3(row01, row02, row03);

        // vertices stored at local space and the origin (0,0) (usually center)
        for (int i = 0; i < verts.Length; i++)
        {
            var point = verts[i];
            Vector3 newPoint = rollMatrix * point;
            rollAlteredVertices[i] = newPoint + cube.transform.position;
        }
    }

    private void Update()
    {
        sphereInterpolateTime += Time.deltaTime * interpolationDir;

        sphereQuaternion.rotation = Quaternion.Lerp(quatStart, quatEnd, sphereInterpolateTime);
        sphereEuler.rotation = Quaternion.Euler(Vector3.Lerp(eulerStart, eulerEnd, sphereInterpolateTime));

        if (Mathf.Abs(sphereInterpolateTime) >= 1f)
        {
            interpolationDir = -1;
        }

        if (sphereInterpolateTime <= 0f)
        {
            interpolationDir = +1;
        }

        // Vector3.RotateTowards
    }

    private void OnDrawGizmos()
    {
        foreach (var item in transforms)
        {
            // left hand side coordinates
            // X+ right Y+ up Z+ forward
            // draw identify or home orientation

            // upright space concept is brilliant if you ask me..
            Vector3 origin = item.position;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + Vector3.right);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, origin + Vector3.up);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(origin, origin + Vector3.forward);

        }

        foreach (var item in yawAlteredVertices)
        {
            Gizmos.DrawWireSphere(item, .01f);
        }

        foreach (var item in pitchAlteredVertices)
        {
            Gizmos.DrawWireSphere(item, .01f);
        }

        foreach (var item in rollAlteredVertices)
        {
            Gizmos.DrawWireSphere(item, .01f);
        }

        Handles.ArrowHandleCap(0, sphereEuler.position, sphereEuler.rotation, 1, EventType.Repaint);
        Handles.ArrowHandleCap(0, sphereQuaternion.position, sphereQuaternion.rotation, 1, EventType.Repaint);

        Vector3 x = absRotation * Vector3.right;
        Vector3 y = absRotation * Vector3.up;
        Vector3 z = absRotation * Vector3.forward;

        Utils.DrawHandles(childRot, x, y, z);
    }

}
