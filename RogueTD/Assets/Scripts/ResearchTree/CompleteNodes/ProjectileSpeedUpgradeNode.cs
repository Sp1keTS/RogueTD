using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpeedUpgrade", menuName = "Research Tree/Upgrades/ProjectileSpeed Upgrade")]
public class NodePTProjectileSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseRangeMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Increases projectile velocity, allowing faster target engagement.\n" +
        "Faster projectiles are harder for enemies to dodge and reach targets quicker.\n" +
        "Essential for hitting fast-moving enemies at long range.";
    
    public override string TooltipText
    {
        get
        {
            return $"PROJECTILE SPEED UPGRADE\n\n" +
                   $"{description}\n\n" +
                   $"Upgrade Effects (Rank {CurrentRank}):\n" +
                   $"• Base Speed Multiplier: {baseRangeMultiplier:F1}x\n" +
                   $"• Per-Rank Bonus: +{rankBonusPerLevel:F2}x\n" +
                   $"• Total Multiplier at Rank {CurrentRank}: {baseRangeMultiplier + (CurrentRank * rankBonusPerLevel):F2}x\n" +
                   $"• Reduces time-to-impact significantly";
        }
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseRangeMultiplier + (rank * rankBonusPerLevel);
        blueprint.ProjectileSpeed *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}