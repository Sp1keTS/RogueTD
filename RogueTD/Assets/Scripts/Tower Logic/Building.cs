using System;
using UnityEngine;
using Ilumisoft.HealthSystem;
public class Building : MonoBehaviour
{
    public string buildingName;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Collider2D buildingCollider;
    static public Action<Vector2Int> onBuildingDestroyed;
    public float CurrentHealthPoints => healthComponent.CurrentHealth;
    public Vector2Int GridPosition {get; set; }
    public Vector2Int Size => size;
    
    void Awake()
    {
        
        healthComponent.CurrentHealth = healthComponent.MaxHealth;
        
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
        healthComponent.CurrentHealth -= damage;
        
        if (healthComponent.CurrentHealth <= 0)
        {
            BuildingFactory.DestroyBuilding(this);
            onBuildingDestroyed.Invoke(GridPosition);
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
        healthComponent.MaxHealth = maxHP;
        healthComponent.CurrentHealth = healthComponent.MaxHealth;
    }

    public void InitializeFromBlueprint(BuildingBlueprint buildingBlueprint)
    {
        healthComponent.MaxHealth = buildingBlueprint.MaxHealthPoints;
        healthComponent.CurrentHealth = healthComponent.MaxHealth;
    }
}