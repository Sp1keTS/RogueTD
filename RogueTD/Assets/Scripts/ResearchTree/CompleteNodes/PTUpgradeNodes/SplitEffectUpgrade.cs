using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SplitUpgrade", menuName = "Research Tree/Upgrades/Split Upgrade")]
public class SplitEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private int baseSplitCount = 2;
    [SerializeField] private float baseAngle = 30f;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float additionalSplitPerRank = 0.25f;
    [SerializeField] private float angleIncreasePerRank = 10f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Projectiles split on impact.";

    private int rankedSplitCount;
    
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
        var newSplitEffect = new SplitEffect
        {
            SplitCount = rankedSplitCount,
        };
        ResourceManager.RegisterProjectileEffect(newSplitEffect.SetRankedName(rank), newSplitEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        rankedSplitCount = (int)Math.Floor(baseSplitCount + (rank * additionalSplitPerRank));
    }
}