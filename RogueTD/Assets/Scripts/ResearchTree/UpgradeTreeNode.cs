using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileTowerUpgradeTreeNode : TreeNode
{
    public List<ProjectileTowerNode> TowersToUpgrade { get; set; }
    
    public abstract void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank);

    public override void OnActivate()
    {
        this.IsActive = true;
        foreach (ProjectileTowerNode tower in TowersToUpgrade)
        {
           
            if (TowersToUpgrade != null && tower.TowerBlueprint != null)
            {
                ApplyUpgrade(tower.TowerBlueprint, this.CurrentRank);
                
            }
            else
            {
                Debug.LogError($"Cannot apply upgrade {this.name}: No tower to upgrade assigned!");
            }
        }

        if (TowersToUpgrade != null)
        {
            LoadDependencies();
        }
    }

    public override void Initialize(int rank)
    {
        this.CurrentRank = rank;
    }

}