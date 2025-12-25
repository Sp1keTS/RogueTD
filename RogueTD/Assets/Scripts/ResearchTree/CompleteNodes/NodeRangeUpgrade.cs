using UnityEngine;

[CreateAssetMenu(fileName = "RangeUpgrade", menuName = "Research Tree/Upgrades/Range Upgrade")]
public class NodeRangeUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseRangeMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Extends your tower's targeting and attack range.\n" +
        "Allows your tower to engage enemies from further away.\n" +
        "Strategic positioning with increased range can cover more of the battlefield.";
    
    public override string TooltipText
    {
        get
        {
            return $"RANGE UPGRADE\n\n" +
                   $"{description}\n\n" +
                   $"Upgrade Effects (Rank {CurrentRank}):\n" +
                   $"• Base Range Multiplier: {baseRangeMultiplier:F1}x\n" +
                   $"• Per-Rank Bonus: +{rankBonusPerLevel:F2}x\n" +
                   $"• Total Multiplier at Rank {CurrentRank}: {baseRangeMultiplier + (CurrentRank * rankBonusPerLevel):F2}x\n" +
                   $"• Increases tower's effective coverage area";
        }
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseRangeMultiplier + (rank * rankBonusPerLevel);
        blueprint.TargetingRange *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
    
}