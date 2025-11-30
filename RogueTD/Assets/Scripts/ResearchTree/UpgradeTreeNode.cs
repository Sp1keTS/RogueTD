using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileTowerUpgradeTreeNode : TreeNode
{
    public List<ProjectileTowerNode> TowersToUpgrade { get; set; }
    
    public abstract void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank);

    public override void OnActivate()
    {
        
    }

    public override void Initialize(int rank)
    {
        this.CurrentRank = rank;
    }

}