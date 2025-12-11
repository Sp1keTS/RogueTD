using System.Collections.Generic;
using UnityEngine;

public abstract class TowerUpgradeTreeNode : TreeNode
{
    public List<TowerNode> TowersToUpgrade { get; set; }
    
    public abstract void ApplyUpgrade(TowerBlueprint blueprint, int rank);

    public override void OnActivate()
    {
        if (TowersToUpgrade != null)
        {
            foreach (var towerNode in TowersToUpgrade)
            {
                if (towerNode != null && towerNode.TowerBlueprint != null)
                {
                    ApplyUpgrade(towerNode.TowerBlueprint, CurrentRank);
                }
            }
        }
    }

    public override void Initialize(int rank)
    {
        this.CurrentRank = rank;
        TowersToUpgrade = new List<TowerNode>();
    }
}