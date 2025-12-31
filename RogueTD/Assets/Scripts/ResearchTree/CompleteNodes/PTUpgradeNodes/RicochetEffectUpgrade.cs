using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Research Tree/Upgrades/Ricochet Upgrade")]
public class RicochetEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private int baseRicochets = 1;
    [SerializeField] private float baseAngle = 15f;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float additionalRicochetsPerRank = 0.34f;
    [SerializeField] private float angleIncreasePerRank = 5f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Projectiles ricochet on hit.";

    private int rankedRicochets;
    
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
        var newRicochetEffect = CreateInstance<RicochetEffect>();
        newRicochetEffect.MaxRicochets = rankedRicochets;
        ResourceManager.RegisterProjectileEffect(newRicochetEffect.SetRankedName(rank), newRicochetEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        base.Initialize(rank);
        rankedRicochets = baseRicochets + (int)Math.Floor(rank * additionalRicochetsPerRank);
    }
}