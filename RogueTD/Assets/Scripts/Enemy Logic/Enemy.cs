using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int healthPoints;
    [SerializeField] int maxHealthPoints;

    void Start()
    {
        healthPoints = maxHealthPoints;
        EnemyManager.Enemies.Add(this);
    }

    public void TakeDamage(ProjectileTower projectileTower)
    {
        foreach (StatusEffect statusEffect in projectileTower.statusEffects)
        {
            StartCoroutine(statusEffect.ApplyEffect(this));            
        }
        healthPoints -= projectileTower.ProjectileDamage;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
