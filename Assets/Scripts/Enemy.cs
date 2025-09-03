using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public float baseSpeed = 10f;
    protected float speed = 10f;
    public float baseHealth = 100f;
    public float Health = 100f;
    public int Reward = 50;
    public float Armor = 0f;
    public Image HPBar;

    [Header("Waypoint Setup Fields")]
    public Transform target;
    public int waypointIndex = 0;


    protected WaveSpawner waveSpawner;

    public float DistanceToFinish()
    {
        float distance = 0f;
        for (int i = waypointIndex; i < Waypoints.points.Length; i++)
        {
            float distanceToNext = Vector3.Distance(transform.position, Waypoints.points[i].position);
            distance += distanceToNext;
        }
        return distance;
    }

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
    public virtual void Slow(float amount)
    {
        speed = baseSpeed * (1f - amount);

    }
    public virtual void ArmorUp(float amount)
    {
        Armor = amount;
    }

    public virtual void TakeDamage(float amount)
    {
        Health -= amount * (1 - Armor);
        Mathf.Clamp(Health, 0, baseHealth);
        HPBar.fillAmount = Health / baseHealth;
        if (Health <= 0)
        {
            Stats.Money += Reward;
            Destroy(gameObject);
        }
    }
    public virtual void Heal(float amount)
    {
        Health += amount;
        Mathf.Clamp(Health, 0, baseHealth);
        HPBar.fillAmount = Health / baseHealth;
        if (Health > baseHealth)
        {
            Health = baseHealth;
        }
    }
}
