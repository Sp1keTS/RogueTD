using System;
using UnityEngine;

public class ProjectileTower : Tower
{
    [Header("Projectile Tower Specific")]
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected float spread = 0f;
    [SerializeField] protected float projectileLifetime = 3f;
    [SerializeField] protected int projectileCount = 1;
    [SerializeField] protected float projectileScale = 1;
    [SerializeField] protected bool projectileFragile = true;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected GameObject barrel;
    
    public SecondaryProjectileTowerBehavior[] secondaryShots = Array.Empty<SecondaryProjectileTowerBehavior>();
    public ProjectileEffect[] effects = Array.Empty<ProjectileEffect>();    
    public ProjectileBehavior[] movements = Array.Empty<ProjectileBehavior>();
    public ProjectileTowerBehavior towerBehavior;
    
    public Action<ShotData> ShootChain;
    public float ProjectileScale {get => projectileScale; }
    public Vector2 BarrelPosition => barrel.transform.position;
    public int ProjectileDamage => (int)(damage * damageMult);
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileLifetime => projectileLifetime;
    public bool ProjectileFragile { get => projectileFragile; set => projectileFragile = value; }
    public int ProjectileCount => projectileCount;
    
    protected TowerProjectile projectile;
    
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
        Debug.Log("А тут?? :" + projectileFragile); //true
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
        projectileFragile = blueprint.ProjectileFragile;
        projectilePrefab = blueprint.ProjectilePrefab;
        projectileScale = blueprint.ProjectileScale;
        effects = ConvertResourceReferencesToValues(blueprint.ProjectileEffects);
        movements = ConvertResourceReferencesToValues(blueprint.ProjectileBehaviors);
        towerBehavior = blueprint.ShotBehavior?.Value;
        secondaryShots = ConvertResourceReferencesToValues(blueprint.SecondaryShots);
        Debug.Log("Хрупкий ли снаряд?? :" + blueprint.ProjectileFragile + " " + projectileFragile); //true true
        BuildShootChain();
    }
    
    
    private void BuildShootChain()
    {
        Action<ShotData> chain = null;
        
        if (towerBehavior != null)
        {
            chain = (shotData) => towerBehavior.Shoot(this, shotData, null);
        }
        
        if (secondaryShots != null && chain != null)
        {
            for (int i = secondaryShots.Length - 1; i >= 0; i--)
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
        if (EnemyInAttackCone())
        {
            var shotData = GetShotData();
            ShootChain?.Invoke(shotData);
        }
    }

    public TowerProjectile CreateProjectile(Vector2 position)
    {
        if (!projectilePrefab) return null;
        
        GameObject newProjectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        projectile = newProjectile.GetComponent<TowerProjectile>();
        
        projectile.Initialize(this);
        Debug.Log(ProjectileFragile);
        return projectile;
    }
    public override string GetTowerStats()
    {
        string baseStats = base.GetTowerStats();
    
        return $"{baseStats}\n" +
               $"Projectile:\n" +
               $"▸ Count: {projectileCount}\n" +
               $"▸ Speed: {projectileSpeed}\n" +
               $"▸ Spread: {spread}°\n" +
               $"▸ Lifetime: {projectileLifetime}sec\n" +
               $"▸ Fragile: {(projectileFragile ? "Yes" : "No")}\n" +
               $"▸ Projectile Effects: {effects?.Length ?? 0}\n" +
               $"▸ Projectile Behaviors: {movements?.Length ?? 0}\n" +
               $"▸ Secondary Shots: {secondaryShots?.Length ?? 0}";
    }
}