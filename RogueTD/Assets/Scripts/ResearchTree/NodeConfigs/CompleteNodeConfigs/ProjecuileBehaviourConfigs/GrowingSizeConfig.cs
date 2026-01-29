using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrowingSizeConfig", menuName = "Research Tree/Configs/Upgrades/Growing Size")]
public class GrowingSizeConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseGrowthRate = 0.5f;
    [SerializeField] private float baseMaxSize = 3f;
    [SerializeField] private float growthRateIncreasePerRank = 0.2f;
    [SerializeField] private float maxSizeIncreasePerRank = 0.5f;
    [SerializeField, TextArea(3, 5)] private string description = "Projectiles grow over time.";

    public float BaseGrowthRate => baseGrowthRate;
    public float BaseMaxSize => baseMaxSize;
    public float GrowthRateIncreasePerRank => growthRateIncreasePerRank;
    public float MaxSizeIncreasePerRank => maxSizeIncreasePerRank;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new GrowingSizeUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(GrowingSizeMovement)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<GrowingSizeMovement>(treeNodeConfig, typeof(ProjectileTowerNodeConfig));
    }
}