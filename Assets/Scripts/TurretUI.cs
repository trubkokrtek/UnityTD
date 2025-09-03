using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurretUI : MonoBehaviour
{

    public GameObject UI;
    private Node target;
    public TextMeshProUGUI upgradeButton;
    public TextMeshProUGUI sellButton;

    public TextMeshProUGUI targetingText;
    private void Start()
    {
        Hide();
    }

    public void SetTarget(Node newTarget)
    {
        target = newTarget;

        transform.position = target.GetBuildPosition() + new Vector3(0,3,0);
        Debug.Log("Setting turret UI active");
        UI.SetActive(true);
        if (target.turretScript.upgradeTier >= 5)
        {
            upgradeButton.text = "UPGRADE\nmax tier";
        }
        else
        {
            upgradeButton.text = "UPGRADE\n$" + target.turretScript.upgradeCost;
        }
        sellButton.text = "SELL\n$" + target.turretScript.sellValue;
        targetingText.text = target.turretScript.priority;
    }
    public void Hide()
    {
        Debug.Log("Hiding turret UI");
        UI.SetActive(false);
    }
    public void Upgrade()
    {
        target.UpgradeTurret();
        if (target.turretScript.upgradeTier >= 5)
        {
            upgradeButton.text = "UPGRADE\nmax tier";
        }
        else
        {
            upgradeButton.text = "UPGRADE\n$" + target.turretScript.upgradeCost;
        }
        sellButton.text = "SELL\n$" + target.turretScript.sellValue;
    }
    public void Sell()
    {
        target.SellTurret();
        Hide();
    }
    public void nextTargeting()
    {
        switch (target.turretScript.priority)
        {
            case "First":
                target.turretScript.priority = "Closest";
                break;
            case "Closest":
                target.turretScript.priority = "Strong";
                break;
            case "Strong":
                target.turretScript.priority = "Smart";
                break;
            case "Smart":
                target.turretScript.priority = "First";
                break;

        }
        targetingText.text = target.turretScript.priority;
    }
    public void previousTargeting()
    {
        switch (target.turretScript.priority)
        {
            case "First":
                target.turretScript.priority = "Smart";
                break;
            case "Closest":
                target.turretScript.priority = "First";
                break;
            case "Strong":
                target.turretScript.priority = "Closest";
                break;
            case "Smart":
                target.turretScript.priority = "Strong";
                break;

        }
        targetingText.text = target.turretScript.priority;
    }
}
