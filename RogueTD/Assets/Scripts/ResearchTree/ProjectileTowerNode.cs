using System;
using UnityEngine;

public abstract class ProjectileTowerNode : TowerNode
{
    [SerializeField] private BasicShotBehavior basicShotBehavior;
    private ProjectileTowerBlueprint projectileTowerBlueprint;
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
    public ProjectileTowerBlueprint ProjectileTowerBlueprint {get => projectileTowerBlueprint; set => projectileTowerBlueprint = value; }
    
    public ProjectileTowerBlueprint TowerBlueprint => projectileTowerBlueprint;

    public override void OnActivate()
    {
        LoadDependencies();
    }

    protected void LoadBasicShot()
    {
        if (basicShotBehavior)
        {
            projectileTowerBlueprint.ShotBehavior = new ResourceReference<ProjectileTowerBehavior> 
            { 
                Value = basicShotBehavior 
            };
                
            ResourceManager.RegisterTowerBehavior(basicShotBehavior.name, basicShotBehavior);
        }
    }

    public void LoadBasicStats(int rank, float rankMultiplier)
    {
        ProjectileTowerBlueprint.BuildingName = buildingName;
        ProjectileTowerBlueprint.Size = size;
        ProjectileTowerBlueprint.ProjectilePrefab = projectilePrefab;
        ProjectileTowerBlueprint.BuildingPrefab = buildingPrefab;
        ProjectileTowerBlueprint.TowerPrefab = projectileTower;
        ProjectileTowerBlueprint.MaxHealthPoints = maxHealthPoints;
        ProjectileTowerBlueprint.Cost = buildingCost;
        
        
        ProjectileTowerBlueprint.ProjectilePrefab = projectilePrefab;
        ProjectileTowerBlueprint.Damage = (int)(damage * rankMultiplier);
        ProjectileTowerBlueprint.AttackSpeed = attackSpeed * rankMultiplier;
        ProjectileTowerBlueprint.TargetingRange = targetingRange * rankMultiplier;
        ProjectileTowerBlueprint.RotatingSpeed = rotatingSpeed * rankMultiplier;
            
        ProjectileTowerBlueprint.ProjectileSpeed = projectileSpeed * rankMultiplier;
        ProjectileTowerBlueprint.ProjectileLifetime = projectileLifetime * rankMultiplier;
        ProjectileTowerBlueprint.Spread = spread / rankMultiplier;
        ProjectileTowerBlueprint.ProjectileFragile = projectileFragile;
            
        ProjectileTowerBlueprint.ProjectileCount = projectileCount;
        ProjectileTowerBlueprint.ProjectileScale = projectileScale;
            
        ProjectileTowerBlueprint.MaxAmmo = (int)(maxAmmo * rankMultiplier);
        ProjectileTowerBlueprint.CurrentAmmo = ProjectileTowerBlueprint.MaxAmmo;
        ProjectileTowerBlueprint.AmmoRegeneration = ammoRegeneration; 
            
        ProjectileTowerBlueprint.DamageMult =  damageMult;
        ProjectileTowerBlueprint.ProjectileBehaviors = projectileBehaviors;
        ProjectileTowerBlueprint.ProjectileEffects = projectileEffects;
        ProjectileTowerBlueprint.SecondaryShots = secondaryShots;
        ProjectileTowerBlueprint.TowerBehaviours = towerBehaviours;
    }
}