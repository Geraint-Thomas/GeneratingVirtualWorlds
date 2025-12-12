using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Tree 
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    List<Vector3> verticies = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
    private World world;
    public GameObject treeObject;
    int vertexIndex = 0;
    public Vector3 vec;

    //tree constructor
    public Tree(World _world, Vector3 pos)
    {
        world = _world;
        treeObject = new GameObject();
        vec = pos;
        
        Init();
    }

    //initialise voxels and mesh components
    public void Init()
    {
        
        meshFilter = treeObject.AddComponent<MeshFilter>();
        meshRenderer = treeObject.AddComponent<MeshRenderer>();


        meshRenderer.material = world.material;
        treeObject.transform.SetParent(world.transform);
        treeObject.transform.position = vec;
        treeObject.name = "tree" + treeObject.transform.position.x + ", " + treeObject.transform.position.z;


        //AddVoxelData(treeObject.transform.position);
        int trunkHeight = 5;

        AddTrunk(trunkHeight);
        AddLeaves(trunkHeight);

        createMesh();
    }


    //add voxel data to arrays
    void AddVoxelData(Vector3 pos, int blockType)
    {
        //nested for loop drawing triangles for each 6 sides of cube
        for (int p = 0; p < 6; p++)
        {

                verticies.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                verticies.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                verticies.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                verticies.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);

                AddTexture(world.blockTypes[blockType].GetTextureID(p));

                tris.Add(vertexIndex);
                tris.Add(vertexIndex + 1);
                tris.Add(vertexIndex + 2);
                tris.Add(vertexIndex + 2);
                tris.Add(vertexIndex + 1);
                tris.Add(vertexIndex + 3);
                vertexIndex += 4;

            
        }


    }

    //create mesh from voxel data
    void createMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        treeObject.AddComponent<MeshCollider>();
    }



    void AddTexture(int textureID)
    {
        float y = textureID / VoxelData.TextureAtlasSizeInBlocks;
        float x = textureID - (y * VoxelData.TextureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;

        y = 1f - y - VoxelData.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));

        //need to go back and change how th uvs work in order to make this work
        treeObject.transform.position = new Vector3(0, 0, 0);
    }


    void AddTrunk(int height)
    {
        for (int i = 0; i < height; i++)
        {
            Vector3 newPos = vec + new Vector3(0, i, 0);
            AddVoxelData(newPos, 5);
        }
    }

    void AddLeaves(int height)
    {
        Vector3 top = vec + new Vector3(0, height, 0);

        // simple 3󫢫 blob of leaves
        for (int x = -1; x <= 1; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3 leafPos = top + new Vector3(x, y, z);
                    AddVoxelData(leafPos, 6);
                }
            }
        }
    }

}
