using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpeedUpgrade", menuName = "Research Tree/Upgrades/ProjectileSpeed Upgrade")]
public class NodePTProjectileSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseSpeedMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Increases projectile velocity, allowing faster target engagement.\n" +
        "Faster projectiles are harder for enemies to dodge and reach targets quicker.\n" +
        "Essential for hitting fast-moving enemies at long range.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"Cost: {Cost + Cost * Mathf.Pow(rank, 0.5f):F0}\n" +
               $"{description}\n\n" +
               $"Current Effect (Rank {rank}):\n" +
               $"• Speed Multiplier: {baseSpeedMultiplier + (rank * rankBonusPerLevel):F2}x\n\n" +
               $"Rank Bonus:\n" +
               $"• +{rankBonusPerLevel:F2}x multiplier per rank";
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        GameState.Instance.SpendCurrency((int)(Cost * Mathf.Pow(rank, 0.5f)));
        
        float totalMultiplier = baseSpeedMultiplier + (rank * rankBonusPerLevel);
        blueprint.ProjectileSpeed *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}