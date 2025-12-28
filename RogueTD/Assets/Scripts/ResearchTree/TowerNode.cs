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
    
    private TowerBlueprint towerBlueprint;
    
    public TowerBlueprint TowerBlueprint => towerBlueprint;

    public void CreateBlueprint()
    {
        towerBlueprint = new TowerBlueprint();
        towerBlueprint.Initialize(buildingName, buildingPrefab, maxHealthPoints, size );
    }
}