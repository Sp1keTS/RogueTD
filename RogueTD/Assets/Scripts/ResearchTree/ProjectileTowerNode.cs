using UnityEngine;

public abstract class ProjectileTowerNode : TreeNode
{
    [SerializeField] protected ProjectileTowerBlueprint towerBlueprint;
    
    public ProjectileTowerBlueprint TowerBlueprint => towerBlueprint;

    public override void OnActivate()
    {
        this.IsActive = true;
        LoadDependencies();
    }

    public override void LoadDependencies()
    {
        if (towerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(towerBlueprint);
        }
    }
}