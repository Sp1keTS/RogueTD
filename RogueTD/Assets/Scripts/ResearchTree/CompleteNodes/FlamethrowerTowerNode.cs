using UnityEngine;

[CreateAssetMenu(fileName = "NodeFlamethrowerTurret", menuName = "Research Tree/Turrets/Flamethrower Turret Node")]
public class NodeFlamethrowerTurret : ProjectileTowerNode
{
    [SerializeField] private BurnEffect burnEffect;
    
    public override string TooltipText => "Flamethrower tower.";
    
    public override string GetStats(int rank)
    {
        if (towerBlueprint)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Flamethrower (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        if (towerBlueprint)
        {
            float rankMultiplier = 1f + (rank * 0.15f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(Random.Range(3f, 6f) * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(2f, 4f) * (1f + (rank * 0.1f));
            towerBlueprint.TargetingRange = Random.Range(4f, 6f) * (1f + (rank * 0.1f));
            towerBlueprint.RotatingSpeed = Random.Range(80f, 120f) + (rank * 8f);
            
            towerBlueprint.ProjectileSpeed = Random.Range(8f, 15f) + (rank * 1f);
            towerBlueprint.ProjectileLifetime = Random.Range(1.5f, 2.5f) + (rank * 0.1f);
            towerBlueprint.Spread = Random.Range(5f, 15f);
            towerBlueprint.ProjectileFragile = false; // Всегда прочные
            
            towerBlueprint.ProjectileCount = 1;
            towerBlueprint.ProjectileScale = Random.Range(0.5f, 1f);
            
            towerBlueprint.MaxAmmo = Random.Range(30, 50) + (rank * 5);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(3f, 5f) + (rank * 0.3f);
            
            towerBlueprint.DamageMult = Random.Range(0.8f, 1.2f) + (rank * 0.05f);
            
            LoadBasicShot();
            
            if (burnEffect)
            {
                towerBlueprint.StatusEffects = new ResourceReference<StatusEffect>[]
                {
                    new ResourceReference<StatusEffect> { Value = burnEffect }
                };
            }
            
            towerBlueprint.ProjectileBehaviors = null;
            towerBlueprint.ProjectileEffects = null;
            towerBlueprint.SecondaryShots = null;
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
        if (burnEffect)
        {
            ResourceManager.RegisterStatusEffect(burnEffect.name, burnEffect);
        }
    }
}