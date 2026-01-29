using System;
using System.Collections.Generic;
using UnityEngine;

public class BurstShotBehaviorUpgrade : ProjectileTowerUpgradeTreeNode
{
    private int baseBurstCount;
    private float baseBurstDelay;
    private float additionalBurstPerRank;
    private float delayReductionPerRank;
    private string description;
    
    private int rankedBurstCount;
    private float rankedBurstDelay;
    private BurstShotBehavior newBurstShotBehavior;
    
    public override string TooltipText => description;

    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Burst Count: <color=#00FF00>{rankedBurstCount:F1}</color>\n" +
               $"• Delay: <color=#00FF00>{rankedBurstDelay:F2}s</color>\n\n" +
               $"<b>Per Rank:</b> +{additionalBurstPerRank:F1} shots, -{delayReductionPerRank:F2}s delay";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    { 
        ResourceManager.RegisterSecondaryBehavior(newBurstShotBehavior.SetRankedName(rank), newBurstShotBehavior);
        blueprint.SecondaryShots.Add(newBurstShotBehavior);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newBurstShotBehavior };
    }

    public BurstShotBehaviorUpgrade(BurstShotConfig config, int rank) 
    {
        baseBurstCount = config.BaseBurstCount;
        baseBurstDelay = config.BaseBurstDelay;
        additionalBurstPerRank = config.AdditionalBurstPerRank;
        delayReductionPerRank = config.DelayReductionPerRank;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedBurstCount = (int)Math.Floor(baseBurstCount + (rank * additionalBurstPerRank));
        rankedBurstDelay = Mathf.Max(0.05f, baseBurstDelay - (rank * delayReductionPerRank));
        newBurstShotBehavior = new BurstShotBehavior();
        newBurstShotBehavior.BurstCount = rankedBurstCount;
        newBurstShotBehavior.BurstDelay = rankedBurstDelay;
    }
}