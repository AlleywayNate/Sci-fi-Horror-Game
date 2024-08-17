using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float speed = 2f; // Speed of movement
    public float changeInterval = 2f; // Time between direction changes
    public Vector3 movementRange = new Vector3(10f, 0f, 10f); // Range of movement in X, Y, Z axes

    private Vector3 targetPosition;
    private float timeSinceChange;

    void Start()
    {
        // Initialize target position
        SetRandomTargetPosition();
    }

    void Update()
    {
        // Move the object towards the target position
        MoveTowardsTarget();

        // Check if it's time to change the target position
        timeSinceChange += Time.deltaTime;
        if (timeSinceChange >= changeInterval)
        {
            SetRandomTargetPosition();
            timeSinceChange = 0f;
        }
    }

    void MoveTowardsTarget()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void SetRandomTargetPosition()
    {
        // Generate a new random position within the defined range
        float x = Random.Range(-movementRange.x, movementRange.x);
        float y = Random.Range(-movementRange.y, movementRange.y);
        float z = Random.Range(-movementRange.z, movementRange.z);

        targetPosition = new Vector3(x, y, z);
    }
}
