using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Research Tree/Upgrades/Attack Speed Upgrade")]
public class NodeAttackSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseSpeedMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.07f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Significantly increases your tower's rate of fire.\n" +
        "Improves attack speed  with additional bonuses per rank.\n";
    
    public override string TooltipText
    {
        get
        {
            return $"ATTACK SPEED UPGRADE\n\n" +
                   $"{description}\n\n" +
                   $"Upgrade Effects (Rank {CurrentRank}):\n" +
                   $"• Base Speed Multiplier: {baseSpeedMultiplier:F1}x\n" +
                   $"• Per-Rank Bonus: +{rankBonusPerLevel:F2}x\n" +
                   $"• Total Multiplier at Rank {CurrentRank}: {baseSpeedMultiplier + (CurrentRank * rankBonusPerLevel):F2}x\n" +
                   $"• Increases DPS significantly";
        }
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseSpeedMultiplier + (rank * rankBonusPerLevel);
        blueprint.AttackSpeed *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
    
    
    
}