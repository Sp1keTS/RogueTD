using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedConfig", menuName = "Research Tree/Configs/Upgrades/Attack Speed")]
public class AttackSpeedConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseSpeedMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.07f;
    [SerializeField, TextArea(3, 5)] private string description = "Increases rate of fire.";

    public float BaseSpeedMultiplier => baseSpeedMultiplier;
    public float RankBonusPerLevel => rankBonusPerLevel;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new NodeAttackSpeedUpgrade(this, rank);
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