using UnityEngine;

public abstract class TowerNode : BuildingNode
{
    [SerializeField] private Tower tower;
    [SerializeField] protected float targetingRange;
    [SerializeField] protected float damageMult;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float rotatingSpeed;
    [SerializeField] protected int damage ;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float currentAmmo;
    [SerializeField] protected float ammoRegeneration;
    [SerializeField] protected StatusEffect[] statusEffects;
    [SerializeField] protected TowerBehaviour[] towerBehaviours;
    [SerializeField] protected float attackAngle;
    
    private TowerBlueprint _towerBlueprint;
    
    public TowerBlueprint TowerBlueprint => _towerBlueprint;

    public void CreateBlueprint()
    {
        _towerBlueprint = new TowerBlueprint();
        _towerBlueprint.Initialize(buildingName, buildingPrefab, maxHealthPoints, size );
    }
    public override void LoadBasicStats(int rank, float rankMultiplier)
    {
        base.LoadBasicStats(rank, rankMultiplier);
        
        _towerBlueprint.TowerPrefab = tower;
        _towerBlueprint.TargetingRange = targetingRange * rankMultiplier;
        _towerBlueprint.DamageMult = damageMult;
        _towerBlueprint.AttackSpeed = attackSpeed * rankMultiplier;
        _towerBlueprint.RotatingSpeed = rotatingSpeed * rankMultiplier;
        _towerBlueprint.Damage = (int)(damage * rankMultiplier);
        _towerBlueprint.MaxAmmo = (int)(maxAmmo * rankMultiplier);
        _towerBlueprint.CurrentAmmo = _towerBlueprint.MaxAmmo;
        _towerBlueprint.AmmoRegeneration = ammoRegeneration;
        _towerBlueprint.TowerBehaviours = towerBehaviours;
        _towerBlueprint.AttackAngle = attackAngle;
    }
}