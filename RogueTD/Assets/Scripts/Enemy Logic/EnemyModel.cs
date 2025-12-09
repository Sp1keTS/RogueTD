using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyModel
{
    public string Id { get; set; }
    public int HealthPoints{ get; set; }
    public int MaxHealthPoints{ get; set; }
    public float MoveSpeed{ get; set; }
    public Vector2 Size { get; set; }
    public Texture2D Texture{ get; set; }
    public int Reward { get; set; }
    public int Rank { get; set; }
    public int Cost { get; set; }
    
    private Dictionary<System.Type, StatusEffect> activeEffects = new Dictionary<System.Type, StatusEffect>();
    public EnemyTargetingBehavior targetingBehavior;
    public EnemyMovementBehavior movementBehavior;
    
    [NonSerialized] public Building currentTarget;
    [NonSerialized] public Vector2 currentTargetPosition;
    
    public event Action<EnemyModel> OnDeath;
    public event Action<int, int> OnDamageTaken;
    
    public EnemyModel(EnemyData data, string enemyId)
    {
        Id = enemyId;
        MaxHealthPoints = data.MaxHealth;
        HealthPoints = data.MaxHealth;
        MoveSpeed = data.MoveSpeed;
        Size = data.Size;
        Texture = data.Texture;
        Reward = data.Reward;
        Rank = data.Rank;
        Cost = data.Cost;
        targetingBehavior = data.TargetingBehavior;
        movementBehavior = data.MovementBehavior;
        currentTargetPosition = Vector2.zero;
    }
    
    public void TakeDamage(int damage)
    {
        HealthPoints -= damage;
        OnDamageTaken?.Invoke(HealthPoints, damage);
        
        if (HealthPoints <= 0)
        {
            HealthPoints = 0;
            OnDeath?.Invoke(this);
        }
    }
    
    public void UpdateTarget()
    {
        if (!targetingBehavior)
        {
            FindMainBuildingTarget();
        }
        else
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
    
    private void FindMainBuildingTarget()
    {
        
        foreach (var building in ConstructionGridManager.buildingsPos.Values)
        {
            if (building && building.gameObject.activeInHierarchy && 
                building.CompareTag("MainBuilding"))
            {
                if (currentTarget != building)
                {
                    currentTarget = building;
                    currentTargetPosition = building.transform.position;
                }
                return;
            }
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