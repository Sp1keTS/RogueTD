using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : Tower
{
    [Header("Projectile Tower Specific")]
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected float spread = 0f;
    [SerializeField] protected float projectileLifetime = 3f;
    [SerializeField] protected int projectileCount = 1;
    [SerializeField] protected float projectileScale = 1;
    [SerializeField] protected int penetrationCount = 0;
    [SerializeField] protected TowerProjectile projectilePrefab;
    [SerializeField] protected GameObject barrel;
    
    protected bool canShoot;
    protected bool multiply;
    protected TowerProjectile projectile;
    
    public List<SecondaryProjectileTowerBehavior> secondaryShots = new List<SecondaryProjectileTowerBehavior>();
    public List<ProjectileEffect> effects =  new List<ProjectileEffect>();    
    public List<ProjectileBehavior> movements = new List<ProjectileBehavior>();
    public ProjectileTowerBehavior towerBehavior;
    
    public Action<ShotData> ShootChain;
    public float ProjectileScale {get => projectileScale; }
    public Vector2 BarrelPosition => barrel.transform.position;
    public int ProjectileDamage => (int)(damage * damageMult);
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileLifetime => projectileLifetime;
    public int PenetrationCount { get => penetrationCount; set => penetrationCount = value; }
    public bool CanShoot { get => canShoot; set => canShoot = value; }
    public int ProjectileCount => projectileCount;
    public bool Multiply { get => multiply; set => multiply = value; }
    
    
    public struct ShotData
    {
        public int ProjectileCount;
        public float Spread;
        public float ProjectileSpeed;
        public Quaternion Rotation;
        public Func<Vector2, TowerProjectile> CreateProjectileFunc;

        public ShotData(ProjectileTower tower)
        {
            ProjectileCount = tower.projectileCount;
            Spread = tower.spread;
            ProjectileSpeed = tower.projectileSpeed;
            Rotation = tower.transform.rotation;
            CreateProjectileFunc = tower.CreateProjectile;
        }
    }
    
    protected void Start()
    {
        BuildShootChain();
    }
    
    public ShotData GetShotData()
    {
        return new ShotData(this);
    }
    
    public override void InitializeFromBlueprint(TowerBlueprint blueprint)
    {
        base.InitializeFromBlueprint(blueprint);
        if (blueprint is ProjectileTowerBlueprint projectileBlueprint)
        {
            InitializeFromProjectileBlueprint(projectileBlueprint);
        }
    }
    
    public void InitializeFromProjectileBlueprint(ProjectileTowerBlueprint blueprint)
    {
        
        projectileSpeed = blueprint.ProjectileSpeed;
        spread = blueprint.Spread;
        projectileLifetime = blueprint.ProjectileLifetime;
        projectileCount = blueprint.ProjectileCount;
        penetrationCount = blueprint.PenetrationCount;
        projectilePrefab = blueprint.ProjectilePrefab;
        projectileScale = blueprint.ProjectileScale;
        effects = blueprint.ProjectileEffects;
        movements = blueprint.ProjectileBehaviors;
        towerBehavior = blueprint.ShotBehavior;
        secondaryShots = blueprint.SecondaryShots;
        BuildShootChain();
    }
    
    
    private void BuildShootChain()
    {
        Debug.Log("Цепочка создается");
        Action<ShotData> chain = null;
        
        if (towerBehavior != null)
        {
            chain = (shotData) => towerBehavior.Shoot(this, shotData, null);
        }
        else
        {
            Debug.Log("Нет класса поведения");
        }
        if (secondaryShots != null && chain != null)
        {
            for (int i = secondaryShots.Count - 1; i >= 0; i--)
            {
                if (secondaryShots[i] != null)
                {
                    var currentBehavior = secondaryShots[i];
                    var nextInChain = chain; 
                    chain = (shotData) => currentBehavior.Shoot(this, shotData, nextInChain);
                }
            }
        }
        
        ShootChain = chain;
    }
    
    public void ExecuteShootChain()
    {
        if (CanShoot && EnemyInAttackCone())
        {
            var shotData = GetShotData();
            ShootChain?.Invoke(shotData);
        }
    }

    public TowerProjectile CreateProjectile(Vector2 position)
    {
        if (!projectilePrefab)
        {
            return null;
        }
        var newProjectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        projectile = newProjectile;
        projectile.Initialize(this);
        return projectile;
    }
    public override string GetTowerStats()
    {
        string baseStats = base.GetTowerStats();
    
        return $"{baseStats}\n" +
               $"Projectile:\n" +
               $"Count: {projectileCount}\n" +
               $"Speed: {projectileSpeed}\n" +
               $"Spread: {spread}°\n" +
               $"Lifetime: {projectileLifetime}sec\n" +
               $"PenCount: {penetrationCount}\n" +
               $"Projectile Effects: {effects?.Count ?? 0}\n" +
               $"Projectile Behaviors: {movements?.Count ?? 0}\n" +
               $"Secondary Shots: {secondaryShots?.Count ?? 0}";
    }
}