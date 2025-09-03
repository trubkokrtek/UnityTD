using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileTurret;
    public TurretBlueprint machineGunTurret;
    public TurretBlueprint laserTurret;
    public TurretBlueprint sniperTurret;
    public TurretBlueprint freezeTurret;

    void Start()
    {
        buildManager = BuildManager.instance;
    }
    public void SelectClassicTurret()
    {
        Debug.Log("Turret selected");
        buildManager.SetTurret(standardTurret);
    }
    public void SelectMissileTurret()
    {
        Debug.Log("Missile Turret selected");
        buildManager.SetTurret(missileTurret);
    }
    public void SelectMachineGunTurret()
    {
        Debug.Log("Machine Gun Turret selected");
        buildManager.SetTurret(machineGunTurret);
    }
    public void SelectLaserTurret()
    {
        Debug.Log("Laser Turret selected");
        buildManager.SetTurret(laserTurret);
    }
    public void SelectSniperTurret()
    {
        Debug.Log("Sniper Turret selected");
        buildManager.SetTurret(sniperTurret);
    }
    public void SelectFreezeTurret()
    {
        Debug.Log("Freeze Turret selected");
        buildManager.SetTurret(freezeTurret);
    }
}
