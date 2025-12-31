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
    [SerializeField] protected float attackAngle;
    
    private TowerBlueprint _towerBlueprint;
    public TowerBlueprint TowerBlueprint
    {
        get
        {
            if (_towerBlueprint == null)
            {
                _towerBlueprint = new TowerBlueprint();  
            }
            return _towerBlueprint;
        }
        set => _towerBlueprint = value;
    }

    public void CreateBlueprint()
    {
        TowerBlueprint = new TowerBlueprint();
        TowerBlueprint.Initialize(buildingName, buildingPrefab, maxHealthPoints, size );
    }
    public override void LoadBasicStats(int rank, float rankMultiplier)
    {
        base.LoadBasicStats(rank, rankMultiplier);
        
        TowerBlueprint.TowerPrefab = tower;
        TowerBlueprint.TargetingRange = targetingRange * rankMultiplier;
        TowerBlueprint.DamageMult = damageMult;
        TowerBlueprint.AttackSpeed = attackSpeed * rankMultiplier;
        TowerBlueprint.RotatingSpeed = rotatingSpeed * rankMultiplier;
        TowerBlueprint.Damage = (int)(damage * rankMultiplier);
        TowerBlueprint.MaxAmmo = (int)(maxAmmo * rankMultiplier);
        TowerBlueprint.CurrentAmmo = _towerBlueprint.MaxAmmo;
        TowerBlueprint.AmmoRegeneration = ammoRegeneration;
        TowerBlueprint.AttackAngle = attackAngle;
    }
}