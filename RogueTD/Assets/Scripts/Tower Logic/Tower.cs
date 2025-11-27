using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : Building
{
    [SerializeField] protected float targetingRange = 5f;
    [SerializeField] protected float damageMult = 1f;
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected float rotatingSpeed = 10f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float currentAmmo;
    [SerializeField] protected float ammoRegeneration;
    [SerializeField] public StatusEffect[] statusEffects;
    [SerializeField] protected TowerBehaviour[] towerBehaviours;
    
    protected float currentAngle;
    protected Enemy target;
    private List<Enemy> enemies;
    
    public int Damage => damage;
    public float DamageMult => damageMult;
    public float TargetingRange => targetingRange;
    public float AttackSpeed => attackSpeed;
    
    
    public virtual void InitializeFromBlueprint(TowerBlueprint blueprint)
    {
        buildingName = blueprint.buildingName;
        targetingRange = blueprint.TargetingRange;
        damageMult = blueprint.DamageMult;
        attackSpeed = blueprint.AttackSpeed;
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
        if (target != null)
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
        // Базовая логика обновления для всех башен
        
        RestoreAmmo();
        GetTarget();
        RotateTowardsTarget();
    }
    
    // Вспомогательные методы для преобразования ResourceReference[] в T[]
    protected T[] ConvertResourceReferencesToValues<T>(ResourceReference<T>[] references) where T : Resource
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
}