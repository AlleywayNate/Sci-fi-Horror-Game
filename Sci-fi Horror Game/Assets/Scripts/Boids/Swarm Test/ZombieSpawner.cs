using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int spawnCount = 10;
    public float spawnRadius = 30f; // Increased radius to spawn further from the player
    public float safeDistance = 15f; // Minimum distance from player
    public Transform player; // Reference to the player

    void Start()
    {
        SpawnZombies();
    }

void SpawnZombies()
{
    for (int i = 0; i < spawnCount; i++)
    {
        Vector3 spawnPosition = GetValidSpawnPosition();
        
        Debug.Log($"Attempting to spawn zombie at: {spawnPosition}");

        if (spawnPosition != Vector3.zero)
        {
            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"Zombie instantiated at: {spawnPosition} with ID: {zombie.GetInstanceID()}");
        }
        else
        {
            Debug.Log("Failed to find a valid position for zombie.");
        }
    }
}

Vector3 GetValidSpawnPosition()
{
    Vector3 randomDirection;
    Vector3 spawnPosition = Vector3.zero;
    bool positionFound = false;

    for (int attempt = 0; attempt < 10; attempt++)
    {
        randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0; // Keep spawn on the ground level
        spawnPosition = player.position + randomDirection;

        Debug.Log($"Attempting position: {spawnPosition}");

        if (Vector3.Distance(spawnPosition, player.position) >= safeDistance && !IsInPlayerView(spawnPosition))
        {
            positionFound = true;
            Debug.Log($"Valid spawn position found: {spawnPosition}");
            break;
        }
        else
        {
            Debug.Log($"Invalid position: {spawnPosition}");
        }
    }

    return positionFound ? spawnPosition : Vector3.zero;
}



    bool IsInPlayerView(Vector3 position)
    {
        Vector3 directionToTarget = position - player.position;
        float angle = Vector3.Angle(player.forward, directionToTarget);

        // Check if the spawn position is within the player's field of view (e.g., 60 degrees)
        if (angle < 60f)
        {
            RaycastHit hit;
            if (Physics.Raycast(player.position, directionToTarget, out hit))
            {
                // If the ray hits something other than the terrain, the position is not in direct view
                return hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain");
            }
        }

        return false;
    }

        private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Draw the spawn radius around the player
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.position, spawnRadius);

        // Draw the safe distance around the player
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, safeDistance);

        // Draw potential spawn positions within the radius
        Gizmos.color = Color.green;
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0; // Keep spawn on the ground level
        Vector3 spawnPosition = player.position + randomDirection;

        if (Vector3.Distance(spawnPosition, player.position) >= safeDistance && !IsInPlayerView(spawnPosition))
        {
            Gizmos.DrawWireSphere(spawnPosition, 1f); // Visualize potential spawn positions
        }
    }
}
