using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageConfig", menuName = "Research Tree/Configs/Upgrades/Damage")]
public class DamageConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseDamageMultiplier = 1.3f;
    [SerializeField] private float rankBonusPerLevel = 0.1f;
    [SerializeField, TextArea(3, 5)] private string description = "Increases damage output.";

    public float BaseDamageMultiplier => baseDamageMultiplier;
    public float RankBonusPerLevel => rankBonusPerLevel;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new NodeDamageUpgrade(this, rank);
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