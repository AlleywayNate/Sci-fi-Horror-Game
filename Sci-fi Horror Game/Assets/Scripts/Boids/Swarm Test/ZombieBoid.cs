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
    private Vector3 velocity;

    private bool isPlayerDetected = false;
    private float awarenessDelay = 2f; // Time before the zombie fully detects the player

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();

        StartCoroutine(DelayedAwareness());
    }

    IEnumerator DelayedAwareness()
    {
        yield return new WaitForSeconds(awarenessDelay);
        isPlayerDetected = true;
    }

    void Update()
    {
        if (isPlayerDetected && player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 velocity = direction * speed;

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

            Vector3 finalVelocity = (velocity + separation.normalized * speed).normalized * speed;
            rb.MovePosition(rb.position + finalVelocity * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
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