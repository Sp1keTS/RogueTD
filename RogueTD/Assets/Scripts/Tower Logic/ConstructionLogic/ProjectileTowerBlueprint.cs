using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ProjectileTowerBlueprint : TowerBlueprint
{
    private TowerProjectile projectilePrefab;
    private float projectileSpeed;
    private float spread;
    private float projectileLifetime;
    private int _penetrationCount = 0;
    
    protected int projectileCount;
    
    private List<ProjectileBehavior> projectileBehaviors;
    private List<ProjectileEffect> projectileEffects;
    private ProjectileTowerBehavior shotBehavior;
    private List<SecondaryProjectileTowerBehavior> secondaryShots;

    private ProjectileTower _projectileTowerPrefab;
    public ProjectileTower ProjectileTowerPrefab => _projectileTowerPrefab;
    
    public TowerProjectile ProjectilePrefab { get => projectilePrefab; set => projectilePrefab = value; }
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float Spread { get => spread; set => spread = value; }
    
    public int ProjectileCount { get => projectileCount; set => projectileCount = value; }
    public float ProjectileLifetime { get => projectileLifetime; set => projectileLifetime = value; }
    public int PenetrationCount { get => _penetrationCount; set => _penetrationCount = value; }
    
    public List<ProjectileBehavior> ProjectileBehaviors 
    { 
        get => projectileBehaviors; 
        set => projectileBehaviors = value; 
    }
    
    public List<ProjectileEffect> ProjectileEffects 
    { 
        get => projectileEffects; 
        set => projectileEffects = value; 
    }
    
    public ProjectileTowerBehavior ShotBehavior 
    { 
        get => shotBehavior; 
        set => shotBehavior = value; 
    }
    
    public List<SecondaryProjectileTowerBehavior> SecondaryShots 
    { 
        get => secondaryShots; 
        set => secondaryShots = value; 
    }
    
    public override string GetTowerStats()
    {
        StringBuilder stats = new StringBuilder();
        stats.Append(base.GetTowerStats());
        
        stats.AppendLine("\nProjectile Stats:");
        stats.AppendLine($"Projectile Speed: {projectileSpeed:F1}");
        stats.AppendLine($"Projectile Count: {projectileCount}");
        stats.AppendLine($"Spread: {spread:F1}°");
        stats.AppendLine($"Lifetime: {projectileLifetime:F1}sec");
        stats.AppendLine($"PenCount: {_penetrationCount}");
        stats.AppendLine($"Projectile Effects: {projectileEffects?.Count ?? 0}");
        stats.AppendLine($"Projectile Behaviors: {projectileBehaviors?.Count ?? 0}");
        stats.AppendLine($"Secondary Shots: {secondaryShots?.Count ?? 0}");
        
        return stats.ToString();
    }
    public void Initialize(string buildingName, ProjectileTower projectileTower, Building buildingPrefab, int maxHealthPoints, int cost, Vector2 size)
    {
        _buildingName = buildingName;
        _projectileTowerPrefab = projectileTower;
        _buildingPrefab = buildingPrefab;
        _maxHealthPoints = maxHealthPoints;
        _cost = cost;
        _size = size;
    }
}