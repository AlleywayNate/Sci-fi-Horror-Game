using UnityEngine;

public class ChaseAI : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform target; // Assign the player's transform in the inspector

    private void Update()
    {
        // Calculate the direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Move towards the target
        transform.position += direction * speed * Time.deltaTime;
    }
}