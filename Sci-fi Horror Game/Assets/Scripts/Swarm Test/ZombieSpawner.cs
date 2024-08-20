using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;  // Reference to the Zombie Prefab
    public int spawnCount = 10;      // Number of zombies to spawn
    public float spawnRadius = 10f;  // Radius around the spawner to spawn zombies

    void Start()
    {
        SpawnZombies();
    }

    void SpawnZombies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = 0; // Ensure zombies spawn on the ground
            Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
