using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private int healthPoints;
    [SerializeField] int maxHealthPoints;
    [SerializeField] float moveSpeed;
    [SerializeField] Renderer enemyRenderer;
    
    private Dictionary<System.Type, Coroutine> activeEffects = new Dictionary<System.Type, Coroutine>();
    
    public Renderer EnemyRenderer { get { return enemyRenderer; } }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    
    void Start()
    {
        healthPoints = maxHealthPoints;
        EnemyManager.Enemies.Add(this.name, this);
    }
    
    void OnDestroy()
    {
        if (EnemyManager.Enemies.ContainsKey(this.name))
        {
            EnemyManager.Enemies.Remove(this.name);
        }
        
        StopAllEffects();
    }
    
    public void TakeDamage(int damage, StatusEffect[] statusEffects)
    {
        if (statusEffects != null)
        {
            foreach (StatusEffect statusEffect in statusEffects)
            {
                ApplyStatusEffect(statusEffect);
            }
        }
        
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        System.Type effectType = statusEffect.GetType();
        
        if (activeEffects.TryGetValue(effectType, out Coroutine existingCoroutine))
        {
            statusEffect.OnReapply(this, existingCoroutine);
        }
        else
        {
            Coroutine newCoroutine = StartCoroutine(RunStatusEffect(statusEffect));
            activeEffects[effectType] = newCoroutine;
        }
    }
    
    private IEnumerator RunStatusEffect(StatusEffect statusEffect)
    {
        System.Type effectType = statusEffect.GetType();
        
        try
        {
            yield return statusEffect.ApplyEffect(this);
        }
        finally
        {
            if (activeEffects.ContainsKey(effectType))
            {
                activeEffects.Remove(effectType);
            }
        }
    }
    
    public void ReplaceEffectCoroutine(System.Type effectType, Coroutine newCoroutine)
    {
        if (activeEffects.ContainsKey(effectType))
        {
            StopCoroutine(activeEffects[effectType]);
            activeEffects[effectType] = newCoroutine;
        }
    }
    
    public void StopAllEffects()
    {
        StopAllCoroutines();
        activeEffects.Clear();
    }
}