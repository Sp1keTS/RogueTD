using System;
using System.Collections.Generic;
using UnityEngine;

public class SplitEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    private int baseSplitCount;
    private float baseAngle;
    private float additionalSplitPerRank;
    private float angleIncreasePerRank;
    private string description;
    
    private int rankedSplitCount;
    private SplitEffect newSplitEffect;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Split Count: <color=#00FF00>{rankedSplitCount}</color>\n" +
               $"<b>Per Rank:</b> +{additionalSplitPerRank:F1} projectiles, +{angleIncreasePerRank:F0}°";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        ResourceManager.RegisterProjectileEffect(newSplitEffect.SetRankedName(rank), newSplitEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newSplitEffect };
    }

    public SplitEffectUpgrade(SplitConfig config, int rank) 
    {
        baseSplitCount = config.BaseSplitCount;
        baseAngle = config.BaseAngle;
        additionalSplitPerRank = config.AdditionalSplitPerRank;
        angleIncreasePerRank = config.AngleIncreasePerRank;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedSplitCount = (int)Math.Floor(baseSplitCount + (rank * additionalSplitPerRank));
        newSplitEffect = new SplitEffect();
        newSplitEffect.SplitCount = rankedSplitCount;
    }
}