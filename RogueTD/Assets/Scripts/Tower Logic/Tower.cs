using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected float targetingRange = 5f;
    [SerializeField] protected float damageMult = 1f;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float rotatingSpeed = 10f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float currentAmmo;
    [SerializeField] protected float ammoRegeneration;
    [SerializeField] public  StatusEffect[] statusEffects;
    [SerializeField] protected TowerBehaviour[] towerBehaviours;
    [SerializeField] protected GameObject light;
    [SerializeField] protected float attackAngle = 10;
    
    protected float currentAngle;
    protected Enemy target;
    private List<Enemy> enemies;
    
    public int Damage => damage;
    public float DamageMult => damageMult;
    public float TargetingRange => targetingRange;
    public float AttackSpeed => attackSpeed;
    
    
    public virtual void InitializeFromBlueprint(TowerBlueprint blueprint)
    {
        targetingRange = blueprint.TargetingRange;
        damageMult = blueprint.DamageMult;
        attackSpeed = blueprint.AttackSpeed;
        Debug.Log(attackSpeed);
        rotatingSpeed = blueprint.RotatingSpeed;
        damage = blueprint.Damage;
        maxAmmo = blueprint.MaxAmmo;
        currentAmmo = blueprint.CurrentAmmo;
        ammoRegeneration = blueprint.AmmoRegeneration;
        statusEffects = ConvertResourceReferencesToValues(blueprint.StatusEffects);
        towerBehaviours = blueprint.TowerBehaviours;
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
    
    protected virtual void RotateTowardsTarget()
    {
        if (target)
        {
            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotatingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        }
    }
    protected void RestoreAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo += Time.deltaTime * ammoRegeneration;   
        }
    }
    protected virtual void Update()
    {
        RestoreAmmo();
        GetTarget();
        RotateTowardsTarget();
    }
    
    protected T[] ConvertResourceReferencesToValues<T>(ResourceReference<T>[] references) where T : ScriptableObject
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
    public bool EnemyInAttackCone()
    {
        var position2D = (Vector2)transform.position;
        var colliders = Physics2D.OverlapCircleAll(position2D, targetingRange);
    
        if (colliders.Length == 0)
            return false;
    
        var forwardDirection = (Vector2)transform.right;
        var halfAttackAngle = attackAngle * 0.5f;

        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Enemy"))
                continue;

            var enemyPos = (Vector2)collider.transform.position;
            var direction = enemyPos - position2D;
        
            direction.Normalize();
            var angle = Vector2.Angle(forwardDirection, direction);

            if (angle <= halfAttackAngle)
            {
                return true;
            }
        }

        return false;
    }
    
    
    public virtual string GetTowerStats()
    {
        return $"▸ Damage: {damage} (x{damageMult:F1})\n" +
               $"▸ Attack Speed: {attackSpeed:F1}/sec\n" +
               $"▸ Range: {targetingRange:F1}\n" +
               $"▸ Rotation Speed: {rotatingSpeed}°/sec\n" +
               $"▸ Ammo: {currentAmmo:F0}/{maxAmmo}\n" +
               $"▸ Regeneration: {ammoRegeneration:F1}/sec\n" +
               $"▸ Status Effects: {statusEffects?.Length ?? 0}";
    }
}