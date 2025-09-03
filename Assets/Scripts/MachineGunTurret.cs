using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MachineGunTurret : Turret
{
    [Header("Machine Gun Specific")]
    public int baseAmmoCapacity = 100;
    public int AmmoCapacity = 100;
    public int Ammo = 100;
    public float baseReloadTime = 2f;
    public float reloadTime = 2f;
    public float reloadTimeCountdown = 0f;
    public bool reloading = false;
    [Header("Machine Gun Upogrades")]
    public float reloadSpeedUpgrade = 0.5f;
    public int ammoCapacityUpgrade = 10;

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
        AmmoCapacity = baseAmmoCapacity;
        Ammo = AmmoCapacity;
        reloadTime = baseReloadTime;
        reloading = false;
    }

    void Update()
    {
        if (Ammo <= 0 && !reloading)
        {
            reloading = true;
            reloadTimeCountdown = reloadTime;
            return;
        }
        else if (reloading)
        {
            reloadTimeCountdown -= Time.deltaTime;
            if (reloadTimeCountdown <= 0)
            {
                reloading = false;
                Ammo = AmmoCapacity;
            }
            return;
        }
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
        Shoot();
        fireCountdown = 1f / fireRate;
        fireCountdown -= Time.deltaTime;
    }
    void OnMouseEnter()
    {
        Debug.Log("Turret Clicked");
        rangeIndicator = Instantiate(rangeIndicatorPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.Euler(-90, 0, 0), transform);
        rangeIndicator.transform.localScale = new Vector3(range * 1000, range * 1000, 100);
    }
    void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        Destroy(rangeIndicator.gameObject);
    }
    public override void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(target, Damage);
        }
        Ammo--;
    }
    public override void Upgrade()
    {
        sellValue += (int)(0.75 * upgradeCost);
        upgradeTier += 1;
        upgradeCost = (int)(1.5 * upgradeCost);
        range += rangeUpgrade;
        fireRate += fireRateUpgrade;
        Damage += damageUpgrade;
        reloadTime -= reloadSpeedUpgrade;
        AmmoCapacity += ammoCapacityUpgrade;

    }
}
