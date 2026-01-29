using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RicochetConfig", menuName = "Research Tree/Configs/Upgrades/Ricochet")]
public class RicochetConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private int baseRicochets = 1;
    [SerializeField] private float baseAngle = 15f;
    [SerializeField] private float additionalRicochetsPerRank = 0.34f;
    [SerializeField] private float angleIncreasePerRank = 5f;
    [SerializeField, TextArea(3, 5)] private string description = "Projectiles ricochet on hit.";

    public int BaseRicochets => baseRicochets;
    public float BaseAngle => baseAngle;
    public float AdditionalRicochetsPerRank => additionalRicochetsPerRank;
    public float AngleIncreasePerRank => angleIncreasePerRank;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new RicochetEffectUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(RicochetEffect)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<RicochetEffect>(treeNodeConfig, typeof(ProjectileTowerNodeConfig));
    }
}