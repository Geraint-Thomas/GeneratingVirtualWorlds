using UnityEngine;

public static class Noise 
{
   
    public static float get2dPerlin(Vector2 position, float offset, float scale)
    {

        return Mathf.PerlinNoise((position.x + 0.1f) / VoxelData.chunkWidth * scale + offset, (position.y + 0.1f) / VoxelData.chunkWidth * scale + offset);
    }

}
