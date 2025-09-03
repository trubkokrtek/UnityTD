using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;

    public GameObject rangeIndicatorPrefab;

    private Renderer renderer;
    private Color defaultColor;
    public GameObject turret = null;
    public Turret turretScript;
    private GameObject rangeIndicator;

    BuildManager buildManager;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultColor = renderer.material.color;
        buildManager = BuildManager.instance;
    }
    void OnMouseEnter()
    {
        if(buildManager.GetTurret() == null)
        {
            return;
        }
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(Stats.Money < buildManager.GetTurret().cost)
        {
            renderer.material.color = notEnoughMoneyColor;
        }
        else
        {
            renderer.material.color = hoverColor;
        }

        rangeIndicator = Instantiate(rangeIndicatorPrefab, transform.position + new Vector3(0,2f,0), Quaternion.Euler(-90, 0, 0), transform);
        rangeIndicator.transform.localScale = new Vector3(buildManager.GetTurret().prefab.GetComponent<Turret>().range*25f, buildManager.GetTurret().prefab.GetComponent<Turret>().range*25f, 10);

    }
    public void BuildTurret()
    {
        TurretBlueprint turretToBuild = buildManager.GetTurret();
        if (turretToBuild == null)
        {
            Debug.Log("No turret selected");
            buildManager.DeselectNode();
            return;
        }
        if (Stats.Money < turretToBuild.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }
        turret = Instantiate(turretToBuild.prefab, GetBuildPosition(), transform.rotation, this.transform);
        turret.transform.localScale = new Vector3(turret.transform.localScale.x / this.transform.localScale.x, turret.transform.localScale.y, turret.transform.localScale.z / this.transform.localScale.z);
        turretScript = turret.GetComponent<Turret>();
        Stats.Money -= turretToBuild.cost;
        buildManager.DeselectTurret();
        Debug.Log("Turret built! Money left: " + Stats.Money);
    }
    public void UpgradeTurret()
    {
        if (Stats.Money < turretScript.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade that!");
            return;
        }
        if (turretScript.upgradeTier >= 5)
        {
            Debug.Log("Max upgrade tier reached!");
            return;
        }
        Stats.Money -= turretScript.upgradeCost;
        turretScript.Upgrade();
        Debug.Log("Turret upgraded! Money left: " + Stats.Money);
    }
    public void SellTurret()
    {
        Stats.Money += turretScript.sellValue;
        Destroy(turretScript.gameObject);
        turret = null;
        Debug.Log("Turret sold! Money left: " + Stats.Money);
    }

    void OnMouseExit()
    {
        renderer.material.color = defaultColor;
        Destroy(rangeIndicator);
    }
    private void OnMouseDown()
    {
        if (turret != null)
        {
            Debug.Log("Turret is not null");
            buildManager.SelectNode(this);
            return;
        }
        else
        {

            BuildTurret();
        }
        
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + buildManager.positionOffset;
    }
}

