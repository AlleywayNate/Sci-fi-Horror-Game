using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoid : MonoBehaviour
{
    public float speed = 3.5f;
    public float rotationSpeed = 5f;
    public float detectionRadius = 10f;
    public float separationDistance = 1.5f;

    private Transform player;
    private Rigidbody rb;
    private Vector3 lastKnownPlayerPosition;
    private bool isPlayerDetected = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Detect the player if within the detection radius
            if (distanceToPlayer <= detectionRadius)
            {
                lastKnownPlayerPosition = player.position; // Store the last known position
                isPlayerDetected = true;
            }
        }

        // Chase the last known position of the player
        if (isPlayerDetected)
        {
            ChaseLastKnownPosition();
        }
    }

    void ChaseLastKnownPosition()
    {
        // Calculate the direction to the last known player position
        Vector3 direction = (lastKnownPlayerPosition - transform.position).normalized;

        // Calculate the velocity to move in the direction
        Vector3 velocity = direction * speed;

        // Apply separation logic to avoid boids clustering
        Vector3 separation = Vector3.zero;
        foreach (var zombie in GameObject.FindGameObjectsWithTag("Zombie"))
        {
            if (zombie != this.gameObject)
            {
                // Calculate the distance to each other zombie
                float distance = Vector3.Distance(transform.position, zombie.transform.position);

                // If the distance is less than the separation distance, add a separation force
                if (distance < separationDistance)
                {
                    // Calculate the separation force as the normalized direction from this zombie to the other
                    // multiplied by the distance (so closer zombies have a stronger force)
                    separation += (transform.position - zombie.transform.position) / distance;
                }
            }
        }

        // Calculate the final velocity by adding the separation force to the original velocity
        Vector3 finalVelocity = (velocity + separation.normalized * speed).normalized * speed;

        // Move the boid to the new position
        rb.MovePosition(rb.position + finalVelocity * Time.deltaTime);

        // Rotate the boid to face the direction it's moving
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));

        // Stop chasing if the boid reaches the last known position
        if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 0.5f)
        {
            isPlayerDetected = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, separationDistance);
    }
}
