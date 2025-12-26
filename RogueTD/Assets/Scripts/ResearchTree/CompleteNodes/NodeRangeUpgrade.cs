using UnityEngine;

[CreateAssetMenu(fileName = "RangeUpgrade", menuName = "Research Tree/Upgrades/Range Upgrade")]
public class NodeRangeUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseRangeMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Increases tower range.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        float cost = Cost + Cost * Mathf.Pow(rank, 0.5f);
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Range: <color=#00FF00>{baseRangeMultiplier + (rank * rankBonusPerLevel):F2}x</color>\n\n" +
               $"<b>Per Rank:</b> +{rankBonusPerLevel:F2}x";
    }
    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.5f));
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