using System;
using UnityEngine;

public class ProjectileTower : Tower
{
    [Header("Projectile Tower Specific")]
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected float spread = 0f;
    [SerializeField] protected float projectileLifetime = 3f;
    [SerializeField] protected int projectileCount = 1;
    [SerializeField] protected bool projectileFragile = true;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected GameObject barrel;
    
    public SecondaryProjectileTowerBehavior[] secondaryShots = Array.Empty<SecondaryProjectileTowerBehavior>();
    public ProjectileEffect[] effects = Array.Empty<ProjectileEffect>();    
    public ProjectileBehavior[] movements = Array.Empty<ProjectileBehavior>();
    public ProjectileTowerBehavior towerBehavior;
    
    public Action<ShotData> ShootChain;
    
    public Vector2 BarrelPosition => barrel.transform.position;
    public int ProjectileDamage => (int)(damage * damageMult);
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileLifetime => projectileLifetime;
    public bool ProjectileFragile => projectileFragile;
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
        
        // Преобразуем ResourceReference в реальные значения
        effects = ConvertResourceReferencesToValues(blueprint.ProjectileEffects);
        movements = ConvertResourceReferencesToValues(blueprint.ProjectileBehaviors);
        towerBehavior = blueprint.ShotBehavior?.Value;
        secondaryShots = ConvertResourceReferencesToValues(blueprint.SecondaryShots);
        
        BuildShootChain();
    }
    
    // Метод для обновления поведения/эффектов без изменения базовых параметров
    public void UpdateFromBlueprint(ProjectileTowerBlueprint blueprint)
    {
        effects = ConvertResourceReferencesToValues(blueprint.ProjectileEffects);
        movements = ConvertResourceReferencesToValues(blueprint.ProjectileBehaviors);
        towerBehavior = blueprint.ShotBehavior?.Value;
        secondaryShots = ConvertResourceReferencesToValues(blueprint.SecondaryShots);
        
        BuildShootChain();
    }
    
    private void BuildShootChain()
    {
        // Начинаем с основного поведения (конец цепочки)
        Action<ShotData> chain = null;
        
        if (towerBehavior != null)
        {
            chain = (shotData) => towerBehavior.Shoot(this, shotData, null);
        }
        
        // Оборачиваем вторичными поведениями (в обратном порядке)
        if (secondaryShots != null && chain != null)
        {
            for (int i = secondaryShots.Length - 1; i >= 0; i--)
            {
                if (secondaryShots[i] != null)
                {
                    var currentBehavior = secondaryShots[i];
                    var nextInChain = chain; // Сохраняем текущую цепочку
                    chain = (shotData) => currentBehavior.Shoot(this, shotData, nextInChain);
                }
            }
        }
        
        ShootChain = chain;
    }
    
    public void ExecuteShootChain()
    {
        var shotData = GetShotData();
        ShootChain?.Invoke(shotData);
    }

    public TowerProjectile CreateProjectile(Vector2 position)
    {
        if (projectilePrefab == null) return null;
        
        GameObject newProjectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        projectile = newProjectile.GetComponent<TowerProjectile>();
        
        projectile.Initialize(this);
        return projectile;
    }
    
}