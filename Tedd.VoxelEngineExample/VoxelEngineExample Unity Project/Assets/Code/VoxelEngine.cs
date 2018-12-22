using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelEngine : MonoBehaviour
{
    private World _world = new World();
    private System.Random _random = new System.Random();
    public Material Material;

    // Start is called before the first frame update
    void Start()
    {
        // Create GameObject that will hold a Chunk
        var chunkGameObject = new GameObject("Chunk 0, 0, 0");
        chunkGameObject.transform.parent = transform.parent;
        // Add Chunk to GameObject
        var chunk = chunkGameObject.AddComponent<Chunk>();
        // Add chunk to world at position 0, 0, 0
        _world.Chunks.Add(new ChunkId(0, 0, 0), chunk);
        // Set material
        chunkGameObject.GetComponent<MeshRenderer>().material = Material;

    }

    // Update is called once per frame
    void Update()
    {
        // Update a random block to random type
        var x = _random.Next(0, 16);
        var y = _random.Next(0, 16);
        var z = _random.Next(0, 16);
        var voxelType = (UInt16)_random.Next(0, 2);

        _world[x, y, z] = voxelType;
    }
}
