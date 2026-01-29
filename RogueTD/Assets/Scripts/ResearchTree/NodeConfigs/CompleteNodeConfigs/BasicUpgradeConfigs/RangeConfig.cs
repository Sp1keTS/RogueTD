using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeConfig", menuName = "Research Tree/Configs/Upgrades/Range")]
public class RangeConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseRangeMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    [SerializeField, TextArea(3, 5)] private string description = "Increases tower range.";

    public float BaseRangeMultiplier => baseRangeMultiplier;
    public float RankBonusPerLevel => rankBonusPerLevel;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        
        return new NodeRangeUpgrade(this, rank);
        
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