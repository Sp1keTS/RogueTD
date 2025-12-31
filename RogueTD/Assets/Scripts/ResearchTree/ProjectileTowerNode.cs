using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileTowerNode : TowerNode
{
    [SerializeField] private BasicShotBehavior basicShotBehavior;
    private ProjectileTowerBlueprint _projectileTowerBlueprint;
    [SerializeField] private ProjectileTower projectileTower;
    [SerializeField] protected TowerProjectile projectilePrefab;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float spread ;
    [SerializeField] protected float projectileLifetime;
    [SerializeField] protected bool projectileFragile = true;
    [SerializeField] protected float projectileScale;
    [SerializeField] protected int projectileCount;

    
    public ProjectileTower ProjectileTower => projectileTower;
    public ProjectileTowerBlueprint _ProjectileTowerBlueprint
    {
        get
        {
            if (_projectileTowerBlueprint == null)
            {
                _projectileTowerBlueprint = new ProjectileTowerBlueprint();
            }
            return _projectileTowerBlueprint;
        }
        set => _projectileTowerBlueprint = value;
    }


    protected void LoadBasicShot()
    {
        if (basicShotBehavior)
        {
            _ProjectileTowerBlueprint.ShotBehavior = basicShotBehavior;
            ResourceManager.RegisterTowerBehavior(basicShotBehavior.name, basicShotBehavior);
        }
    }
    
    public override void LoadBasicStats(int rank, float rankMultiplier)
    {
        base.LoadBasicStats(rank, rankMultiplier);
        
        _ProjectileTowerBlueprint.ProjectilePrefab = projectilePrefab;
        _ProjectileTowerBlueprint.TowerPrefab = projectileTower;
        _ProjectileTowerBlueprint.BuildingPrefab = buildingPrefab;
        
        _ProjectileTowerBlueprint.ProjectileSpeed = projectileSpeed * rankMultiplier;
        _ProjectileTowerBlueprint.ProjectileLifetime = projectileLifetime * rankMultiplier;
        _ProjectileTowerBlueprint.Spread = spread / rankMultiplier;
        _ProjectileTowerBlueprint.ProjectileFragile = projectileFragile;
        
        _ProjectileTowerBlueprint.ProjectileCount = projectileCount;
        _ProjectileTowerBlueprint.ProjectileScale = projectileScale;
    }
}