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
    public int biomeLenght = 9999;
}

public class LevelGenerator : MonoBehaviour
{
    public Transform player;
    public List<Biome> biomes = new List<Biome>();
    public int currentBiome = 0;
    public int spawnCount = 100;
    public int extraPiecesCount = 5;

    float nextPieceSpawnY = 0.0f;
    List<GameObject> spawnedPieces = new List<GameObject>();
    List<GameObject> extraPieces = new List<GameObject>();
    bool spawnMorePieces = false;
    int biomeProgress;

    void Start()
    {
        if(spawnCount <= extraPiecesCount)
        { 
            Debug.LogError("Spawn count must be greater then extra pieces count!");
        }

        SpawnNextSection();
    }

    void Update()
    {
        if (player.position.y < spawnedPieces[spawnedPieces.Count - 1].transform.position.y)
        {
            //Destroy and clear extra pieces if any
            if(extraPieces.Count > 0)
            {
                for (int i = 0; i < extraPieces.Count; ++i)
                {
                    Destroy(extraPieces[i]);
                }
            }
            extraPieces.Clear();

            // Destroy spawned pieces, save some extras for the transition area
            for (int i = 0; i < spawnCount - extraPiecesCount; ++i)
            {
                Destroy(spawnedPieces[i]);
            }

            // Save extras to a list
            for (int i = spawnCount - extraPiecesCount; i < spawnCount; ++i)
            {
                extraPieces.Add(spawnedPieces[i]);
            }

            // Clear all spawned pieces
            spawnedPieces.Clear();

            // Spawn new section
            SpawnNextSection();
        }
    }

    void SpawnNextPiece()
    {
        LevelPiece nextPiece = biomes[currentBiome].pieces[Random.Range(0, biomes[currentBiome].pieces.Length)];
        spawnedPieces.Add(Instantiate(nextPiece.prefab, new Vector3(0.0f, nextPieceSpawnY, 0.0f), Quaternion.identity));
        nextPieceSpawnY -= nextPiece.height;
        biomeProgress++;

        if(biomeProgress > biomes[currentBiome].biomeLenght)
        {
            Debug.Log("Next biome");
            biomeProgress = 0;
            currentBiome = Mathf.Min(currentBiome++, biomes.Count - 1);
        }
    }

    void SpawnNextSection()
    {
        for (int i = 0; i < spawnCount; ++i)
        {
            SpawnNextPiece();
        }
    }
}
