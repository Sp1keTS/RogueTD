using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SplitConfig", menuName = "Research Tree/Configs/Upgrades/Split")]
public class SplitConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private int baseSplitCount = 2;
    [SerializeField] private float baseAngle = 30f;
    [SerializeField] private float additionalSplitPerRank = 0.25f;
    [SerializeField] private float angleIncreasePerRank = 10f;
    [SerializeField, TextArea(3, 5)] private string description = "Projectiles split on impact.";

    public int BaseSplitCount => baseSplitCount;
    public float BaseAngle => baseAngle;
    public float AdditionalSplitPerRank => additionalSplitPerRank;
    public float AngleIncreasePerRank => angleIncreasePerRank;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new SplitEffectUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(SplitEffect)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<SplitEffect>(treeNodeConfig, typeof(TowerNodeConfig), typeof(ProjectileTowerNodeConfig));
    }
}