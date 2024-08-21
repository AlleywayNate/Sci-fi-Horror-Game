using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;
    public float maxSpeed = 5f;
    public float neighborDistance = 3f;
    public float separationDistance = 1f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float separationWeight = 1.5f;

    BoidManager manager;

    void Start()
    {
        manager = FindObjectOfType<BoidManager>();
        velocity = Random.insideUnitSphere * maxSpeed;
    }

    void Update()
    {
        Vector3 alignment = Align();
        Vector3 cohesion = Cohere();
        Vector3 separation = Separate();

        Vector3 acceleration = alignmentWeight * alignment +
                               cohesionWeight * cohesion +
                               separationWeight * separation;

        velocity += acceleration * Time.deltaTime;

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    Vector3 Align()
    {
        Vector3 averageDirection = Vector3.zero;
        int neighborCount = 0;

        foreach (Boid boid in manager.boids)
        {
            if (boid != this && Vector3.Distance(boid.transform.position, transform.position) < neighborDistance)
            {
                averageDirection += boid.velocity;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            averageDirection /= neighborCount;
            return (averageDirection.normalized * maxSpeed) - velocity;
        }
        return Vector3.zero;
    }

    Vector3 Cohere()
    {
        Vector3 centerOfMass = Vector3.zero;
        int neighborCount = 0;

        foreach (Boid boid in manager.boids)
        {
            if (boid != this && Vector3.Distance(boid.transform.position, transform.position) < neighborDistance)
            {
                centerOfMass += boid.transform.position;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            centerOfMass /= neighborCount;
            return (centerOfMass - transform.position).normalized * maxSpeed - velocity;
        }
        return Vector3.zero;
    }

    Vector3 Separate()
    {
        Vector3 avoidVector = Vector3.zero;
        int neighborCount = 0;

        foreach (Boid boid in manager.boids)
        {
            float distance = Vector3.Distance(boid.transform.position, transform.position);
            if (boid != this && distance < separationDistance)
            {
                avoidVector += (transform.position - boid.transform.position) / distance;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            avoidVector /= neighborCount;
            return avoidVector.normalized * maxSpeed - velocity;
        }
        return Vector3.zero;
    }
}

