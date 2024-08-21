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
    public float roamRadius = 20f;
    public float roamTimerMin = 3f;
    public float roamTimerMax = 7f;

    public static List<ZombieBoid> allBoids = new List<ZombieBoid>();
    private Transform player;
    private Rigidbody rb;
    private Vector3 roamDirection;
    private float roamTime;
    private float nextRoamChangeTime;
    private bool isPlayerDetected = false;
    private static Vector3 lastKnownPlayerPosition; // Static variable

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        allBoids.Add(this);
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            bool isWithinDetectionRadius = distanceToPlayer <= detectionRadius;
            bool isWithinFieldOfView = Vector3.Angle(transform.forward, (player.position - transform.position).normalized) < fieldOfViewAngle / 2f;

            if (isWithinDetectionRadius && isWithinFieldOfView)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, detectionRadius))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        lastKnownPlayerPosition = player.position; // Update static variable
                        isPlayerDetected = true;
                        NotifyAllBoids();
                    }
                }
            }
            else
            {
                isPlayerDetected = false;
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

    void NotifyAllBoids()
    {
        foreach (var boid in allBoids)
        {
            boid.isPlayerDetected = true;
            // Correct way to update static variable for all instances
            boid.GetType().GetField("lastKnownPlayerPosition", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, lastKnownPlayerPosition);
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (lastKnownPlayerPosition - transform.position).normalized;
        Vector3 velocity = direction * speed;

        Vector3 separation = Vector3.zero;
        foreach (var zombie in allBoids)
        {
            if (zombie != this)
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
        if (Time.time >= nextRoamChangeTime)
        {
            SetNewRoamDirection();
            ScheduleNextRoamChange();
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

    void ScheduleNextRoamChange()
    {
        nextRoamChangeTime = Time.time + Random.Range(roamTimerMin, roamTimerMax);
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
