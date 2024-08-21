using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainZombieController : MonoBehaviour
{
    public GameObject mainZombiePrefab; // Reference to the main zombie prefab
    public GameObject zombieClonePrefab; // Reference to the clone prefab

    private GameObject mainZombie; // Instance of the main zombie

    void Start()
    {
        // Create and disable the main zombie
        if (mainZombiePrefab != null)
        {
            mainZombie = Instantiate(mainZombiePrefab, Vector3.zero, Quaternion.identity);
            mainZombie.SetActive(false); // Disable the main zombie
        }

        // Example of spawning clones
        SpawnZombies();
    }

    void SpawnZombies()
    {
        // Example of how to spawn clones; adjust as needed
        for (int i = 0; i < 5; i++) // Adjust the number of clones
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Instantiate(zombieClonePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
