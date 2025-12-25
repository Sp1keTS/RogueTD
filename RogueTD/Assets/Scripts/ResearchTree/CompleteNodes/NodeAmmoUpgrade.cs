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
        "Increases maximum ammo by {baseAmmoMultiplier:F1}x and improves ammo regeneration by {regenerationBonus:F1} per rank.\n" +
        "Higher ranks provide additional bonuses for sustained fire.";
    
    public override string TooltipText
    {
        get
        {
            return $"AMMO CAPACITY UPGRADE\n\n" +
                   $"{description}\n\n" +
                   $"Upgrade Effects (Rank {CurrentRank}):\n" +
                   $"• Base Ammo Multiplier: {baseAmmoMultiplier:F1}x\n" +
                   $"• Per-Rank Bonus: +{rankBonusPerLevel:F2}x\n" +
                   $"• Regeneration Bonus: +{regenerationBonus * CurrentRank:F1}/sec\n" +
                   $"• Total Multiplier at Rank {CurrentRank}: {baseAmmoMultiplier + (CurrentRank * rankBonusPerLevel):F2}x";
        }
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
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