using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelPiece
{
    public GameObject prefab;
    public float height;
}

[System.Serializable]
public class Biome
{
    public LevelPiece[] pieces;
}

public class LevelGenerator : MonoBehaviour
{
    public List<Biome> biomes = new List<Biome>();
    public int currentBiome = 0;
    float nextPieceSpawnY = 0.0f;

    void Start()
    {
        for(int i = 0; i < 100; ++i)
        {
            SpawnNextPiece();
        }
    }

    void Update()
    {
        
    }

    void SpawnNextPiece()
    {
        LevelPiece nextPiece = biomes[currentBiome].pieces[Random.Range(0, biomes[currentBiome].pieces.Length)];
        Instantiate(nextPiece.prefab, new Vector3(0.0f, nextPieceSpawnY, 0.0f), Quaternion.identity);
        nextPieceSpawnY -= nextPiece.height;
    }
}
