using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunConfig", menuName = "Research Tree/Configs/Turrets/Shotgun")]
public class ShotgunTowerConfig : ProjectileTowerNodeConfig
{
    [Header("Shotgun Settings")]
    [SerializeField] private float projectileCountMultiplier = 0.1f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Close-range turret firing multiple projectiles with high spread.";

    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        var node = new NodeShotgunTurret(this, rank);
        node.ProjectileTowerBlueprint.ProjectileCount += (int)(projectileCountMultiplier * node.ProjectileTowerBlueprint.ProjectileCount * rank);
        return node;
        
    }
    
    public override System.Collections.Generic.List<System.Type> GetConfigResources()
    {
        return new System.Collections.Generic.List<System.Type>();
    }
}