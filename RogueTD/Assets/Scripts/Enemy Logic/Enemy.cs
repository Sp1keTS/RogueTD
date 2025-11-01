using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int healthPoints;
    [SerializeField] int maxHealthPoints;

    void Awake()
    {
        healthPoints = maxHealthPoints;
        EnemyManager.Enemies.Add(this);
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
