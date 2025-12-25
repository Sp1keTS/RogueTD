using UnityEngine;
using System.Text;

[CreateAssetMenu(fileName = "TowerBlueprint", menuName = "Tower Defense/TowerBlueprint")]
public class TowerBlueprint : BuildingBlueprint
{
    [Header("Tower Settings")]
    [SerializeField] protected GameObject towerPrefab;
    [SerializeField] protected float rotatingSpeed = 10f;
    [SerializeField] protected float targetingRange = 5f;
    [SerializeField] protected float damageMult = 1f;
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float currentAmmo;
    [SerializeField] protected float ammoRegeneration;
    [SerializeField] protected float projectileScale;
    [SerializeField] protected ResourceReference<StatusEffect>[] statusEffects; 
    [SerializeField] protected TowerBehaviour[] towerBehaviours;
    
    public GameObject TowerPrefab { get => towerPrefab; set => towerPrefab = value; }
    public float RotatingSpeed { get => rotatingSpeed; set => rotatingSpeed = value; }
    public float TargetingRange { get => targetingRange; set => targetingRange = value; }
    public float DamageMult { get => damageMult; set => damageMult = value; }  
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Damage { get => damage; set => damage = value; }
    public ResourceReference<StatusEffect>[] StatusEffects { get => statusEffects; set => statusEffects = value; } 
    public float ProjectileScale { get => projectileScale; set => projectileScale = value; }
    public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }
    public float CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public float AmmoRegeneration { get => ammoRegeneration; set => ammoRegeneration = value; }
    public TowerBehaviour[] TowerBehaviours { get => towerBehaviours; set => towerBehaviours = value; }
    
    public virtual string GetTowerStats()
    {
        StringBuilder stats = new StringBuilder();
        
        stats.AppendLine($"▸ Damage: {damage} (x{damageMult:F1})");
        stats.AppendLine($"▸ Attack Speed: {attackSpeed:F1}/sec");
        stats.AppendLine($"▸ Range: {targetingRange:F1}");
        stats.AppendLine($"▸ Rotation Speed: {rotatingSpeed:F0}°/sec");
        stats.AppendLine($"▸ Ammo: {currentAmmo:F0}/{maxAmmo}");
        stats.AppendLine($"▸ Regeneration: {ammoRegeneration:F1}/sec");
        stats.AppendLine($"▸ Status Effects: {statusEffects?.Length ?? 0}");
        
        return stats.ToString();
    }
}