using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstShotConfig", menuName = "Research Tree/Configs/Upgrades/Burst Shot")]
public class BurstShotConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private int baseBurstCount = 2;
    [SerializeField] private float baseBurstDelay = 0.1f;
    [SerializeField] private float additionalBurstPerRank = 0.5f;
    [SerializeField] private float delayReductionPerRank = 0.02f;
    [SerializeField, TextArea(3, 5)] private string description = "Fires multiple shots rapidly.";

    public int BaseBurstCount => baseBurstCount;
    public float BaseBurstDelay => baseBurstDelay;
    public float AdditionalBurstPerRank => additionalBurstPerRank;
    public float DelayReductionPerRank => delayReductionPerRank;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new BurstShotBehaviorUpgrade(this,  rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(BurstShotBehavior)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<BurstShotBehavior>(treeNodeConfig, typeof(ProjectileTowerNodeConfig));
    }
}