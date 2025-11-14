using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingName;
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
    
    public void Initialize(int maxHP)
    {
        maxHealthPoints = maxHP;
        healthPoints = maxHealthPoints;
    }

    public void InitializeFromBlueprint(BuildingBlueprint buildingBlueprint)
    {
        maxHealthPoints = buildingBlueprint.MaxHealthPoints;
        
    }
}
