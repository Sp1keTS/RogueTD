using UnityEngine;

[CreateAssetMenu(fileName = "NodeMachineGunTurret", menuName = "Research Tree/Turrets/Machine Gun Turret Node")]
public class NodeMachineGunTurret : ProjectileTowerNode
{
    [SerializeField] private RampingFireRateBehavior rampingBehavior;
    [SerializeField] private AmmoBasedShootingBehavior ammoBehavior;
    
    public override string TooltipText => "Machine gun with ramping fire rate.";
    
    public override string GetStats(int rank)
    {
        if (_ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Machine Gun (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        _ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
        _ProjectileTowerBlueprint.Initialize(buildingName, ProjectileTower, buildingPrefab, maxHealthPoints, buildingCost, size);
        
        if (_ProjectileTowerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.12f);
            
            _ProjectileTowerBlueprint.Damage = Mathf.RoundToInt(Random.Range(1f, 3f) * rankMultiplier);
            _ProjectileTowerBlueprint.AttackSpeed = Random.Range(8f, 15f) * (1f + (rank * 0.2f));
            _ProjectileTowerBlueprint.TargetingRange = Random.Range(5f, 7f) * (1f + (rank * 0.1f));
            _ProjectileTowerBlueprint.RotatingSpeed = Random.Range(150f, 200f) + (rank * 15f);
            
            _ProjectileTowerBlueprint.ProjectileSpeed = Random.Range(25f, 35f) + (rank * 1.5f);
            _ProjectileTowerBlueprint.ProjectileLifetime = Random.Range(1.2f, 2f) + (rank * 0.1f);
            _ProjectileTowerBlueprint.Spread = Random.Range(2f, 6f);
            _ProjectileTowerBlueprint.ProjectileFragile = true;
            
            _ProjectileTowerBlueprint.ProjectileCount = 1;
            _ProjectileTowerBlueprint.ProjectileScale = 1;
            
            _ProjectileTowerBlueprint.MaxAmmo = Random.Range(50, 80) + (rank * 8);
            _ProjectileTowerBlueprint.CurrentAmmo = _ProjectileTowerBlueprint.MaxAmmo;
            _ProjectileTowerBlueprint.AmmoRegeneration = Random.Range(4f, 6f) + (rank * 0.4f);
            
            _ProjectileTowerBlueprint.DamageMult = Random.Range(0.6f, 0.9f) + (rank * 0.04f);
            
            LoadBasicShot();
            
            if (rampingBehavior && ammoBehavior)
            {
                _ProjectileTowerBlueprint.SecondaryShots = new ResourceReference<SecondaryProjectileTowerBehavior>[]
                {
                    new ResourceReference<SecondaryProjectileTowerBehavior> { Value = rampingBehavior },
                    new ResourceReference<SecondaryProjectileTowerBehavior> { Value = ammoBehavior }
                };
            }
            
            _ProjectileTowerBlueprint.ProjectileBehaviors = null;
            _ProjectileTowerBlueprint.ProjectileEffects = null;
            _ProjectileTowerBlueprint.StatusEffects = null;
            _ProjectileTowerBlueprint.TowerBehaviours = null;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (_ProjectileTowerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(_ProjectileTowerBlueprint);
        }
        if (rampingBehavior)
        {
            ResourceManager.RegisterSecondaryBehavior(rampingBehavior.name, rampingBehavior);
        }
        if (ammoBehavior)
        {
            ResourceManager.RegisterSecondaryBehavior(ammoBehavior.name, ammoBehavior);
        }
    }
}