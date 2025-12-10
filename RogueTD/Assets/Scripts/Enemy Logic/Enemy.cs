using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private EnemyModel enemyModel; 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameState gameState;
    
    private string id;
    private int currentHealth;
    private Dictionary<System.Type, Coroutine> activeEffectCoroutines = new Dictionary<System.Type, Coroutine>();
    
    public event System.Action<Enemy> OnDeath;
    
    public string Id => id;
    public int CurrentHealth => currentHealth;
    public int MaxHealth {get => enemyModel.MaxHealth;
        set => enemyModel.MaxHealth = value;
    }
    public float MoveSpeed {get => enemyModel.MoveSpeed;
        set => enemyModel.MoveSpeed = value;
    }
    public Vector2 Size {get => enemyModel.Size;
        set => enemyModel.Size = value;
    }
    public int Reward => enemyModel?.Reward ?? 0;
    public EnemyModel Model => enemyModel;
    public Renderer EnemyRenderer => enemyRenderer;
    public Rigidbody2D Rigidbody => rb;
    public bool IsAlive => currentHealth > 0;
    
    private Building currentTarget;
    private Vector2 currentTargetPosition;
    
    public Building GetCurrentTarget() => currentTarget;
    public Vector2 GetCurrentTargetPosition() => currentTargetPosition;
    
    void Start()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        if (enemyModel == null)
        {
            Debug.LogError($"EnemyInstance {name}: EnemyModel is not assigned!");
            Destroy(gameObject);
            return;
        }
        
        id = $"{enemyModel.EnemyName}_{GetInstanceID()}";
        
        currentHealth = enemyModel.MaxHealth;
        
        SetupRigidbody();
        
        ApplyModelSettings();
        
        EnemyManager.RegisterEnemy(this);
    }
    
    void FixedUpdate()
    {
        if (!IsAlive || !rb) return;
        
        UpdateTarget();
        
        if (currentTarget && enemyModel.MovementBehavior)
        {
            enemyModel.MovementBehavior.Move(
                this, 
                rb, 
                transform.position, 
                Time.fixedDeltaTime
            );
        }
        else if (rb && enemyModel.MovementBehavior)
        {
            enemyModel.MovementBehavior.Stop(rb);
        }
    }
    
    private void SetupRigidbody()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }
        }
        
        rb.gravityScale = 0;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.5f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
    private void ApplyModelSettings()
    {
        transform.localScale = new Vector3(enemyModel.Size.x, enemyModel.Size.y, 1f);
        
        if (enemyModel.Texture != null && enemyRenderer != null)
        {
            enemyRenderer.material.mainTexture = enemyModel.Texture;
        }
    }
    
    private void UpdateTarget()
    {
        if (!enemyModel.TargetingBehavior)
        {
            FindMainBuildingTarget();
        }
        else
        {
            Building newTarget = enemyModel.TargetingBehavior.SelectTarget(this);
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
    
    public void TakeDamage(int damage, StatusEffect[] statusEffects)
    {
        if (!IsAlive) return;
        
        if (statusEffects != null)
        {
            foreach (StatusEffect statusEffect in statusEffects)
            {
                ApplyStatusEffect(statusEffect);
            }
        }
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HandleDeath();
        }
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
        }
    }
    
    private IEnumerator RunStatusEffect(StatusEffect statusEffect)
    {
        var effectType = statusEffect.GetType();
        bool effectCompleted = false;
    
        try
        {
            yield return statusEffect.ApplyEffect(this);
            effectCompleted = true; 
        }
        finally
        {
            if (effectCompleted && 
                activeEffectCoroutines.TryGetValue(effectType, out var currentCoroutine) &&
                currentCoroutine == null)
            {
                activeEffectCoroutines.Remove(effectType);
            }
        }
    }
    
    private void HandleDeath()
    {
        if (rb && enemyModel.MovementBehavior)
        {
            enemyModel.MovementBehavior.Stop(rb);
        }
        
        gameState.AddCurrency(enemyModel.Reward);
        
        OnDeath?.Invoke(this);
        
        StopAllEffects();
        
        EnemyManager.UnregisterEnemy(this);
        
        Destroy(gameObject);
    }
    public void ReplaceEffectCoroutine(System.Type effectType, Coroutine newCoroutine)
    {
        if (activeEffectCoroutines.ContainsKey(effectType))
        {
            var oldCoroutine = activeEffectCoroutines[effectType];
            if (oldCoroutine != null)
            {
                StopCoroutine(oldCoroutine);
            }
        }
    
        activeEffectCoroutines[effectType] = newCoroutine;
    }
    void OnDestroy()
    {
        if (IsAlive) 
        {
            EnemyManager.UnregisterEnemy(this);
        }
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
    }
}