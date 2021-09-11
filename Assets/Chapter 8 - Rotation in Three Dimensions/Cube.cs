using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshFilter meshFilter;

    public Mesh mesh;
    public Vector3[] originalVertices;
    public Vector3[] updatedVertices;
    public Vector3 pivot;

    private void Awake()
    {
        mesh = meshFilter.mesh;
        originalVertices = updatedVertices = mesh.vertices;
        pivot = transform.position;
    }

    public void UpdateVertices(Vector3[] verts)
    {
        meshFilter.sharedMesh.vertices = updatedVertices = verts;
    }

    private void OnDrawGizmos()
    {
        transform.DrawName();
    }
}
