using System.Collections.Generic;
using UnityEngine;

public class GrowingSizeUpgrade : ProjectileTowerUpgradeTreeNode
{
    private float baseGrowthRate;
    private float baseMaxSize;
    private float growthRateIncreasePerRank;
    private float maxSizeIncreasePerRank;
    private string description;
    
    private float rankedGrowthRate;
    private float rankedMaxSize;
    private GrowingSizeMovement newGrowingSizeMovement;
    
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
        ResourceManager.RegisterProjectileBehavior(newGrowingSizeMovement.SetRankedName(rank), newGrowingSizeMovement);
        blueprint.ProjectileBehaviors.Add(newGrowingSizeMovement);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newGrowingSizeMovement };
    }

    public GrowingSizeUpgrade(GrowingSizeConfig config, int rank) 
    {
        baseGrowthRate = config.BaseGrowthRate;
        baseMaxSize = config.BaseMaxSize;
        growthRateIncreasePerRank = config.GrowthRateIncreasePerRank;
        maxSizeIncreasePerRank = config.MaxSizeIncreasePerRank;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedGrowthRate = baseGrowthRate + (rank * growthRateIncreasePerRank);
        rankedMaxSize = baseMaxSize + (rank * maxSizeIncreasePerRank);
        newGrowingSizeMovement = new GrowingSizeMovement();
        newGrowingSizeMovement.GrowthRate = rankedGrowthRate;
        newGrowingSizeMovement.MaxSize = rankedMaxSize;
    }
}