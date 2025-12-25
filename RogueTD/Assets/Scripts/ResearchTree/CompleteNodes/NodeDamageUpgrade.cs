using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Research Tree/Upgrades/Damage Upgrade")]
public class NodeDamageUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseDamageMultiplier = 1.3f;
    [SerializeField] private float rankBonusPerLevel = 0.1f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Massively increases your tower's damage output.\n" +
        "Boosts both base damage and damage multiplier for devastating attacks.\n" +
        "Essential for dealing with armored or high-health enemies.";
    
    public override string TooltipText
    {
        get
        {
            return $"DAMAGE UPGRADE\n\n" +
                   $"{description}\n\n" +
                   $"Upgrade Effects (Rank {CurrentRank}):\n" +
                   $"• Base Damage Multiplier: {baseDamageMultiplier:F1}x\n" +
                   $"• Per-Rank Bonus: +{rankBonusPerLevel:F2}x\n" +
                   $"• Total Multiplier at Rank {CurrentRank}: {baseDamageMultiplier + (CurrentRank * rankBonusPerLevel):F2}x\n" +
                   $"• Increases both base damage and damage multiplier";
        }
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseDamageMultiplier + (rank * rankBonusPerLevel);
        blueprint.Damage = Mathf.RoundToInt(blueprint.Damage * totalMultiplier);
        blueprint.DamageMult *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
        Debug.Log($"Damage upgraded to {totalMultiplier:F1}x! New damage: {blueprint.Damage}");
    }

    public override void LoadDependencies()
    {
    }
}