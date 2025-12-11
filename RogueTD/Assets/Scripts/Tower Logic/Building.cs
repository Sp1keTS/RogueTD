using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingName;
    [SerializeField] private int healthPoints;
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Collider2D buildingCollider;
    
    public int CurrentHealthPoints => healthPoints;
    public Vector2Int Size => size;
    
    void Awake()
    {
        healthPoints = maxHealthPoints;
        
        if (!buildingCollider)
        {
            buildingCollider = GetComponent<Collider2D>();
            
            if (!buildingCollider)
            {
                buildingCollider = gameObject.AddComponent<BoxCollider2D>();
                var boxCollider = buildingCollider as BoxCollider2D;
                if (boxCollider)
                {
                    boxCollider.size = new Vector2(size.x, size.y);
                }
            }
        }
        
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        
        if (healthPoints <= 0)
        {
            BuildingFactory.DestroyBuilding(this);
        }
    }
    
    public Vector2 GetClosestPoint(Vector2 fromPosition)
    {
        if (buildingCollider && buildingCollider.enabled)
        {
            return buildingCollider.ClosestPoint(fromPosition);
        }

        return Vector2.zero;
    }
    
    
    
    public void Initialize(int maxHP)
    {
        maxHealthPoints = maxHP;
        healthPoints = maxHealthPoints;
    }

    public void InitializeFromBlueprint(BuildingBlueprint buildingBlueprint)
    {
        maxHealthPoints = buildingBlueprint.MaxHealthPoints;
        healthPoints = maxHealthPoints;
    }
}