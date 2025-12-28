using UnityEngine;

[CreateAssetMenu(fileName = "NodeMachineGunTurret", menuName = "Research Tree/Turrets/Machine Gun Turret Node")]
public class NodeMachineGunTurret : ProjectileTowerNode
{
    [SerializeField] private RampingFireRateBehavior rampingBehavior;
    [SerializeField] private AmmoBasedShootingBehavior ammoBehavior;
    
    public override string TooltipText => "Machine gun with ramping fire rate.";
    
    public override string GetStats(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Machine Gun (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
        ProjectileTowerBlueprint.Initialize(buildingName, ProjectileTower, buildingPrefab, maxHealthPoints, buildingCost, size);
        
        if (ProjectileTowerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.12f);
            
            ProjectileTowerBlueprint.Damage = Mathf.RoundToInt(Random.Range(1f, 3f) * rankMultiplier);
            ProjectileTowerBlueprint.AttackSpeed = Random.Range(8f, 15f) * (1f + (rank * 0.2f));
            ProjectileTowerBlueprint.TargetingRange = Random.Range(5f, 7f) * (1f + (rank * 0.1f));
            ProjectileTowerBlueprint.RotatingSpeed = Random.Range(150f, 200f) + (rank * 15f);
            
            ProjectileTowerBlueprint.ProjectileSpeed = Random.Range(25f, 35f) + (rank * 1.5f);
            ProjectileTowerBlueprint.ProjectileLifetime = Random.Range(1.2f, 2f) + (rank * 0.1f);
            ProjectileTowerBlueprint.Spread = Random.Range(2f, 6f);
            ProjectileTowerBlueprint.ProjectileFragile = true;
            
            ProjectileTowerBlueprint.ProjectileCount = 1;
            ProjectileTowerBlueprint.ProjectileScale = 1;
            
            ProjectileTowerBlueprint.MaxAmmo = Random.Range(50, 80) + (rank * 8);
            ProjectileTowerBlueprint.CurrentAmmo = ProjectileTowerBlueprint.MaxAmmo;
            ProjectileTowerBlueprint.AmmoRegeneration = Random.Range(4f, 6f) + (rank * 0.4f);
            
            ProjectileTowerBlueprint.DamageMult = Random.Range(0.6f, 0.9f) + (rank * 0.04f);
            
            LoadBasicShot();
            
            if (rampingBehavior && ammoBehavior)
            {
                ProjectileTowerBlueprint.SecondaryShots = new ResourceReference<SecondaryProjectileTowerBehavior>[]
                {
                    new ResourceReference<SecondaryProjectileTowerBehavior> { Value = rampingBehavior },
                    new ResourceReference<SecondaryProjectileTowerBehavior> { Value = ammoBehavior }
                };
            }
            
            ProjectileTowerBlueprint.ProjectileBehaviors = null;
            ProjectileTowerBlueprint.ProjectileEffects = null;
            ProjectileTowerBlueprint.StatusEffects = null;
            ProjectileTowerBlueprint.TowerBehaviours = null;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (ProjectileTowerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
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