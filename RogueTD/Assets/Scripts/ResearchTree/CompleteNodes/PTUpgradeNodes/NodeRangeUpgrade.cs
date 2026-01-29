using System.Collections.Generic;
using UnityEngine;

public class NodeRangeUpgrade : ProjectileTowerUpgradeTreeNode
{
    private float baseRangeMultiplier;
    private float rankBonusPerLevel;
    private string description;
    
    private float rankedRangeMultiplier;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Range: <color=#00FF00>{rankedRangeMultiplier:F2}x</color>\n\n" +
               $"<b>Per Rank:</b> +{rankBonusPerLevel:F2}x";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        blueprint.TargetingRange *= rankedRangeMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource>(); // Нет ресурсов для регистрации
    }

    public NodeRangeUpgrade(RangeConfig config, int rank) 
    {
        baseRangeMultiplier = config.BaseRangeMultiplier;
        rankBonusPerLevel = config.RankBonusPerLevel;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedRangeMultiplier = baseRangeMultiplier + (rank * rankBonusPerLevel);
    }
}