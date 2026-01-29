using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileTowerNode<TBlueprint> : TowerNode<TBlueprint> where TBlueprint : ProjectileTowerBlueprint, new()
{
    public ProjectileTowerBlueprint ProjectileTowerBlueprint
    {
        get => (ProjectileTowerBlueprint) _buildingBlueprint;
        set => _buildingBlueprint = value;
    }

    public ProjectileTowerBehavior LoadShotBehavior(ProjectileTowerBehavior behavior )
    {
        ResourceManager.RegisterTowerBehavior(behavior.name, behavior);
        return ResourceManager.GetResource<ProjectileTowerBehavior>(behavior.name);
    }
    
    public ProjectileTowerNode(float rankMultiplier, ProjectileTowerNodeConfig TowerConfig) : base(rankMultiplier, TowerConfig)
    {
        
        ProjectileTowerBlueprint.ProjectilePrefab = TowerConfig.ProjectilePrefab;
        ProjectileTowerBlueprint.TowerPrefab = TowerConfig.Tower;
        
        ProjectileTowerBlueprint.ProjectileSpeed = TowerConfig.ProjectileSpeed * rankMultiplier;
        ProjectileTowerBlueprint.ProjectileLifetime = TowerConfig.ProjectileLifetime * rankMultiplier;
        ProjectileTowerBlueprint.Spread = TowerConfig.Spread / rankMultiplier;
        ProjectileTowerBlueprint.PenetrationCount = TowerConfig.PenetrationCount;
        
        ProjectileTowerBlueprint.ProjectileCount = TowerConfig.ProjectileCount;
        ProjectileTowerBlueprint.ProjectileScale = TowerConfig.ProjectileScale;
    }
}