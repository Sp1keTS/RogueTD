using UnityEngine;

[CreateAssetMenu(fileName = "NodeMachineGunTurret", menuName = "Research Tree/Turrets/Machine Gun Turret Node")]
public class NodeMachineGunTurret : ProjectileTowerNode
{
    [SerializeField] private RampingFireRateBehavior rampingBehavior;
    [SerializeField] private AmmoBasedShootingBehavior ammoBehavior;
    
    public override string TooltipText => "Machine gun with ramping fire rate.";
    
    public override string GetStats(int rank)
    {
        if (towerBlueprint)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Machine Gun (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        if (towerBlueprint)
        {
            float rankMultiplier = 1f + (rank * 0.12f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(Random.Range(1f, 3f) * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(8f, 15f) * (1f + (rank * 0.2f));
            towerBlueprint.TargetingRange = Random.Range(5f, 7f) * (1f + (rank * 0.1f));
            towerBlueprint.RotatingSpeed = Random.Range(150f, 200f) + (rank * 15f);
            
            towerBlueprint.ProjectileSpeed = Random.Range(25f, 35f) + (rank * 1.5f);
            towerBlueprint.ProjectileLifetime = Random.Range(1.2f, 2f) + (rank * 0.1f);
            towerBlueprint.Spread = Random.Range(2f, 6f);
            towerBlueprint.ProjectileFragile = Random.value > 0.5f;
            
            towerBlueprint.ProjectileCount = 1;
            towerBlueprint.ProjectileScale = 1;
            
            towerBlueprint.MaxAmmo = Random.Range(50, 80) + (rank * 8);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(4f, 6f) + (rank * 0.4f);
            
            towerBlueprint.DamageMult = Random.Range(0.6f, 0.9f) + (rank * 0.04f);
            
            LoadBasicShot();
            
            if (rampingBehavior && ammoBehavior)
            {
                towerBlueprint.SecondaryShots = new ResourceReference<SecondaryProjectileTowerBehavior>[]
                {
                    new ResourceReference<SecondaryProjectileTowerBehavior> { Value = rampingBehavior },
                    new ResourceReference<SecondaryProjectileTowerBehavior> { Value = ammoBehavior }
                };
            }
            
            towerBlueprint.ProjectileBehaviors = null;
            towerBlueprint.ProjectileEffects = null;
            towerBlueprint.StatusEffects = null;
            towerBlueprint.TowerBehaviours = null;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (towerBlueprint)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(towerBlueprint);
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