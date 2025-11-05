using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Chunk  
{
    GameObject chunkObject;
    public ChunkCoord coord;

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    int vertexIndex = 0;
    List<Vector3> verticies = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    byte[,,] voxelMap = new byte[VoxelData.chunkWidth, VoxelData.chunkHeight, VoxelData.chunkWidth];

    private World world;

    /*
    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();

        PopulateVoxelMap();

        createMeshData();
        
        createMesh();

    }
    */

    public Chunk (ChunkCoord _coord, World _world)
    {
        world = _world;
        coord = _coord;
        

        chunkObject = new GameObject();
        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
       

        meshRenderer.material = world.material;
        chunkObject.transform.SetParent(world.transform);
        chunkObject.transform.position = new Vector3(coord.x * VoxelData.chunkWidth, 0f, coord.z * VoxelData.chunkWidth);
        chunkObject.name = "Chunk" + coord.x + ", " + coord.z;
        chunkObject.tag = "ground";

        PopulateVoxelMap();

        createMeshData();

        createMesh();
    }

    void AddVoxelDataToChunk(Vector3 pos)
    {
        //nested for loop drawing triangles for each 6 sides of cube
        for (int p = 0; p < 6; p++)
        {
            if(!checkVoxel(pos + VoxelData.faceChecks[p]))
            {
                /*
                for (int i = 0; i < 6; i++)
                {
                    int triangleIndex = VoxelData.voxelTris[p, i];
                    verticies.Add(VoxelData.voxelVerts[triangleIndex] + pos);
                    tris.Add(vertexIndex);

                    uvs.Add(VoxelData.voxelUvs[i]);

                    vertexIndex++;
                }*/

                byte blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];

                verticies.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                verticies.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                verticies.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                verticies.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);

                AddTexture(world.blockTypes[blockID].GetTextureID(p));

                tris.Add(vertexIndex);
                tris.Add(vertexIndex +1);
                tris.Add(vertexIndex +2);
                tris.Add(vertexIndex +2);
                tris.Add(vertexIndex +1);
                tris.Add(vertexIndex +3);
                vertexIndex += 4;


            }
        }
    }

    void createMeshData()
    {
        for (int y = 0; y < VoxelData.chunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.chunkWidth; x++)
            {
                for (int z = 0; z< VoxelData.chunkWidth; z++)
                {
                    if (world.blockTypes[voxelMap[x,y,z]].isSolid)
                        AddVoxelDataToChunk(new Vector3(x, y, z));
                }
            }
        }
    }

    void PopulateVoxelMap()
    {

        for (int y = 0; y < VoxelData.chunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.chunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.chunkWidth; z++)
                {
                    voxelMap[x,y,z] = world.GetVoxel(new Vector3(x,y,z) +position);
                }
            }
        }

    }

    public bool isActive
    {
        get{ return chunkObject.activeSelf; }
        set{ chunkObject.SetActive(value); }
    }

    public Vector3 position
    {
        get{ return chunkObject.transform.position; }
    }

    bool checkVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (!isVoxelInChunk(x, y, z)) //return false;
            return world.blockTypes[world.GetVoxel(pos + position)].isSolid;

        return world.blockTypes[voxelMap[x, y, z]].isSolid;
    }

    bool isVoxelInChunk(int x, int y, int z)
    {
        if (x < 0 || x > VoxelData.chunkWidth - 1 || y < 0 || y > VoxelData.chunkHeight - 1 || z < 0 || z > VoxelData.chunkWidth - 1)
            return false;
        else
            return true;
    }


    void createMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        chunkObject.AddComponent<MeshCollider>();
    }

    void AddTexture(int textureID)
    {
        float y = textureID / VoxelData.TextureAtlasSizeInBlocks;
        float x = textureID - (y * VoxelData.TextureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;

        y = 1f -y -  VoxelData.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y +VoxelData.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));

        //need to go back and change how th uvs work in order to make this work

    }
}

public class ChunkCoord
{
    public int x;
    public int z;

    public ChunkCoord(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public bool Equals(ChunkCoord other)
    {
        if (other == null)
            return false;
        else if (other.x == x && other.z == z)
            return true;
        else 
            return false;
    }
}
