using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyModel", menuName = "Tower Defense/Enemy Model")]
public class EnemyModel : ScriptableObject
{
    [Header("Basic Stats")]
    [SerializeField] private string enemyName = "Basic Enemy";
    [SerializeField] private int rank = 1;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector2 size = Vector2.one;
    [SerializeField] private Texture2D texture;
    [SerializeField] private int cost = 10;
    [SerializeField] private int reward = 5;
    
    [Header("Behavior")]
    [SerializeField] private EnemyTargetingBehavior targetingBehavior;
    [SerializeField] private EnemyMovementBehavior movementBehavior;
    
    public string EnemyName {get; set; }
    public int Rank {get; set; }
    public int MaxHealth {get; set; }
    public float MoveSpeed {get; set; }
    public Vector2 Size {get; set; }
    public Texture2D Texture {get; set; }
    public int Cost {get; set; }
    public int Reward {get; set; }
    public EnemyTargetingBehavior TargetingBehavior => targetingBehavior;
    public EnemyMovementBehavior MovementBehavior => movementBehavior;
    
}