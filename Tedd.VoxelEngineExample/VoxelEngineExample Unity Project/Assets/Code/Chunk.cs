using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    private UInt16[] _voxels = new ushort[16 * 16 * 16];
    private MeshFilter _meshFilter;

    private Vector3[] _cubeVertices = new[] {
        new Vector3 (0, 0, 0),
        new Vector3 (1, 0, 0),
        new Vector3 (1, 1, 0),
        new Vector3 (0, 1, 0),
        new Vector3 (0, 1, 1),
        new Vector3 (1, 1, 1),
        new Vector3 (1, 0, 1),
        new Vector3 (0, 0, 1),
    };
    private int[] _cubeTriangles = new[] {
        // Front
        0, 2, 1,
        0, 3, 2,
        // Top
        2, 3, 4,
        2, 4, 5,
        // Right
        1, 2, 5,
        1, 5, 6,
        // Left
        0, 7, 4,
        0, 4, 3,
        // Back
        5, 4, 7,
        5, 7, 6,
        // Bottom
        0, 6, 7,
        0, 1, 6
    };
    private static Vector3[] _cubeNormals = new[]
    {
        Vector3.up,Vector3.up,Vector3.up,
        Vector3.up,Vector3.up,Vector3.up,
        Vector3.up,Vector3.up
    };

    public UInt16 this[int x, int y, int z]
    {
        get { return _voxels[x * 16 * 16 + y * 16 + z]; }
        set { _voxels[x * 16 * 16 + y * 16 + z] = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        RenderToMesh();
    }

    public void RenderToMesh()
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();

        for (var x = 0; x < 16; x++)
        {
            for (var y = 0; y < 16; y++)
            {
                for (var z = 0; z < 16; z++)
                {
                    var voxelType = this[x, y, z];
                    // If it is air we ignore this block
                    if (voxelType == 0)
                        continue;
                    var pos = new Vector3(x, y, z);
                    // Remember current position in vertices list so we can add triangles relative to that
                    var verticesPos = vertices.Count;
                    foreach (var vert in _cubeVertices)
                        vertices.Add(pos + vert); // Voxel postion + cubes vertex
                    foreach (var tri in _cubeTriangles)
                        triangles.Add(verticesPos + tri); // Position in vertices list for new vertex we just added
                    foreach (var normal in _cubeNormals)
                        normals.Add(normal);
                }
            }
        }

        // Apply new mesh to MeshFilter
        var mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles.ToArray(), 0);
        mesh.SetNormals(normals);
        _meshFilter.mesh = mesh;
    }

}
