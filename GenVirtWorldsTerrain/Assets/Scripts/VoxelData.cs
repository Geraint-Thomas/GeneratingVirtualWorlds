using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class VoxelData 
{

    public static int chunkWidth = 5;
    public static int chunkHeight = 25;

    public static readonly int worldSizeInChunks = 100;

    public static int viewDistanceInChunks = 10;

    public static int worldSizeInVoxels
    {
        get { return worldSizeInChunks * chunkWidth; }
    }

    public static readonly int TextureAtlasSizeInBlocks = 4;
    public static float NormalizedBlockTextureSize
    {
        get { return 1f / (float)TextureAtlasSizeInBlocks; }
    }

    //positions of each vertex
    public static readonly Vector3[] voxelVerts = new Vector3[8] {
            new Vector3(0.0f,0.0f,0.0f),
            new Vector3(1.0f,0.0f,0.0f),
            new Vector3(1.0f,1.0f,0.0f),
            new Vector3(0.0f,1.0f,0.0f),
            new Vector3(0.0f,0.0f,1.0f),
            new Vector3(1.0f,0.0f,1.0f),
            new Vector3(1.0f,1.0f,1.0f),
            new Vector3(0.0f,1.0f,1.0f),
        };


    public static readonly Vector3[] faceChecks = new Vector3[6]
    {
        new Vector3(0.0f,1.0f,0.0f),
        new Vector3(0.0f,-1.0f,0.0f),
        new Vector3(0.0f,0.0f,-1.0f),
        new Vector3(0.0f,0.0f,1.0f),
        new Vector3(-1.0f,0.0f,0.0f),
        new Vector3(1.0f,0.0f,0.0f)
    };

    //direction of vertixies for drawing triangles
    public static readonly int[,] voxelTris = new int[6,4]{

    { 3,7,2,6}, //top face
    { 1,5,0,4}, //bottom face
    { 0,3,1,2}, //front face
    { 5,6,4,7}, // back face
    { 4,7,0,3}, //left face
    { 1,2,5,6}, //right face

        };

    public static readonly Vector2[] voxelUvs = new Vector2[4]
    {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 1.0f)
    };

    
}
