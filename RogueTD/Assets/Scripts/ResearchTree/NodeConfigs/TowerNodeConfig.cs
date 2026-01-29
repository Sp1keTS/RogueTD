using UnityEngine;

public abstract class TowerNodeConfig : BuildingNodeConfig
{
    [SerializeField] private GameObject tower;
    [SerializeField] protected float targetingRange; 
    [SerializeField] protected float damageMult;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float rotatingSpeed;
    [SerializeField] protected int damage ;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float currentAmmo;
    [SerializeField] protected float ammoRegeneration;
    [SerializeField] protected float attackAngle;
    
    public virtual Tower Tower => tower.GetComponent<Tower>();
    public float TargetingRange => targetingRange;
    public float DamageMult => damageMult;
    public float AttackSpeed => attackSpeed;
    public float RotatingSpeed => rotatingSpeed;
    public int Damage => damage;
    public int MaxAmmo => maxAmmo;
    public float CurrentAmmo => currentAmmo;
    public float AmmoRegeneration => ammoRegeneration;
    public float AttackAngle => attackAngle;
}
