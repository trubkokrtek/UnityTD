using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Turret
{
    [Header("Sniper Attributes")]
    public float focusTime = 1f;
    public float baseFocusTime = 1f;
    public float focusTimer = 0f;
    public bool targetLocked = false;
    public Transform lockedTarget;
    [Header("Sniper Upgrade")]
    public float focusTimeUpgrade = 0.1f;
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
        focusTime = baseFocusTime;
        targetLocked = false;
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
        try
        {
            if (!targetLocked && Vector3.Distance(target.position, this.transform.position) <= range)
            {
                focusTimer = focusTime;
                lockedTarget = target;
                targetLocked = true;
            }
            if (targetLocked)
            {
                if (focusTimer <= 0f && Vector3.Distance(lockedTarget.position, this.transform.position) <= range)
                {
                    Shoot();
                    targetLocked = false;
                }
                else if(focusTimer <= 0f && Vector3.Distance(lockedTarget.position, this.transform.position) > range)
                {
                    targetLocked = false;
                    focusTimer = focusTime;
                }

                focusTimer -= Time.deltaTime;
                if(lockedTarget == null)
                {
                    targetLocked = false;
                }
            }
        
            Vector3 dir = lockedTarget.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        catch (MissingReferenceException)
        {
            targetLocked = false;
            return;
        }


    }
    public override void Upgrade()
    {
        base.Upgrade();
        focusTime -= focusTimeUpgrade;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    void OnMouseEnter()
    {
        Debug.Log("Turret Clicked");
        rangeIndicator = Instantiate(rangeIndicatorPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.Euler(-90, 0, 0), transform);
        rangeIndicator.transform.localScale = new Vector3(range * 20000, range * 20000, 2000);
    }
   void OnMouseExit()
   {
        Debug.Log("Mouse Exit");
        Destroy(rangeIndicator.gameObject);
   }
}
