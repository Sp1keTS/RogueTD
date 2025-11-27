using UnityEngine;

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
    [SerializeField] protected int projectileCount;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float currentAmmo;
    [SerializeField] protected float ammoRegeneration;
    [SerializeField] protected ResourceReference<StatusEffect>[] statusEffects; 
    [SerializeField] protected TowerBehaviour[] towerBehaviours;
    
    public GameObject TowerPrefab { get => towerPrefab; set => towerPrefab = value; }
    public float RotatingSpeed { get => rotatingSpeed; set => rotatingSpeed = value; }
    public float TargetingRange { get => targetingRange; set => targetingRange = value; }
    public float DamageMult { get => damageMult; set => damageMult = value; }  
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Damage { get => damage; set => damage = value; }
    public ResourceReference<StatusEffect>[] StatusEffects { get => statusEffects; set => statusEffects = value; } // Обновлено
    public int ProjectileCount { get => projectileCount; set => projectileCount = value; }
    public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }
    public float CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public float AmmoRegeneration { get => ammoRegeneration; set => ammoRegeneration = value; }
    public TowerBehaviour[] TowerBehaviours { get => towerBehaviours; set => towerBehaviours = value; }
}