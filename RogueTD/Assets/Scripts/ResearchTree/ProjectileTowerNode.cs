using System;
using UnityEngine;

public abstract class ProjectileTowerNode : TowerNode
{
    [SerializeField] private BasicShotBehavior basicShotBehavior;
    private ProjectileTowerBlueprint _projectileTowerBlueprint;
    [SerializeField] private ProjectileTower projectileTower;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float spread ;
    [SerializeField] protected float projectileLifetime;
    [SerializeField] protected bool projectileFragile = true;
    [SerializeField] protected float projectileScale;
    [SerializeField] protected int projectileCount;
    
    [Header("Projectile Behaviors")]
    [SerializeField] protected ResourceReference<ProjectileBehavior>[] projectileBehaviors;
    [SerializeField] protected ResourceReference<ProjectileEffect>[] projectileEffects;
    [SerializeField] protected ResourceReference<ProjectileTowerBehavior> shotBehavior;
    [SerializeField] protected ResourceReference<SecondaryProjectileTowerBehavior>[] secondaryShots;

    
    public ProjectileTower ProjectileTower => projectileTower;
    public ProjectileTowerBlueprint _ProjectileTowerBlueprint {get => _projectileTowerBlueprint; set => _projectileTowerBlueprint = value; }
    
    public ProjectileTowerBlueprint TowerBlueprint => _projectileTowerBlueprint;

    protected void LoadBasicShot()
    {
        if (basicShotBehavior)
        {
            _projectileTowerBlueprint.ShotBehavior = new ResourceReference<ProjectileTowerBehavior> 
            { 
                Value = basicShotBehavior 
            };
                
            ResourceManager.RegisterTowerBehavior(basicShotBehavior.name, basicShotBehavior);
        }
    }
    public void CreateBlueprint()
    {
        _ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
        _ProjectileTowerBlueprint.Initialize(buildingName, buildingPrefab, maxHealthPoints, size );
    }
    public override void LoadBasicStats(int rank, float rankMultiplier)
    {
        base.LoadBasicStats(rank, rankMultiplier);
        
        _projectileTowerBlueprint.ProjectilePrefab = projectilePrefab;
        _projectileTowerBlueprint.TowerPrefab = projectileTower;
        _projectileTowerBlueprint.BuildingPrefab = buildingPrefab;
        
        _projectileTowerBlueprint.ProjectileSpeed = projectileSpeed * rankMultiplier;
        _projectileTowerBlueprint.ProjectileLifetime = projectileLifetime * rankMultiplier;
        _projectileTowerBlueprint.Spread = spread / rankMultiplier;
        _projectileTowerBlueprint.ProjectileFragile = projectileFragile;
        
        _projectileTowerBlueprint.ProjectileCount = projectileCount;
        _projectileTowerBlueprint.ProjectileScale = projectileScale;
        
        _projectileTowerBlueprint.ProjectileBehaviors = projectileBehaviors;
        _projectileTowerBlueprint.ProjectileEffects = projectileEffects;
        _projectileTowerBlueprint.SecondaryShots = secondaryShots;
    }
}