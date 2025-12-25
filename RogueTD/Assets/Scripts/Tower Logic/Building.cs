using System;
using UnityEngine;
using Ilumisoft.HealthSystem;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string buildingName;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Collider2D buildingCollider;
    [SerializeField] private GameObject healthBar;
    static public Action<Vector2Int> onBuildingDestroyed;
    public float CurrentHealthPoints
    {
        get => healthComponent.CurrentHealth; set => healthComponent.CurrentHealth = value;
    }
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
    
    public void Initialize(int maxHP, float currentHP = -1)
    {
        healthComponent.MaxHealth = maxHP;
        CurrentHealthPoints = (currentHP >= 0) ? currentHP : maxHP;
    }

    public void InitializeFromBlueprint(BuildingBlueprint buildingBlueprint, float currentHP = -1)
    {
        Initialize(buildingBlueprint.MaxHealthPoints, currentHP);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        healthBar.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        healthBar.SetActive(false);
    }
}