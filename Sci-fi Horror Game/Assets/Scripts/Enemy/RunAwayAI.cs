using UnityEngine;

public class RunAwayAI : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform target; // Assign the player's transform in the inspector
    public float detectionRange = 10.0f; // Adjust this value to set the detection range

    private void Update()
    {
        // Calculate the distance to the target
        float distance = Vector3.Distance(transform.position, target.position);

        // If the target is within the detection range, run away
        if (distance < detectionRange)
        {
            // Calculate the direction away from the target
            Vector3 direction = (transform.position - target.position).normalized;

            // Move away from the target
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}