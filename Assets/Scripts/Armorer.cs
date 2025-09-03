using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Armorer : Enemy
{
    [Header("Armorer Attributes")]
    public float armorValue = .25f;
    public float Range = 5f;

    void Start()
    {
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
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= Range)
            {
                enemy.GetComponent<Enemy>().ArmorUp(armorValue);
            }
        }
        Armor = 0f;
    }
}