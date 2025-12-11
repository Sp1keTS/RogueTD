using UnityEngine;

public abstract class TowerNode : TreeNode
{
    [SerializeField] protected TowerBlueprint towerBlueprint;
    public TowerBlueprint TowerBlueprint => towerBlueprint;

    public override void OnActivate()
    {
        LoadDependencies();
    }
}