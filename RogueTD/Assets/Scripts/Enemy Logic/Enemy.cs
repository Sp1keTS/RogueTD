using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int healthPoints;
    [SerializeField] int maxHealthPoints;
    [SerializeField] float moveSpeed;
    [SerializeField] Renderer enemyRenderer;
    public Renderer EnemyRenderer { get { return enemyRenderer; } }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    void Start()
    {
        healthPoints = maxHealthPoints;
        EnemyManager.Enemies.Add(this.name, this);
    }

    public void TakeDamage(int damage, StatusEffect[] statusEffects)
    {
        if(statusEffects != null){
            foreach (StatusEffect statusEffect in statusEffects)
            {
                StartCoroutine(statusEffect.ApplyEffect(this));            
            }
        }
        
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
