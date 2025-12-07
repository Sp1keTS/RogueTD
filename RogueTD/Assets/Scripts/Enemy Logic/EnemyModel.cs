using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyModel
{
    public string id;
    public int healthPoints;
    public int maxHealthPoints;
    public float moveSpeed;
    public Vector2 size;
    public Texture2D texture;
    
    public Dictionary<System.Type, StatusEffect> activeEffects = new Dictionary<System.Type, StatusEffect>();
    public EnemyTargetingBehavior targetingBehavior;
    public EnemyMovementBehavior movementBehavior;
    
    [NonSerialized] public Building currentTarget;
    [NonSerialized] public Vector2 currentTargetPosition;
    
    public event Action<EnemyModel> OnDeath;
    public event Action<int, int> OnDamageTaken;
    
    public EnemyModel(string enemyId, int maxHP, float speed, Vector2 enemySize, Texture2D enemyTexture = null)
    {
        id = enemyId;
        maxHealthPoints = maxHP;
        healthPoints = maxHP;
        moveSpeed = speed;
        size = enemySize;
        texture = enemyTexture;
    }
    
    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        OnDamageTaken?.Invoke(healthPoints, damage);
        
        if (healthPoints <= 0)
        {
            healthPoints = 0;
            OnDeath?.Invoke(this);
        }
    }
    
    public void UpdateTarget()
    {
        if (targetingBehavior)
        {
            Building newTarget = targetingBehavior.SelectTarget(this);
            if (newTarget != currentTarget)
            {
                currentTarget = newTarget;
                currentTargetPosition = newTarget ? (Vector2)newTarget.transform.position : Vector2.zero;
            }
        }
        
        if (currentTarget)
        {
            currentTargetPosition = currentTarget.transform.position;
        }
    }
    
    public void AddStatusEffect(StatusEffect effect)
    {
        System.Type effectType = effect.GetType();
        if (!activeEffects.ContainsKey(effectType))
        {
            activeEffects[effectType] = effect;
        }
    }
    
    public void RemoveStatusEffect(System.Type effectType)
    {
        if (activeEffects.ContainsKey(effectType))
        {
            activeEffects.Remove(effectType);
        }
    }
    
    public bool HasStatusEffect<T>() where T : StatusEffect
    {
        return activeEffects.ContainsKey(typeof(T));
    }
    
    public void ClearAllStatusEffects()
    {
        activeEffects.Clear();
    }
}