using System.Collections.Generic;
using UnityEngine;

public abstract class TowerUpgradeTreeNode : TreeNode
{
    public abstract void ApplyUpgrade(TowerBlueprint blueprint, int rank);

}