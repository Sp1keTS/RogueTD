using UnityEngine;

[CreateAssetMenu(fileName = "GrowingSizeUpgrade", menuName = "Research Tree/Upgrades/Growing Size Upgrade")]
public class GrowingSizeUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private float baseGrowthRate = 0.5f;
    [SerializeField] private float baseMaxSize = 3f;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float growthRateIncreasePerRank = 0.2f;
    [SerializeField] private float maxSizeIncreasePerRank = 0.5f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Projectiles grow over time.";

    private float rankedGrowthRate;
    private float rankedMaxSize;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Growth Rate: <color=#00FF00>{rankedGrowthRate:F1}/s</color>\n" +
               $"• Max Size: <color=#00FF00>{rankedMaxSize:F1}</color>\n\n" +
               $"<b>Per Rank:</b> +{growthRateIncreasePerRank:F1}/s growth, +{maxSizeIncreasePerRank:F1} max size";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }

    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        var newGrowingSizeMovement = CreateInstance<GrowingSizeMovement>();
        newGrowingSizeMovement.GrowthRate = rankedGrowthRate;
        newGrowingSizeMovement.MaxSize = rankedMaxSize;
        ResourceManager.RegisterProjectileBehavior(newGrowingSizeMovement.SetRankedName(rank), newGrowingSizeMovement);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        rankedGrowthRate = baseGrowthRate + (rank * growthRateIncreasePerRank);
        rankedMaxSize = baseMaxSize + (rank * maxSizeIncreasePerRank);
    }
}