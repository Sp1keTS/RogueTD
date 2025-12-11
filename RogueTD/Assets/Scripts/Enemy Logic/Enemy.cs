using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] private string enemyName = "Basic Enemy";
    [SerializeField] private int rank = 1;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector2 size = Vector2.one;
    [SerializeField] private int cost = 10;
    [SerializeField] private int reward = 5;
    
    [Header("Components")]
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Behavior")]
    [SerializeField] private EnemyTargetingBehavior targetingBehavior;
    [SerializeField] private EnemyMovementBehavior movementBehavior;
    
    private string id;
    private int currentHealth;
    private Dictionary<System.Type, Coroutine> activeEffectCoroutines = new Dictionary<System.Type, Coroutine>();
    
    public event System.Action<Enemy> OnDeath;
    
    public string Id => id;
    
    public string EnemyName 
    { 
        get => enemyName; 
        set => enemyName = value; 
    }
    
    public int Rank 
    { 
        get => rank; 
        set => rank = value; 
    }
    
    public int CurrentHealth => currentHealth;
    
    public int MaxHealth 
    { 
        get => maxHealth; 
        set => maxHealth = value; 
    }
    
    public float MoveSpeed 
    { 
        get => moveSpeed; 
        set => moveSpeed = value; 
    }
    
    public Vector2 Size 
    { 
        get => size; 
        set => size = value; 
    }
    
    public int Cost 
    { 
        get => cost; 
        set => cost = value; 
    }
    
    public int Reward 
    { 
        get => reward; 
        set => reward = value; 
    }
    
    public EnemyTargetingBehavior TargetingBehavior 
    { 
        get => targetingBehavior; 
        set => targetingBehavior = value; 
    }
    
    public EnemyMovementBehavior MovementBehavior 
    { 
        get => movementBehavior; 
        set => movementBehavior = value; 
    }
    
    public Renderer EnemyRenderer 
    { 
        get => enemyRenderer; 
        set => enemyRenderer = value; 
    }
    
    public Rigidbody2D Rigidbody 
    { 
        get => rb; 
        set => rb = value; 
    }
    
    public bool IsAlive => currentHealth > 0;
    
    private Building currentTarget;
    private Vector2 currentTargetPosition;
    
    public Building GetCurrentTarget() => currentTarget;
    public Vector2 GetCurrentTargetPosition() => currentTargetPosition;
    
    void Start()
    {
        // Если не был инициализирован через InitializeImmediate
        if (string.IsNullOrEmpty(id))
        {
            Initialize();
        }
    }
    
    private void Initialize()
    {
        id = $"{enemyName}_{GetInstanceID()}";
        currentHealth = maxHealth;
        
        SetupRigidbody();
        ApplySettings();
        
        EnemyManager.RegisterEnemy(this);
    }
    
    // Метод для немедленной инициализации при создании через EnemyManager
    public void InitializeImmediate()
    {
        id = $"{enemyName}_{GetInstanceID()}";
        currentHealth = maxHealth;
        
        SetupRigidbody();
        ApplySettings();
    }
    
    void FixedUpdate()
    {
        if (!IsAlive || !rb) return;
        
        UpdateTarget();
        
        if (currentTarget && movementBehavior)
        {
            movementBehavior.Move(
                this, 
                rb, 
                transform.position, 
                Time.fixedDeltaTime
            );
        }
        else if (rb && movementBehavior)
        {
            movementBehavior.Stop(rb);
        }
    }
    
    private void SetupRigidbody()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
            if (!rb)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }
        }
        
        rb.gravityScale = 0;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.5f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
    private void ApplySettings()
    {
        transform.localScale = new Vector3(size.x, size.y, 1f);
    }
    
    private void UpdateTarget()
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
        if (rb && movementBehavior)
        {
            movementBehavior.Stop(rb);
        }
        
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
    
    public void SetCurrentHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }
}