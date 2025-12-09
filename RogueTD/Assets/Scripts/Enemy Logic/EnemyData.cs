using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Tower Defense/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Stats")]
    [SerializeField] private string enemyName = "Basic Enemy";
    [SerializeField] private int rank = 1;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector2 size = Vector2.one;
    [SerializeField] private Texture2D texture;
    [SerializeField] private int cost = 10; 
    
    [Header("Behavior")]
    [SerializeField] private EnemyTargetingBehavior targetingBehavior;
    [SerializeField] private EnemyMovementBehavior movementBehavior;
    
    [SerializeField] private int reward = 5;
    
    public string EnemyName => enemyName;
    public int Rank => rank;
    public int MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public Vector2 Size => size;
    public Texture2D Texture => texture;
    public int Cost => cost;
    public EnemyTargetingBehavior TargetingBehavior => targetingBehavior;
    public EnemyMovementBehavior MovementBehavior => movementBehavior;
    public int Reward => reward;
}