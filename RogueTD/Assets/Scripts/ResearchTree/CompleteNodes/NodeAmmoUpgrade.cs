using UnityEngine;

[CreateAssetMenu(fileName = "AmmoUpgrade", menuName = "Research Tree/Upgrades/Ammo Upgrade")]
public class NodeAmmoUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseAmmoMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.08f;
    [SerializeField] private float regenerationBonus = 0.1f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Upgrades your tower's ammunition capacity and regeneration rate.\n" +
        "Increases maximum ammo and improves ammo regeneration.\n" +
        "Higher ranks provide additional bonuses for sustained fire.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"Cost: {Cost + Cost * Mathf.Pow(rank, 0.5f):F0}\n" +
               $"{description}\n\n" +
               $"Current Effect (Rank {rank}):\n" +
               $"• Ammo Multiplier: {baseAmmoMultiplier + (rank * rankBonusPerLevel):F2}x\n" +
               $"• Regeneration Bonus: {1f + (rank * regenerationBonus):F1}x\n\n" +
               $"Rank Bonus:\n" +
               $"• +{rankBonusPerLevel:F2}x ammo multiplier per rank\n" +
               $"• +{regenerationBonus:F1}x regeneration per rank";
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        GameState.Instance.SpendCurrency((int)(Cost * Mathf.Pow(rank, 0.5f)));
        
        float totalMultiplier = baseAmmoMultiplier + (rank * rankBonusPerLevel);
        
        if (blueprint is ProjectileTowerBlueprint projectileBlueprint)
        {
            projectileBlueprint.MaxAmmo = Mathf.RoundToInt(projectileBlueprint.MaxAmmo * totalMultiplier);
            projectileBlueprint.AmmoRegeneration *= (1f + rank * regenerationBonus);
        }
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}