using UnityEngine;

public abstract class ProjectileTowerUpgradeTreeNode : TreeNode
{
    public ProjectileTowerNode TowerToUpgrade { get; set; }
    
    public abstract void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank);

    public override void OnActivate()
    {
        this.IsActive = true;
        
        if (TowerToUpgrade != null && TowerToUpgrade.TowerBlueprint != null)
        {
            ApplyUpgrade(TowerToUpgrade.TowerBlueprint, this.CurrentRank);
            LoadDependencies();
            Debug.Log($"Upgrade {this.name} (Rank {CurrentRank}) applied to {TowerToUpgrade.name}");
        }
        else
        {
            Debug.LogError($"Cannot apply upgrade {this.name}: No tower to upgrade assigned!");
        }
    }

    public override void Initialize(int rank)
    {
        this.CurrentRank = rank;
    }

}