using UnityEngine;

[CreateAssetMenu(fileName = "NodeSniperTurret", menuName = "Research Tree/Turrets/Sniper Turret Node")]
public class NodeSniperTurret : ProjectileTowerNode
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Long-range precision tower with massive single-shot damage.\n" +
        "Slow rate of fire but devastating impact per shot.\n" +
        "Excellent for picking off high-value targets and armored enemies from a safe distance.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        if (towerBlueprint != null)
        {
            return $"Cost: {Cost}\n" +
                   $"{description}\n\n" +
                   $"Tower Stats (Rank {rank}):\n" +
                   $"{towerBlueprint.GetTowerStats()}\n\n" +
                   $"Specialization:\n" +
                   $"• Extreme single-shot damage (20-30+)\n" +
                   $"• Very long range (8-12+ tiles)\n" +
                   $"• High damage multiplier (1.3-1.8x)\n" +
                   $"• Pinpoint accuracy (0-1° spread)\n" +
                   $"• Durable projectiles\n" +
                   $"• Slow attack speed\n" +
                   $"• Excellent for boss elimination\n" +
                   $"• Projectiles travel at extreme velocity";
        }
        return $"FAILED TO LOAD\n\n{description}";
    }
    
    public override void Initialize(int rank)
    {
        if (towerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.25f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(Random.Range(20f, 30f) * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(0.3f, 0.7f) * (1f + (rank * 0.05f));
            towerBlueprint.TargetingRange = Random.Range(8f, 12f) * (1f + (rank * 0.15f));
            towerBlueprint.RotatingSpeed = Random.Range(60f, 90f) + (rank * 8f);
            towerBlueprint.ProjectileScale = 1;
            towerBlueprint.ProjectileSpeed = Random.Range(50f, 100f) + (rank * 2f);
            towerBlueprint.ProjectileLifetime = Random.Range(2.5f, 4f) + (rank * 0.2f);
            towerBlueprint.Spread = Random.Range(0f, 1f);
            towerBlueprint.ProjectileFragile = false;
            
            towerBlueprint.ProjectileCount = 1;
            
            towerBlueprint.MaxAmmo = Random.Range(8, 15) + (rank * 2);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(0.3f, 0.7f) + (rank * 0.1f);
            
            towerBlueprint.DamageMult = Random.Range(1.3f, 1.8f) + (rank * 0.1f);
            
            LoadBasicShot();
            
            towerBlueprint.ProjectileBehaviors = null;
            towerBlueprint.ProjectileEffects = null;
            towerBlueprint.SecondaryShots = null;
            towerBlueprint.StatusEffects = null;
            towerBlueprint.TowerBehaviours = null;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (towerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(towerBlueprint);
        }
    }
}