using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ProjectileTowerBlueprint", menuName = "Tower Defense/ProjectileTowerBlueprint")]
public class ProjectileTowerBlueprint : TowerBuildingBlueprint
{
    [Header("Projectile Tower Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float spread = 0f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private bool projectileFragile = true;
    
    [Header("Projectile Behaviors")]
    [SerializeField] private ProjectileBehavior[] projectileBehaviors;
    [SerializeField] private ProjectileEffect[] projectileEffects;
    [SerializeField] private ProjectileTowerBehavior shotBehavior;
    [SerializeField] private SecondaryProjectileTowerBehavior[] secondaryShots;
    public GameObject ProjectilePrefab { get => projectilePrefab; set => projectilePrefab = value; }
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float Spread { get => spread; set => spread = value; }
    public float ProjectileLifetime { get => projectileLifetime; set => projectileLifetime = value; }
    public bool ProjectileFragile { get => projectileFragile; set => projectileFragile = value; }
    public ProjectileBehavior[] ProjectileBehaviors { get => projectileBehaviors; set => projectileBehaviors = value; }
    public ProjectileEffect[] ProjectileEffects { get => projectileEffects; set => projectileEffects = value; }
    public ProjectileTowerBehavior ShotBehavior { get => shotBehavior; set => shotBehavior = value; }
    public SecondaryProjectileTowerBehavior[] SecondaryShots { get => secondaryShots; set => secondaryShots = value; }
}