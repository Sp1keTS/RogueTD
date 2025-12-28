using UnityEngine;

[CreateAssetMenu(fileName = "GrowingSizeUpgrade", menuName = "Research Tree/Upgrades/Growing Size Upgrade")]
public class GrowingSizeUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private GrowingSizeMovement growingSizeMovement;
    [SerializeField] private float growthRateIncreasePerRank = 0.2f;
    [SerializeField] private float maxSizeIncreasePerRank = 0.5f;
    
    public override string TooltipText => "Projectiles grow over time.";
    
    public override string GetStats(int rank)
    {
        if (growingSizeMovement)
        {
            var baseGrowthRate = 0.5f;
            var baseMaxSize = 3f;
            
            return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank)}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Growth Rate: <color=#00FF00>{baseGrowthRate + (rank * growthRateIncreasePerRank):F1}/s</color>\n" +
                   $"• Max Size: <color=#00FF00>{baseMaxSize + (rank * maxSizeIncreasePerRank):F1}</color>\n\n" +
                   $"<b>Per Rank:</b> +{growthRateIncreasePerRank:F1}/s growth, +{maxSizeIncreasePerRank:F1} max size";
        }
        
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank)}</color></size>\n\n" +
               "<color=#FF5555>Failed to load effect</color>";
    }

    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (!growingSizeMovement)
        {
            Debug.LogError("GrowingSizeMovement is not assigned!");
            return;
        }
        
        growingSizeMovement.GrowthRate = 0.5f + (rank * growthRateIncreasePerRank);
        growingSizeMovement.MaxSize = 3f + (rank * maxSizeIncreasePerRank);
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            growingSizeMovement, 
            b => b.ProjectileBehaviors,
            (b, arr) => b.ProjectileBehaviors = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (growingSizeMovement)
        {
            ResourceManager.RegisterProjectileBehavior(growingSizeMovement.name, growingSizeMovement);
        }
    }
}