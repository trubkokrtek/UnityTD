using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss : Enemy
{
    private bool usedCoffee = false;
    private bool usedMonster = false;

    public GameObject Coffee;
    public GameObject Monster;

    void Start()
    {
        Coffee.SetActive(false);
        Monster.SetActive(false);
        speed = baseSpeed;
        target = Waypoints.points[0];
        Health = baseHealth;
        waveSpawner = GameObject.FindWithTag("GameController").GetComponent<WaveSpawner>();
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
                Stats.Lives -= 2;
                waveSpawner.enemiesAlive--;
                Stats.Lives = Mathf.Clamp(Stats.Lives, 0, 1000000000);
            }
            else
            {
                target = Waypoints.points[waypointIndex];
            }
        }
        speed = baseSpeed;
        if (Health <= baseHealth / 2 && !usedCoffee)
        {
            Heal(baseHealth / 10);
            usedCoffee = true;
            Coffee.SetActive(true);
            baseSpeed += 5f;
        }
        else if (Health <= baseHealth / 5 && !usedMonster)
        {
            Heal(baseHealth / 20);
            usedMonster = true;
            Monster.SetActive(true);
            baseSpeed *= 2f;
        }
        Armor = 0f;
    }
}
