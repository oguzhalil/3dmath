using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// row matrices
// left-hand side classic 

// m00 m01 m02 // vector.right
// m10 m11 m12 // vector.up
// m20 m21 m22 // vector.forward

// unity uses column major 
// we are using row major..
// important notes
// orthogonal vector's inverse matrix is "transposed matrix"

public struct Matrix3x3
{
    public float m00;
    public float m01;
    public float m02;

    public float m10;
    public float m11;
    public float m12;

    public float m20;
    public float m21;
    public float m22;

    // Transpose
    // m00 m01 m02     m00 m10 m20
    // m10 m11 m12 --> m01 m11 m21
    // m20 m21 m22     m02 m12 m22

    public Matrix3x3 Transpose()
    {
        Matrix3x3 mat = new Matrix3x3();

        mat.m00 = m00;
        mat.m01 = m10;
        mat.m02 = m20;

        mat.m10 = m01;
        mat.m11 = m11;
        mat.m12 = m21;

        mat.m20 = m02;
        mat.m21 = m12;
        mat.m22 = m22;

        return mat;
    }

    // 3x3 * 1x3 = 3x1 (3x1 - 3D Vector)
    // mxn * nxm = nxn
    // m00 m01 m02   v.x   (m00*v.x + m01*v.y + m02*v.z) 
    // m10 m11 m12 * v.y = (m10*v.x + m11*v.y + m12*v.z) 
    // m20 m21 m22   v.z   (m20*v.x + m21*v.y + m22*v.z) 
    public static Vector3 operator *(Matrix3x3 a, Vector3 v)
    {
        float x = a.m00 * v.x + a.m01 * v.y + a.m02 * v.z;
        float y = a.m10 * v.x + a.m11 * v.y + a.m12 * v.z;
        float z = a.m20 * v.x + a.m21 * v.y + a.m22 * v.z;

        return new Vector3(x, y, z);
    }

    // m00 m01 m02     p.x p.y p.z // pitch
    // m10 m11 m12 --> q.x q.y q.z // yaw
    // m20 m21 m22     r.x r.y r.z // roll
    public Matrix3x3(Vector3 row01, Vector3 row02, Vector3 row03)
    {
        m00 = row01.x;
        m01 = row01.y;
        m02 = row01.z;

        m10 = row02.x;
        m11 = row02.y;
        m12 = row02.z;

        m20 = row03.x;
        m21 = row03.y;
        m22 = row03.z;
    }

    public static Matrix3x3 Identity()
    {
        Matrix3x3 identity = new Matrix3x3();
        
        identity.m00 = identity.m11 = identity.m22 = 1;
        
        return identity;
    }
}
