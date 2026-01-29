using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpeedConfig", menuName = "Research Tree/Configs/Upgrades/Projectile Speed")]
public class ProjectileSpeedConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseSpeedMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    [SerializeField, TextArea(3, 5)] private string description = "Increases projectile velocity.";

    public float BaseSpeedMultiplier => baseSpeedMultiplier;
    public float RankBonusPerLevel => rankBonusPerLevel;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new NodePTProjectileSpeedUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>();
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return treeNodeConfig is ProjectileTowerNodeConfig;
    }
}