using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    public string buildingName;
    [SerializeField] protected float targetingRange = 5f;
    [SerializeField] protected float damageMult = 1f;
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected float spread = 0f;
    [SerializeField] protected float projectileLifetime = 3f;
    [SerializeField] protected float rotatingSpeed = 10f;
    [SerializeField] protected int damage;
    [SerializeField] protected int projectileCount;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float currentAmmo;
    [SerializeField] protected float ammoRegeneration;
    [SerializeField] protected bool projectileFragile = true;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected GameObject barrel;
    [SerializeField] protected ProjectileTowerBehavior towerBehavior;
    
    public SecondaryProjectileTowerBehavior[] secondaryShots = Array.Empty<SecondaryProjectileTowerBehavior>();
    public ProjectileEffect[] effects = Array.Empty<ProjectileEffect>();    
    public StatusEffect[] statusEffects = Array.Empty<StatusEffect>();
    public ProjectileBehavior[] movements = Array.Empty<ProjectileBehavior>();
    public Vector2 BarrelPosition => barrel.transform.position;
    public Action<ShotData> ShootChain;
    
    public int ProjectileDamage => (int)(damage * damageMult);
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileLifetime => projectileLifetime;
    public bool ProjectileFragile => projectileFragile;
    public int Damage => damage;
    
    protected float currentAngle;
    protected Enemy target;
    protected TowerProjectile projectile;
    private List<Enemy> enemies;
    
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
    
    void Start()
    {
        BuildShootChain();
    }
    
    public ShotData GetShotData()
    {
        return new ShotData(this);
    }
    
    public void InitializeFromBlueprint(ProjectileTowerBlueprint blueprint)
    {
        targetingRange = blueprint.TargetingRange;
        damageMult = blueprint.DamageMult;
        attackSpeed = blueprint.AttackSpeed;
        projectileSpeed = blueprint.ProjectileSpeed;
        spread = blueprint.Spread;
        projectileLifetime = blueprint.ProjectileLifetime;
        rotatingSpeed = blueprint.RotatingSpeed;
        damage = blueprint.Damage;
        projectileCount = blueprint.ProjectileCount;
        maxAmmo = blueprint.MaxAmmo;
        currentAmmo = blueprint.CurrentAmmo;
        ammoRegeneration = blueprint.AmmoRegeneration;
        projectileFragile = blueprint.ProjectileFragile;
        projectilePrefab = blueprint.ProjectilePrefab;
        
        // ИСПРАВЛЕННЫЕ СТРОКИ - преобразуем ResourceReference в реальные значения
        effects = ConvertResourceReferencesToValues(blueprint.ProjectileEffects);
        movements = ConvertResourceReferencesToValues(blueprint.ProjectileBehaviors);
        towerBehavior = blueprint.ShotBehavior?.Value; // Получаем значение из ссылки
        secondaryShots = ConvertResourceReferencesToValues(blueprint.SecondaryShots);
        
        BuildShootChain();
    }
    
    // Вспомогательные методы для преобразования ResourceReference[] в T[]
    private T[] ConvertResourceReferencesToValues<T>(ResourceReference<T>[] references) where T : Resource
    {
        if (references == null || references.Length == 0)
            return Array.Empty<T>();
            
        List<T> result = new List<T>();
        foreach (var reference in references)
        {
            if (reference?.Value != null)
            {
                result.Add(reference.Value);
            }
        }
        return result.ToArray();
    }
    
    // Альтернативный вариант - если нужно сохранить ссылки для последующих изменений
    public void InitializeFromBlueprintWithReferences(ProjectileTowerBlueprint blueprint)
    {
        // Базовые настройки
        targetingRange = blueprint.TargetingRange;
        damageMult = blueprint.DamageMult;
        attackSpeed = blueprint.AttackSpeed;
        projectileSpeed = blueprint.ProjectileSpeed;
        spread = blueprint.Spread;
        projectileLifetime = blueprint.ProjectileLifetime;
        rotatingSpeed = blueprint.RotatingSpeed;
        damage = blueprint.Damage;
        projectileCount = blueprint.ProjectileCount;
        maxAmmo = blueprint.MaxAmmo;
        currentAmmo = blueprint.CurrentAmmo;
        ammoRegeneration = blueprint.AmmoRegeneration;
        projectileFragile = blueprint.ProjectileFragile;
        projectilePrefab = blueprint.ProjectilePrefab;
        
        // Сохраняем ссылки для возможности обновления
        UpdateFromBlueprint(blueprint);
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
    
    protected void GetTarget()
    {
        enemies = EnemyManager.Enemies.Values.ToList();
        
        if (enemies == null || enemies.Count == 0)
        {
            target = null;
            return;
        }
        
        float lowestDistance = Mathf.Infinity;
        target = null;
        Vector3 myPosition = transform.position;
        
        foreach (var enemy in enemies)
        {
            if (!enemy) continue;
            
            float distance = (enemy.transform.position - myPosition).sqrMagnitude;
            float rangeSqr = targetingRange * targetingRange;
            
            if (distance <= rangeSqr && distance < lowestDistance)
            {
                lowestDistance = distance;
                target = enemy;
            }
        }
    }

    public TowerProjectile CreateProjectile(Vector2 position)
    {
        if (projectilePrefab == null) return null;
        
        GameObject newProjectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        projectile = newProjectile.GetComponent<TowerProjectile>();
        
        projectile.Initialize(this);
        return projectile;
    }
    
    protected virtual void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotatingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        }
    }
}