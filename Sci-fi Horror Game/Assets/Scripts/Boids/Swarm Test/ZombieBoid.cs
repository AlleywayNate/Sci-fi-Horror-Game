using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoid : MonoBehaviour
{
    public float speed = 3.5f;
    public float rotationSpeed = 5f;
    public float detectionRadius = 10f;
    public float separationDistance = 1.5f;
    public float fieldOfViewAngle = 60f;
    public float roamRadius = 20f; // Radius for roaming behavior
    public float roamTimer = 5f; // Time before changing roaming direction

    private Transform player;
    private Rigidbody rb;
    private Vector3 roamDirection;
    private float roamTime;
    private bool isPlayerDetected = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        SetNewRoamDirection();
        roamTime = Time.time;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                if (angle < fieldOfViewAngle / 2f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            isPlayerDetected = true;
                        }
                    }
                }
            }
        }

        if (isPlayerDetected)
        {
            ChasePlayer();
        }
        else
        {
            RoamAround();
        }
    }

    void ChasePlayer()
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

    void RoamAround()
    {
        if (Time.time - roamTime > roamTimer)
        {
            SetNewRoamDirection();
            roamTime = Time.time;
        }

        Vector3 targetPosition = transform.position + roamDirection * speed * Time.deltaTime;
        rb.MovePosition(targetPosition);

        Quaternion targetRotation = Quaternion.LookRotation(roamDirection);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
    }

    void SetNewRoamDirection()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection.y = 0; // Keep on ground level
        roamDirection = randomDirection.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Vector3 forward = transform.forward * detectionRadius;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * forward;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position + leftBoundary, transform.position + rightBoundary);

        // Gizmo for roaming direction
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + roamDirection * roamRadius);
    }
}
