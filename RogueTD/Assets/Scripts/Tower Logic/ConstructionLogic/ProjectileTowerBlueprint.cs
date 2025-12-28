using System;
using UnityEngine;
using System.Text;

public class ProjectileTowerBlueprint : TowerBlueprint
{
    private GameObject projectilePrefab;
    private float projectileSpeed;
    private float spread;
    private float projectileLifetime;
    private bool projectileFragile = true;
    
     protected int projectileCount;
    
    private ResourceReference<ProjectileBehavior>[] projectileBehaviors;
    private ResourceReference<ProjectileEffect>[] projectileEffects;
    private ResourceReference<ProjectileTowerBehavior> shotBehavior;
    private ResourceReference<SecondaryProjectileTowerBehavior>[] secondaryShots;

    private ProjectileTower _projectileTowerPrefab;
    public ProjectileTower ProjectileTowerPrefab => _projectileTowerPrefab;
    
    public GameObject ProjectilePrefab { get => projectilePrefab; set => projectilePrefab = value; }
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float Spread { get => spread; set => spread = value; }
    
    public int ProjectileCount { get => projectileCount; set => projectileCount = value; }
    public float ProjectileLifetime { get => projectileLifetime; set => projectileLifetime = value; }
    public bool ProjectileFragile { get => projectileFragile; set => projectileFragile = value; }
    
    public ResourceReference<ProjectileBehavior>[] ProjectileBehaviors 
    { 
        get => projectileBehaviors; 
        set => projectileBehaviors = value; 
    }
    
    public ResourceReference<ProjectileEffect>[] ProjectileEffects 
    { 
        get => projectileEffects; 
        set => projectileEffects = value; 
    }
    
    public ResourceReference<ProjectileTowerBehavior> ShotBehavior 
    { 
        get => shotBehavior; 
        set => shotBehavior = value; 
    }
    
    public ResourceReference<SecondaryProjectileTowerBehavior>[] SecondaryShots 
    { 
        get => secondaryShots; 
        set => secondaryShots = value; 
    }
    
    public override string GetTowerStats()
    {
        StringBuilder stats = new StringBuilder();
        stats.Append(base.GetTowerStats());
        
        stats.AppendLine("\nProjectile Stats:");
        stats.AppendLine($"▸ Projectile Speed: {projectileSpeed:F1}");
        stats.AppendLine($"▸ Projectile Count: {projectileCount}");
        stats.AppendLine($"▸ Spread: {spread:F1}°");
        stats.AppendLine($"▸ Lifetime: {projectileLifetime:F1}sec");
        stats.AppendLine($"▸ Fragile: {(projectileFragile ? "Yes" : "No")}");
        stats.AppendLine($"▸ Projectile Effects: {projectileEffects?.Length ?? 0}");
        stats.AppendLine($"▸ Projectile Behaviors: {projectileBehaviors?.Length ?? 0}");
        stats.AppendLine($"▸ Secondary Shots: {secondaryShots?.Length ?? 0}");
        
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