using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TrixterUpgrade", menuName = "Research Tree/Upgrades/Trixter Upgrade")]
public class TrixterUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private int baseRicochets = 3;
    [SerializeField] private float baseRadius = 5f;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float additionalRicochetsPerRank = 0.34f;
    [SerializeField] private float radiusIncreasePerRank = 1f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Ricochet to nearest enemy.";

    private int rankedRicochets;
    private float rankedRadius;
    
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
        var newHomingRicochetEffect = new HomingRicochetEffect
        {
            MaxRicochets = rankedRicochets,
            HomingRadius = rankedRadius
        };
        ResourceManager.RegisterProjectileEffect(newHomingRicochetEffect.SetRankedName(rank), newHomingRicochetEffect);
        
        blueprint.ProjectileFragile = false;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        rankedRicochets = baseRicochets + (int)Math.Floor(rank * additionalRicochetsPerRank);
        rankedRadius = baseRadius + (rank * radiusIncreasePerRank);
    }
}