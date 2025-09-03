using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Enemy
{
    [Header("Healer Attributes")]
    public float healAmount = 10f;
    public float healRange = 5f;
    public float healCooldown = 3f;

    private float healTimer = 0f;   
    void Start()
    {
        speed = baseSpeed;
        target = Waypoints.points[0];
        Health = baseHealth;
        waveSpawner = GameObject.FindWithTag("GameController").GetComponent<WaveSpawner>();
        healTimer = healCooldown;
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
                Stats.Lives = Mathf.Clamp(Stats.Lives, 0, 100000000);
            }
            else
            {
                target = Waypoints.points[waypointIndex];
            }
        }
        speed = baseSpeed;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float leastHealth = Mathf.Infinity;
        GameObject targetEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= healRange && enemy.GetComponent<Enemy>().Health < leastHealth)
            {
                leastHealth = enemy.GetComponent<Enemy>().Health;
                targetEnemy = enemy;
            }
        }
        if(targetEnemy != null && healTimer <= 0f)
        {
            targetEnemy.GetComponent<Enemy>().Heal(healAmount);
            healTimer = healCooldown;
        }
        else
        {
            healTimer -= Time.deltaTime;
        }
        Armor = 0f;
    }
}
