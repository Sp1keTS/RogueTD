using System.Collections.Generic;
using UnityEngine;

public class SlowEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    private float baseSlowPercent;
    private float baseDuration;
    private float slowPercentIncreasePerRank;
    private float durationIncreasePerRank;
    private string description;
    
    private float rankedSlowPercent;
    private float rankedDuration;
    private SlowStatusEffect newSlowEffect;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Slow: <color=#00FF00>{rankedSlowPercent * 100:F0}%</color>\n" +
               $"• Duration: <color=#00FF00>{rankedDuration:F1}s</color>\n\n" +
               $"<b>Per Rank:</b> +{slowPercentIncreasePerRank * 100:F0}%, +{durationIncreasePerRank:F1}s";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        ResourceManager.RegisterStatusEffect(newSlowEffect.SetRankedName(rank), newSlowEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newSlowEffect };
    }

    public SlowEffectUpgrade(SlowEffectConfig config, int rank)
    {
        Initialize(rank);
        baseSlowPercent = config.BaseSlowPercent;
        baseDuration = config.BaseDuration;
        slowPercentIncreasePerRank = config.SlowPercentIncreasePerRank;
        durationIncreasePerRank = config.DurationIncreasePerRank;
        description = config.Description;
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedSlowPercent = Mathf.Clamp01(baseSlowPercent + (rank * slowPercentIncreasePerRank));
        rankedDuration = baseDuration + (rank * durationIncreasePerRank);
        newSlowEffect = new SlowStatusEffect();
        newSlowEffect.SlowPercent = rankedSlowPercent;
        newSlowEffect.Duration = rankedDuration;
    }
}