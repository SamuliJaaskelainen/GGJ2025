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
    public Transform player;
    public List<Biome> biomes = new List<Biome>();
    public int currentBiome = 0;
    public int spawnCount = 100;
    public int extraPiecesCount = 5;

    float nextPieceSpawnY = 0.0f;
    List<GameObject> spawnedPieces = new List<GameObject>();
    List<GameObject> extraPieces = new List<GameObject>();
    bool spawnMorePieces = false;


    void Start()
    {
        for(int i = 0; i < spawnCount; ++i)
        {
            SpawnNextPiece();
        }
    }

    void Update()
    {
        if (player.position.y < spawnedPieces[spawnedPieces.Count - 1].transform.position.y)
        {
            if(extraPieces.Count > 0)
            {
                for (int i = 0; i < extraPieces.Count; ++i)
                {
                    Destroy(extraPieces[i]);
                }
            }
            extraPieces.Clear();

            for (int i = 0; i < spawnCount - extraPiecesCount; ++i)
            {
                Destroy(spawnedPieces[i]);
            }

            for (int i = spawnCount - extraPiecesCount; i < spawnCount; ++i)
            {
                extraPieces.Add(spawnedPieces[i]);
            }

            spawnedPieces.Clear();

            for (int i = 0; i < spawnCount; ++i)
            {
                SpawnNextPiece();
            }
        }
    }

    void SpawnNextPiece()
    {
        LevelPiece nextPiece = biomes[currentBiome].pieces[Random.Range(0, biomes[currentBiome].pieces.Length)];
        spawnedPieces.Add(Instantiate(nextPiece.prefab, new Vector3(0.0f, nextPieceSpawnY, 0.0f), Quaternion.identity));
        nextPieceSpawnY -= nextPiece.height;
    }
}
