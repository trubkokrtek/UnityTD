using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Turret
{
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
            return;  
        }
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(enemyTag))
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().Slow(slowPercent);
            }
        }
    }
    void OnMouseEnter()
    {
        Debug.Log("Turret Clicked");
        rangeIndicator = Instantiate(rangeIndicatorPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.Euler(-90, 0, 0), transform);
        rangeIndicator.transform.localScale = new Vector3(range * 100, range * 100, 10);
    }
    void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        Destroy(rangeIndicator.gameObject);
    }
}
