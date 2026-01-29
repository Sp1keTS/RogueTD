using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UpgradeTreeNode : TreeNode
{
    
    public abstract void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank);
    
    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.5f));
    }
    
}