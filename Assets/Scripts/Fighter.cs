using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Fighter : Enemy
{
    [Header("Fighter Attributes")]
    public GameObject childrenPrefab;
    void Start()
    {
        speed = baseSpeed;
        target = Waypoints.points[0];
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
    public override void TakeDamage(float amount)
    {
        Health -= amount * (1 - Armor);
        Mathf.Clamp(Health, 0, baseHealth);
        HPBar.fillAmount = Health / baseHealth;
        if (Health <= 0)
        {
            Stats.Money += Reward;
            SpawnChild();
            SpawnChild();
            Destroy(gameObject);

        }
    }
    public void SpawnChild()
    {
        GameObject child = Instantiate(childrenPrefab, transform.position, Quaternion.identity);
        child.GetComponent<Enemy>().baseHealth = 0.5f * baseHealth;
        child.GetComponent<Enemy>().Health = 0.5f * baseHealth;
        child.GetComponent<Enemy>().target = target;
        child.GetComponent<Enemy>().waypointIndex = waypointIndex;
    }
}
