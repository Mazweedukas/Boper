using PurrNet;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public PlayerInputState inputState;

    void Update()
    {
        if (inputState.Debug_1)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoints[0].position, Quaternion.identity);
    }
}
