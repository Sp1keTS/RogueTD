using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoUpgradeConfig", menuName = "Research Tree/Configs/Upgrades/Ammo")]
public class AmmoUpgradeConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseAmmoMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.08f;
    [SerializeField] private float regenerationBonus = 0.1f;
    [SerializeField, TextArea(3, 5)] private string description = "Increases maximum ammo and regeneration rate.";

    public float BaseAmmoMultiplier => baseAmmoMultiplier;
    public float RankBonusPerLevel => rankBonusPerLevel;
    public float RegenerationBonus => regenerationBonus;
    public string Description => description;

    public override TreeNode CreateNode(int  rank)
    {
        return new NodeAmmoUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>(); 
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return treeNodeConfig is TowerNodeConfig || treeNodeConfig is ProjectileTowerNodeConfig;
    }
}