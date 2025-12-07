using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private EnemyModel enemyModel;
    [SerializeField] private Rigidbody2D rb;
    
    public event System.Action<Enemy> OnDeath;
    
    private Dictionary<System.Type, Coroutine> activeEffectCoroutines = new Dictionary<System.Type, Coroutine>();
    
    public Renderer EnemyRenderer => enemyRenderer;
    public EnemyModel Model => enemyModel;
    public Rigidbody2D Rigidbody => rb;
    public bool IsAlive => enemyModel?.healthPoints > 0;
    public float MoveSpeed 
    { 
        get => enemyModel?.moveSpeed ?? 0f; 
        set 
        { 
            if (enemyModel != null) 
                enemyModel.moveSpeed = value; 
        } 
    }
    
    void Start()
    {
        if (enemyModel == null)
        {
            enemyModel = new EnemyModel(gameObject.name, 100, 1f, Vector2.one);
        }
        
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        
        ApplyModelSettings();
        enemyModel.OnDeath += HandleModelDeath;
        EnemyManager.RegisterEnemy(this);
    }
    
    void FixedUpdate()
    {
        if (enemyModel == null || !IsAlive || !rb) return;
        
        enemyModel.UpdateTarget();
        
        if (enemyModel.currentTarget && enemyModel.movementBehavior)
        {
            enemyModel.movementBehavior.Move(
                enemyModel, 
                rb, 
                transform.position, 
                Time.fixedDeltaTime
            );
        }
        else if (rb)
        {
            enemyModel.movementBehavior?.Stop(rb);
        }
    }
    
    private void ApplyModelSettings()
    {
        transform.localScale = new Vector3(enemyModel.size.x, enemyModel.size.y, 1f);
        
        if (enemyModel.texture != null && enemyRenderer != null)
        {
            enemyRenderer.material.mainTexture = enemyModel.texture;
        }
    }
    
    public void Initialize(EnemyData data)
    {
        enemyModel = new EnemyModel(
            data.name,
            data.MaxHealth,
            data.MoveSpeed,
            data.Size,
            data.Texture
        );
        
        enemyModel.targetingBehavior = data.TargetingBehavior;
        enemyModel.movementBehavior = data.MovementBehavior;
        
        if (isActiveAndEnabled)
        {
            ApplyModelSettings();
        }
    }
    
    public void TakeDamage(int damage, StatusEffect[] statusEffects)
    {
        if (enemyModel == null) return;
        
        if (statusEffects != null)
        {
            foreach (StatusEffect statusEffect in statusEffects)
            {
                ApplyStatusEffect(statusEffect);
            }
        }
        
        enemyModel.TakeDamage(damage);
    }
    
    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        var effectType = statusEffect.GetType();
        
        if (activeEffectCoroutines.TryGetValue(effectType, out Coroutine existingCoroutine))
        {
            statusEffect.OnReapply(this, existingCoroutine);
        }
        else
        {
            var newCoroutine = StartCoroutine(RunStatusEffect(statusEffect));
            activeEffectCoroutines[effectType] = newCoroutine;
            enemyModel.AddStatusEffect(statusEffect);
        }
    }
    
    private IEnumerator RunStatusEffect(StatusEffect statusEffect)
    {
        var effectType = statusEffect.GetType();
        
        try
        {
            yield return statusEffect.ApplyEffect(this);
        }
        finally
        {
            if (activeEffectCoroutines.ContainsKey(effectType))
            {
                activeEffectCoroutines.Remove(effectType);
            }
            enemyModel.RemoveStatusEffect(effectType);
        }
    }
    
    public void ReplaceEffectCoroutine(System.Type effectType, Coroutine newCoroutine)
    {
        if (activeEffectCoroutines.ContainsKey(effectType))
        {
            StopCoroutine(activeEffectCoroutines[effectType]);
            activeEffectCoroutines[effectType] = newCoroutine;
        }
    }
    
    private void HandleModelDeath(EnemyModel model)
    {
        if (rb && enemyModel.movementBehavior)
        {
            enemyModel.movementBehavior.Stop(rb);
        }
        
        OnDeath?.Invoke(this);
        
        if (enemyModel != null)
        {
            enemyModel.OnDeath -= HandleModelDeath;
        }
        
        StopAllEffects();
        EnemyManager.UnregisterEnemy(this);
        Destroy(gameObject);
    }
    
    void OnDestroy()
    {
        if (enemyModel != null)
        {
            enemyModel.OnDeath -= HandleModelDeath;
        }
        
        EnemyManager.UnregisterEnemy(this);
        StopAllEffects();
    }
    
    public void StopAllEffects()
    {
        foreach (var coroutine in activeEffectCoroutines.Values)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        activeEffectCoroutines.Clear();
        enemyModel?.ClearAllStatusEffects();
    }
}