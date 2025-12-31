using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstShotUpgrade", menuName = "Research Tree/Upgrades/Burst Shot Upgrade")]
public class BurstShotBehaviorUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private int baseBurstCount = 2;
    [SerializeField] private float baseBurstDelay = 0.1f;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float additionalBurstPerRank = 0.5f;
    [SerializeField] private float delayReductionPerRank = 0.02f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Fires multiple shots rapidly.";

    private int rankedBurstCount;
    private float rankedBurstDelay;
    
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
        ApplyUpgrade(ProjectileTowerBlueprint,rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    { 
        var newBurstShotBehavior = ScriptableObject.CreateInstance<BurstShotBehavior>();
        newBurstShotBehavior.BurstCount = baseBurstCount;
        newBurstShotBehavior.BurstDelay = rankedBurstDelay;
        ResourceManager.RegisterSecondaryBehavior(newBurstShotBehavior.SetRankedName(rank), newBurstShotBehavior);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
        
    }

    public override void Initialize(int rank)
    {
        rankedBurstCount = (int)Math.Floor(baseBurstCount + (rank * additionalBurstPerRank));
        rankedBurstDelay = Mathf.Max(0.05f, baseBurstDelay - (rank * delayReductionPerRank));
    }
}