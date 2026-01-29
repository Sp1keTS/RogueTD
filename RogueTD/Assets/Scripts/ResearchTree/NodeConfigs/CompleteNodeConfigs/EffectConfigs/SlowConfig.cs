using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowConfig", menuName = "Research Tree/Configs/Upgrades/Slow")]
public class SlowEffectConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseSlowPercent = 0.5f;
    [SerializeField] private float baseDuration = 2f;
    [SerializeField] private float slowPercentIncreasePerRank = 0.1f;
    [SerializeField] private float durationIncreasePerRank = 0.5f;
    [SerializeField, TextArea(3, 5)] private string description = "Slows enemy movement speed.";

    public float BaseSlowPercent => baseSlowPercent;
    public float BaseDuration => baseDuration;
    public float SlowPercentIncreasePerRank => slowPercentIncreasePerRank;
    public float DurationIncreasePerRank => durationIncreasePerRank;
    public string Description => description;

    public override TreeNode CreateNode(int  rank)
    {
        return new SlowEffectUpgrade(this,  rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(SlowStatusEffect)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<SlowStatusEffect>(treeNodeConfig, typeof(TowerNodeConfig), typeof(ProjectileTowerNodeConfig));
    }
}