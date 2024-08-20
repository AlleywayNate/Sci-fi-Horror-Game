using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoid : MonoBehaviour
{
    public float speed = 3.5f;          // Movement speed of the zombie
    public float rotationSpeed = 5f;    // Speed at which the zombie rotates towards the player
    public float detectionRadius = 10f; // How far the zombie can detect the player
    public float separationDistance = 1.5f; // Minimum distance from other zombies

    private Transform player;           // Reference to the player's transform
    private Rigidbody rb;               // Rigidbody for physics-based movement

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Assumes the player has the "Player" tag
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player != null)
        {
            // Move towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 velocity = direction * speed;

            // Apply separation from other zombies
            Vector3 separation = Vector3.zero;
            foreach (var zombie in GameObject.FindGameObjectsWithTag("Zombie"))
            {
                if (zombie != this.gameObject)
                {
                    float distance = Vector3.Distance(transform.position, zombie.transform.position);
                    if (distance < separationDistance)
                    {
                        separation += (transform.position - zombie.transform.position) / distance;
                    }
                }
            }

            // Combine movement towards the player with separation from other zombies
            Vector3 finalVelocity = (velocity + separation.normalized * speed).normalized * speed;
            rb.MovePosition(rb.position + finalVelocity * Time.deltaTime);

            // Rotate towards the player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
    }
}
