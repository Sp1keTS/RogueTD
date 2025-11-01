using UnityEngine;

public class Building : MonoBehaviour
{
    private int healthPoints;
    [SerializeField] int maxHealthPoints;

    void Awake()
    {
        healthPoints = maxHealthPoints;
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
