using System;
using System.Collections.Generic;
using UnityEngine;

public class TrixterUpgrade : ProjectileTowerUpgradeTreeNode
{
    private int baseRicochets;
    private float baseRadius;
    private float additionalRicochetsPerRank;
    private float radiusIncreasePerRank;
    private string description;
    
    private int rankedRicochets;
    private float rankedRadius;
    private HomingRicochetEffect newHomingRicochetEffect;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Ricochets: <color=#00FF00>{rankedRicochets}</color>\n" +
               $"• Homing Radius: <color=#00FF00>{rankedRadius:F1}</color>\n\n" +
               $"<b>Per Rank:</b> +{additionalRicochetsPerRank:F1} ricochets, +{radiusIncreasePerRank:F1} radius";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }

    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        ResourceManager.RegisterProjectileEffect(newHomingRicochetEffect.SetRankedName(rank), newHomingRicochetEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newHomingRicochetEffect };
    }

    public TrixterUpgrade(TrixterConfig config, int rank) 
    {
        baseRicochets = config.BaseRicochets;
        baseRadius = config.BaseRadius;
        additionalRicochetsPerRank = config.AdditionalRicochetsPerRank;
        radiusIncreasePerRank = config.RadiusIncreasePerRank;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedRicochets = baseRicochets + (int)Math.Floor(rank * additionalRicochetsPerRank);
        rankedRadius = baseRadius + (rank * radiusIncreasePerRank);
        newHomingRicochetEffect = new HomingRicochetEffect();
        newHomingRicochetEffect.HomingRadius = rankedRadius;
        newHomingRicochetEffect.MaxRicochets = rankedRicochets;
    }
}