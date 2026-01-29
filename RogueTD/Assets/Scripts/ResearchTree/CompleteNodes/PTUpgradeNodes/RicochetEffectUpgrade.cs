using System;
using System.Collections.Generic;
using UnityEngine;

public class RicochetEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    private int baseRicochets;
    private float baseAngle;
    private float additionalRicochetsPerRank;
    private float angleIncreasePerRank;
    private string description;
    
    private int rankedRicochets;
    private RicochetEffect newRicochetEffect;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Ricochets: <color=#00FF00>{rankedRicochets}</color>\n" +
               $"<b>Per Rank:</b> +{additionalRicochetsPerRank:F1} ricochets, +{angleIncreasePerRank:F0}°";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }

    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        ResourceManager.RegisterProjectileEffect(newRicochetEffect.SetRankedName(rank), newRicochetEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newRicochetEffect };
    }

    public RicochetEffectUpgrade(RicochetConfig config, int rank) 
    {
        baseRicochets = config.BaseRicochets;
        baseAngle = config.BaseAngle;
        additionalRicochetsPerRank = config.AdditionalRicochetsPerRank;
        angleIncreasePerRank = config.AngleIncreasePerRank;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedRicochets = baseRicochets + (int)Math.Floor(rank * additionalRicochetsPerRank);
        newRicochetEffect = new RicochetEffect();
        newRicochetEffect.MaxRicochets = rankedRicochets;
    }
}