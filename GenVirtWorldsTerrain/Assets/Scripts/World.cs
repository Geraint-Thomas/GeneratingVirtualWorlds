using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;


public class World : MonoBehaviour
{
    public int seed;
    public Material material;

    public BlockType[] blockTypes;

    public Transform player;
    public Vector3 spawn;

    Chunk[,] chunks = new Chunk[VoxelData.worldSizeInChunks, VoxelData.worldSizeInChunks];

    List<ChunkCoord> activeChunks = new List<ChunkCoord>();
    ChunkCoord playerLastChunkCoord;
    ChunkCoord playerChunkCoord;
    public float noiseScale = 0.25f;
    public int grassHeight = 20;
    public int dirtHeight = 10;
    //public int stoneHeight = 1;

    private void Start()
    {

        UnityEngine.Random.InitState(seed);

        //Chunk chunk = new Chunk(new ChunkCoord(0,0), this);
        spawn = new Vector3((VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2f, VoxelData.chunkHeight +2, (VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2f);
        GenerateWorld();
        playerLastChunkCoord = GetChunkCoord(player.position);
        
    }

    private void Update()
    {
        //if the player has changed chunkCoord then run check view distance 
        playerChunkCoord = GetChunkCoord(player.position);
        if(!playerChunkCoord.Equals(playerLastChunkCoord))
            CheckViewDistance();
    }

    ChunkCoord GetChunkCoord(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.chunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.chunkWidth);
        return new ChunkCoord(x, z);
    }

    void CheckViewDistance()
    {
        ChunkCoord coord = GetChunkCoord(player.position);
        //makes new list containing all active coords
        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);

        for (int x = coord.x - VoxelData.viewDistanceInChunks; x<coord.x +VoxelData.viewDistanceInChunks; x++)
        {
            for (int z = coord.z - VoxelData.viewDistanceInChunks; z < coord.z + VoxelData.viewDistanceInChunks; z++)
            {
                if(IsChunkInWorld(new ChunkCoord(x,z)))
                {
                    if (chunks[x,z] == null)
                    {
                        CreateNewChunk(x, z);
                    }
                    else if (!chunks[x,z].isActive)
                    {
                        chunks[x,z].isActive = true;
                        activeChunks.Add(new ChunkCoord(x, z));
                    }
                }

                //checking if chunjks are inactive
                for(int i =0; i<previouslyActiveChunks.Count; i++)
                {
                    //any chunks in view distance are removed from list
                    if (previouslyActiveChunks[i].Equals(new ChunkCoord(x, z)))
                        previouslyActiveChunks.RemoveAt(i);
                }


            }
        }

        //loops over inactive chunks and sets them to false
        foreach(ChunkCoord cc in previouslyActiveChunks)
        {
            chunks[cc.x, cc.z].isActive = false;
        }

    }

    void GenerateWorld()
    {
        for(int x = (VoxelData.worldSizeInChunks / 2) - VoxelData.viewDistanceInChunks; x < (VoxelData.worldSizeInChunks / 2) + VoxelData.viewDistanceInChunks; x++)
        {
            for (int z = (VoxelData.worldSizeInChunks / 2) - VoxelData.viewDistanceInChunks; z < (VoxelData.worldSizeInChunks / 2) + VoxelData.viewDistanceInChunks; z++)
            {
                CreateNewChunk(x, z);
            }
        }
        player.position = spawn;
    }

    public byte GetVoxel(Vector3 pos)
    {

        int yPos = Mathf.FloorToInt(pos.y);

        //Immutable pass//
        //outside world is air
        if (!IsVoxelInWorld(pos))
            return 0;
        //bottom is bedrock
        if (yPos == 0)
            return 1;

        //basic terrain pass//
        int terrainHeight = Mathf.FloorToInt(Noise.get2dPerlin(new Vector2(pos.x, pos.z), 193, noiseScale) * VoxelData.chunkHeight);
        /*
        if (yPos <= terrainHeight)
            return 2;
        else                        //THIS IS WHERE YOU LEFT OFF
            return 0;

        */
        
        if (yPos == 0)
           return  1;
        else if (yPos <= terrainHeight &&yPos >= grassHeight)
           return 3;
        else if (yPos <= terrainHeight&&yPos >= dirtHeight)
            return 4;
        else if (yPos <= terrainHeight&&yPos < dirtHeight)
            return 2;
        else
            return 0;
        
    }


    void CreateNewChunk(int x, int z)
    {
        chunks[x, z] = new Chunk(new ChunkCoord(x, z), this);
        activeChunks.Add(new ChunkCoord(x, z));
    }

    bool IsChunkInWorld(ChunkCoord coord)
    {
        if (coord.x > 0 && coord.x < VoxelData.worldSizeInChunks - 1 && coord.z > 0 && coord.z < VoxelData.worldSizeInChunks - 1)
            return true;
        else
            return false;
    }

    bool IsVoxelInWorld(Vector3 pos)
    {
        if(pos.x >=0 && pos.x < VoxelData.worldSizeInVoxels && pos.y >=0 && pos.y <VoxelData.chunkHeight && pos.z >=0 && pos.z < VoxelData.worldSizeInVoxels)
            return true;
        else 
            return false;
    }

}


[System.Serializable]
public class BlockType
{
    public string blockName;
    public bool isSolid;

    [Header("Texture Values")]
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int frontFaceTexture;
    public int backFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;

    public int GetTextureID(int faceIndex)
    {
        switch(faceIndex)
        {
            case 0:
                return topFaceTexture;
            case 1:
                return bottomFaceTexture;
            case 2:
                return frontFaceTexture;
            case 3:
                return backFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                return 0;
            
        }
    }
}