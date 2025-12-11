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
    
    [Header("Attack Stats")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackSpeed = 1f; // Атак в секунду
    
    [Header("Components")]
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Behavior")]
    [SerializeField] private EnemyTargetingBehavior targetingBehavior;
    [SerializeField] private EnemyMovementBehavior movementBehavior;
    
    private string id;
    private int currentHealth;
    private Dictionary<System.Type, Coroutine> activeEffectCoroutines = new Dictionary<System.Type, Coroutine>();
    private Coroutine attackCoroutine;
    private bool isAttacking = false;
    private float attackCooldown => 1f / attackSpeed;
    
    public event System.Action<Enemy> OnDeath;
    public event System.Action<Building> OnAttackBuilding;
    
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
    
    public float AttackRange 
    { 
        get => attackRange; 
        set => attackRange = value; 
    }
    
    public int AttackDamage 
    { 
        get => attackDamage; 
        set => attackDamage = value; 
    }
    
    public float AttackSpeed 
    { 
        get => attackSpeed; 
        set => attackSpeed = value; 
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
    
    private Building currentTarget;
    private Vector2 currentTargetPosition;
    private float lastAttackTime = 0f;
    
    public Building GetCurrentTarget() => currentTarget;
    public Vector2 GetCurrentTargetPosition() => currentTargetPosition;
    
    void Start()
    {
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
        
        Debug.Log($"{enemyName} spawned with {attackRange} attack range");
    }
    
    public void InitializeImmediate()
    {
        id = $"{enemyName}_{GetInstanceID()}";
        currentHealth = maxHealth;
        
        SetupRigidbody();
        ApplySettings();
    }
    
    void Update()
    {
        if (currentHealth <= 0) return;
        
        CheckForTargetsInRange();
    }
    
    void FixedUpdate()
    {
        if (currentHealth <= 0 || !rb) return;
        
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
        foreach (var building in ConstructionGridManager.BuildingsPos.Values)
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
    
    private void CheckForTargetsInRange()
    {
        bool foundTargetInRange = false;
        Building closestTarget = null;
        float closestDistance = float.MaxValue;
        
        Vector2 enemyPos = transform.position;
        
        foreach (var building in ConstructionGridManager.BuildingsPos.Values)
        {
            if (!building || !building.gameObject.activeInHierarchy) continue;
            
            float distance = GetDistanceToBuilding(enemyPos, building);
            
            if (distance <= attackRange)
            {
                foundTargetInRange = true;
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = building;
                }
                
                #if UNITY_EDITOR
                Debug.DrawLine(enemyPos, building.GetClosestPoint(enemyPos), Color.red);
                #endif
            }
        }
        
        if (foundTargetInRange)
        {
            if (closestTarget && closestTarget != currentTarget)
            {
                currentTarget = closestTarget;
                currentTargetPosition = closestTarget.transform.position;
            }
            
            if (!isAttacking)
            {
                StartAttack();
            }
        }
        else
        {
            if (isAttacking)
            {
                StopAttack();
            }
        }
    }
    
    private float GetDistanceToBuilding(Vector2 enemyPosition, Building building)
    {
        Vector2 closestPoint = building.GetClosestPoint(enemyPosition);
        float distance = Vector2.Distance(enemyPosition, closestPoint);
        
        
        return distance;
    }
    
    private void StartAttack()
    {
        if (isAttacking || attackCoroutine != null) return;
        
        isAttacking = true;
        attackCoroutine = StartCoroutine(AttackRoutine());
        
    }
    
    private void StopAttack()
    {
        if (!isAttacking) return;
        
        isAttacking = false;
        
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
    
    private IEnumerator AttackRoutine()
    {
        while (isAttacking && currentHealth > 0)
        {
            if (currentTarget && currentTarget.gameObject.activeInHierarchy)
            {
                float distance = GetDistanceToBuilding(transform.position, currentTarget);
                
                if (distance <= attackRange)
                {
                    PerformAttack();
                    yield return new WaitForSeconds(attackCooldown);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
        
        StopAttack();
    }
    
    private void PerformAttack()
    {
        if (!currentTarget || !currentTarget.gameObject.activeInHierarchy) return;
        
        currentTarget.TakeDamage(attackDamage);
        
        OnAttackBuilding?.Invoke(currentTarget);
        
        
        lastAttackTime = Time.time;
    }
    
    public void TakeDamage(int damage, StatusEffect[] statusEffects)
    {
        if (currentHealth <= 0) return;
        
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
        StopAttack();
        
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
        StopAttack();
        
        if (currentHealth > 0) 
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