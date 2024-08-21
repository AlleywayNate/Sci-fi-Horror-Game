using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public Boid boidPrefab;
    public int boidCount = 50;
    public float spawnRadius = 10f;
    public Boid[] boids;

    void Start()
    {
        boids = new Boid[boidCount];
        for (int i = 0; i < boidCount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate(boidPrefab, spawnPosition, Quaternion.identity);
            boids[i] = boid;
        }
    }
}

