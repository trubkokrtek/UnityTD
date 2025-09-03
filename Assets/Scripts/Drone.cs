using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Drone : Enemy
{
    void Start()
    {
        speed = baseSpeed;
        Health = baseHealth;
        waveSpawner = GameObject.FindWithTag("GameController").GetComponent<WaveSpawner>();
        Armor = 0f;
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        transform.LookAt(target);
        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            waypointIndex++;
            if (waypointIndex >= Waypoints.points.Length)
            {
                Destroy(gameObject);
                Stats.Lives--;
                waveSpawner.enemiesAlive--;
                Stats.Lives = Mathf.Clamp(Stats.Lives, 0, 100000000);
            }
            else
            {
                target = Waypoints.points[waypointIndex];
            }
        }
        speed = baseSpeed;
        Armor = 0f;
        Mathf.Clamp(Health, 0, baseHealth);
    }
}
