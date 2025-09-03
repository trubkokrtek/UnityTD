using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public Vector3 positionOffset;

    private TurretBlueprint turretToBuild;
    private Node selectedNode;
    public TurretUI turretUI;


    void Awake()
    {
        instance = this;
    }
    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;
        turretUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        turretUI.Hide();
    }
    public void SetTurret(TurretBlueprint turret)
    {
        turretToBuild = turret;
        selectedNode = null;
        turretUI.Hide();
    }
    public TurretBlueprint GetTurret()
    {
        return turretToBuild;
    }
    public void DeselectTurret()
    {
        turretToBuild = null;
    }
}
