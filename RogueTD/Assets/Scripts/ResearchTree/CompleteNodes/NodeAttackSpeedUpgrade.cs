using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Research Tree/Upgrades/Attack Speed Upgrade")]
public class NodeAttackSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseSpeedMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.07f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Significantly increases your tower's rate of fire.\n" +
        "Improves attack speed with additional bonuses per rank.";
    
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
        blueprint.AttackSpeed *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}