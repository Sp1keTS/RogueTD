using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ProjectileTowerUpgradeTreeNode : TreeNode
{
    
    public abstract void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank);

    public override void OnActivate()
    {
        
    }
    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.5f));
    }
    public override void Initialize(int rank)
    {
        this.CurrentRank = rank;
        if (!tags.Contains("Unique"))
        {
            tags.Append("Unique");
        }
    }

}