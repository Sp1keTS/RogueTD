using System;
using UnityEngine;
using System.Text;

[Serializable]
[CreateAssetMenu(fileName = "ProjectileTowerBlueprint", menuName = "Tower Defense/ProjectileTowerBlueprint")]
public class ProjectileTowerBlueprint : TowerBlueprint
{
    [Header("Projectile Tower Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float spread = 0f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private bool projectileFragile = true;
    
    [SerializeField] protected int projectileCount;
    
    [Header("Projectile Behaviors")]
    [SerializeField] private ResourceReference<ProjectileBehavior>[] projectileBehaviors;
    [SerializeField] private ResourceReference<ProjectileEffect>[] projectileEffects;
    [SerializeField] private ResourceReference<ProjectileTowerBehavior> shotBehavior;
    [SerializeField] private ResourceReference<SecondaryProjectileTowerBehavior>[] secondaryShots;
    
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
}