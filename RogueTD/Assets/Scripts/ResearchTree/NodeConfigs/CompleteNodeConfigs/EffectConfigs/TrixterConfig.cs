using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrixterConfig", menuName = "Research Tree/Configs/Upgrades/Trixter")]
public class TrixterConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private int baseRicochets = 3;
    [SerializeField] private float baseRadius = 5f;
    [SerializeField] private float additionalRicochetsPerRank = 0.34f;
    [SerializeField] private float radiusIncreasePerRank = 1f;
    [SerializeField, TextArea(3, 5)] private string description = "Ricochet to nearest enemy.";

    public int BaseRicochets => baseRicochets;
    public float BaseRadius => baseRadius;
    public float AdditionalRicochetsPerRank => additionalRicochetsPerRank;
    public float RadiusIncreasePerRank => radiusIncreasePerRank;
    public string Description => description;

    public override TreeNode CreateNode(int  rank)
    {
        return new TrixterUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(HomingRicochetEffect)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return 
            !HasEffectOfType<HomingRicochetEffect>(treeNodeConfig, typeof(UpgradeTreeNodeConfig), typeof(ProjectileTowerNodeConfig))
            && HasEffectOfType<RicochetEffect>(treeNodeConfig, typeof(UpgradeTreeNodeConfig), typeof(ProjectileTowerNodeConfig));
    }
}