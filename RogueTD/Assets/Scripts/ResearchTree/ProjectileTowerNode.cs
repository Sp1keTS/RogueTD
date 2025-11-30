using UnityEngine;

public abstract class ProjectileTowerNode : TreeNode
{
    [SerializeField] protected ProjectileTowerBlueprint towerBlueprint;
    [SerializeField] private BasicShotBehavior basicShotBehavior;
    public ProjectileTowerBlueprint TowerBlueprint => towerBlueprint;

    public override void OnActivate()
    {
        LoadDependencies();
    }

    protected void LoadBasicShot()
    {
        if (basicShotBehavior != null)
        {
            towerBlueprint.ShotBehavior = new ResourceReference<ProjectileTowerBehavior> 
            { 
                Value = basicShotBehavior 
            };
                
            ResourceManager.RegisterTowerBehavior(basicShotBehavior.name, basicShotBehavior);
        }
    }
}