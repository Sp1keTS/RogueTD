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
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"Cost: {Cost + Cost * Mathf.Pow(rank, 0.5f):F0}\n" +
               $"{description}\n\n" +
               $"Current Effect (Rank {rank}):\n" +
               $"• Range Multiplier: {baseRangeMultiplier + (rank * rankBonusPerLevel):F2}x\n\n" +
               $"Rank Bonus:\n" +
               $"• +{rankBonusPerLevel:F2}x multiplier per rank";
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        GameState.Instance.SpendCurrency((int)(Cost * Mathf.Pow(rank, 0.5f)));
        
        float totalMultiplier = baseRangeMultiplier + (rank * rankBonusPerLevel);
        blueprint.TargetingRange *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}