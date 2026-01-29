using System.Collections.Generic;
using UnityEngine;
using System.Text;

[CreateAssetMenu(fileName = "TowerBlueprint", menuName = "Tower Defense/TowerBlueprint")]
public class TowerBlueprint : BuildingBlueprint
{
    private Tower towerPrefab;
    protected float rotatingSpeed = 10f;
    protected float targetingRange = 5f;
    protected float damageMult = 1f;
    protected float attackSpeed = 1f;
    protected int damage = 1;
    protected int maxAmmo;
    protected float currentAmmo;
    protected float ammoRegeneration;
    protected float projectileScale;
    protected List<StatusEffect> statusEffects; 
    protected TowerBehaviour[] towerBehaviours;
    protected float attackAngle = 10;
    public float AttackAngle { get => attackAngle; set => attackAngle = value; }
    
    public Tower TowerPrefab { get => towerPrefab; set => towerPrefab = value; }
    public float RotatingSpeed { get => rotatingSpeed; set => rotatingSpeed = value; }
    public float TargetingRange { get => targetingRange; set => targetingRange = value; }
    public float DamageMult { get => damageMult; set => damageMult = value; }  
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Damage { get => damage; set => damage = value; }
    public List<StatusEffect> StatusEffects { get => statusEffects; set => statusEffects = value; } 
    public float ProjectileScale { get => projectileScale; set => projectileScale = value; }
    public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }
    public float CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public float AmmoRegeneration { get => ammoRegeneration; set => ammoRegeneration = value; }
    public TowerBehaviour[] TowerBehaviours { get => towerBehaviours; set => towerBehaviours = value; }
    
    public virtual string GetTowerStats()
    {
        StringBuilder stats = new StringBuilder();
        
        stats.AppendLine($"Damage: {damage} (x{damageMult:F1})");
        stats.AppendLine($"Attack Speed: {attackSpeed:F1}/sec");
        stats.AppendLine($"Range: {targetingRange:F1}");
        stats.AppendLine($"Rotation Speed: {rotatingSpeed:F0}°/sec");
        stats.AppendLine($"Ammo: {currentAmmo:F0}/{maxAmmo}");
        stats.AppendLine($"Regeneration: {ammoRegeneration:F1}/sec");
        stats.AppendLine($"Status Effects: {statusEffects?.Count ?? 0}");
        
        return stats.ToString();
    }
    public void Initialize(string buildingName, Tower tower, Building buildingPrefab, int maxHealthPoints, int cost, Vector2 size)
    {
        _buildingName = buildingName;
        towerPrefab = tower;
        _buildingPrefab = buildingPrefab;
        _maxHealthPoints = maxHealthPoints;
        _cost = cost;
        _size = size;
    }
}