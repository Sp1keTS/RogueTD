using System;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
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
    
    public ProjectileEffect[] effects = Array.Empty<ProjectileEffect>();    
    public StatusEffect[] statusEffects = Array.Empty<StatusEffect>();
    public ProjectileBehavior[] movements = Array.Empty<ProjectileBehavior>();
    public int ProjectileDamage => (int)(damage * damageMult);
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileLifetime => projectileLifetime;
    public bool ProjectileFragile => projectileFragile;

    public int Damage => damage;
    protected float currentAngle;
    protected Enemy target;
    protected TowerProjectile projectile;
    
    protected void GetTarget()
    {
        var enemies = EnemyManager.Enemies;
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
            if (enemy == null) continue;
            
            float distance = (enemy.transform.position - myPosition).sqrMagnitude;
            float rangeSqr = targetingRange * targetingRange;
            
            if (distance <= rangeSqr && distance < lowestDistance)
            {
                lowestDistance = distance;
                target = enemy;
            }
        }
    }

    protected TowerProjectile CreateProjectile()
    {
        if (projectilePrefab == null) return null;
        
        GameObject newProjectile = Instantiate(projectilePrefab, barrel.transform.position, Quaternion.identity);
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
    
    public struct ShotData
    {
        public int ProjectileCount;
        public float Spread;
        public float ProjectileSpeed;
        public Vector3 Position;
        public Quaternion Rotation;
        public Func<TowerProjectile> CreateProjectileFunc; 
    
        public ShotData(ProjectileTower tower)
        {
            ProjectileCount = tower.projectileCount;
            Spread = tower.spread;
            ProjectileSpeed = tower.projectileSpeed;
            Position = tower.transform.position;
            Rotation = tower.transform.rotation;
            CreateProjectileFunc = tower.CreateProjectile; 
        }
    }
    public ShotData GetShotData()
    {
        return new ShotData(this);
    }
}