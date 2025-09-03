using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("General")]
    public float baseRange = 15f;
    public Transform partToRotate;
    public float baseDamage = 10f;

    [Header("Bullet Attributes")]
    public float turnSpeed = 10f;
    public float baseFireRate = 1f;
    protected float fireCountdown = 0f;

    [Header("Laser Attributes")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public float baseSlowPercent = 0.5f;

    [Header("Upgrade Attributes")]
    public float rangeUpgrade = 2.5f;
    public float damageUpgrade = 5f;
    public float fireRateUpgrade = 0.5f;
    public float slowUpgrade = 10f;
    public int upgradeTier;
    public int baseUpgradeCost = 100;
    public int upgradeCost = 100;
    public int sellValue;

    [Header("Enemy Setup Fields")]
    public string enemyTag = "Enemy";
    protected Transform target;
    protected Enemy enemy;

    [Header("Unity Setup Fields")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject rangeIndicatorPrefab;
    protected GameObject rangeIndicator;

    [Header("Current values")]
    public float Damage;
    public float range;
    public float slowPercent;
    public float fireRate;
    public string priority = "First"; // First or Closest or Smart
    void Start()
    {
        sellValue = (int)(0.75 * upgradeCost);
        Damage = baseDamage;
        range = baseRange;
        slowPercent = baseSlowPercent;
        upgradeCost = baseUpgradeCost;
        fireRate = baseFireRate;
        upgradeTier = 0;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update()
    {
        if (target == null)
        {
            if (useLaser && lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
            }
            return;
        }
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        if (!useLaser) { 
            if (fireCountdown <= 0f && target != null)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
        else
        {
            Laser();
        }

    }
    protected void UpdateTarget()
    {
        if (priority == "First")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float shortestDistanceToEnd = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                if(enemy.GetComponent<Enemy>().DistanceToFinish() < shortestDistanceToEnd && Vector3.Distance(transform.position,enemy.transform.position) <= range)
                {
                    shortestDistanceToEnd = enemy.GetComponent<Enemy>().DistanceToFinish();
                    target = enemy.transform;
                    nearestEnemy = enemy;
                }

            }
            if (nearestEnemy != null)
            {
                target = nearestEnemy.transform;
                enemy = target.GetComponent<Enemy>();
            }
            else
            {
                target = null;
            }
        }
        else if(priority == "Closest")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                float DistanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (DistanceToEnemy < shortestDistance)
                {
                    shortestDistance = DistanceToEnemy;
                    target = enemy.transform;
                    nearestEnemy = enemy;
                }
            }
            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
                enemy = target.GetComponent<Enemy>();
            }
            else
            {
                target = null;
            }
        }
        else if(priority == "Smart")
        {
            if(target != null && Vector3.Distance(transform.position, target.position) <= range)
            {
                return;
            }
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                float DistanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (DistanceToEnemy < shortestDistance)
                {
                    shortestDistance = DistanceToEnemy;
                    target = enemy.transform;
                    nearestEnemy = enemy;
                }
            }
            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
                enemy = target.GetComponent<Enemy>();
            }
            else
            {
                target = null;
            }
        }
        else if(priority == "Strong")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float strongestHealth = 0f;
            GameObject strongestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Enemy>().Health > strongestHealth && Vector3.Distance(transform.position, enemy.transform.position) <= range)
                {
                    strongestHealth = enemy.GetComponent<Enemy>().Health;
                    target = enemy.transform;
                    strongestEnemy = enemy;
                }
            }
            if (strongestEnemy != null)
            {
                target = strongestEnemy.transform;
                enemy = target.GetComponent<Enemy>();
            }
            else
            {
                target = null;
            }
        }
        else
        {
            Debug.LogError("Invalid priority");
        }
    }
    public virtual void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(target, Damage);
        }
    }
    public virtual void Laser()
    {
        enemy.TakeDamage(Damage * Time.deltaTime);
        enemy.Slow(slowPercent);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }
    public virtual void Upgrade()
    {
        sellValue += (int)(0.75 * upgradeCost);
        upgradeTier += 1;
        upgradeCost = (int)(1.5 * upgradeCost);
        range += rangeUpgrade;
        fireRate += fireRateUpgrade;
        if (useLaser)
        {
            slowPercent += slowUpgrade;
        }
        Damage += damageUpgrade;
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    void OnMouseEnter()
    {
        Debug.Log("Turret Clicked");
        rangeIndicator = Instantiate(rangeIndicatorPrefab, transform.position + new Vector3(0,2f,0), Quaternion.Euler(-90, 0, 0), transform);
        rangeIndicator.transform.localScale = new Vector3(range * 100, range * 100, 10);
    }
    void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        Destroy(rangeIndicator.gameObject);
    }
    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        GameObject.FindGameObjectWithTag("TurretUI").GetComponent<TurretUI>().SetTarget(this.transform.parent.GetComponent<Node>());
    }
}

