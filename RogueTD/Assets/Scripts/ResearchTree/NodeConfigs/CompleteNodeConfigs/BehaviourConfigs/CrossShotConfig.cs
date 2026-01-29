using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrossShotConfig", menuName = "Research Tree/Configs/Upgrades/Cross Shot")]
public class CrossShotConfig : UpgradeTreeNodeConfig
{
    [SerializeField, TextArea(3, 5)] private string description = "Fires in four directions.";

    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new CrossShotBehaviorUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(CrossShotBehavior)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<CrossShotBehavior>(treeNodeConfig, typeof(ProjectileTowerNodeConfig));
    }
}