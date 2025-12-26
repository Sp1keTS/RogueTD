using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstShotUpgrade", menuName = "Research Tree/Upgrades/Burst Shot Upgrade")]
public class BurstShotBehaviorUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private BurstShotBehavior burstShotBehavior;
    [SerializeField] private float additionalBurstPerRank = 0.5f;
    [SerializeField] private float delayReductionPerRank = 0.02f;
    
    public override string TooltipText => "Fires multiple shots rapidly.";
    
    public override string GetStats(int rank)
    {
        float cost = GetDynamicCost(rank);
        
        if (burstShotBehavior != null)
        {
            int baseBurstCount = 3;
            float baseDelay = 0.1f;
            float actualDelay = Mathf.Max(0.05f, baseDelay - (rank * delayReductionPerRank));
            
            return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Burst Count: <color=#00FF00>{baseBurstCount + (rank * additionalBurstPerRank)}</color>\n" +
                   $"• Delay: <color=#00FF00>{actualDelay:F2}s</color>\n\n" +
                   $"<b>Per Rank:</b> +{additionalBurstPerRank} shots, -{delayReductionPerRank:F2}s delay";
        }
        
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load effect</color>";
    }

    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.5f));
    }

    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (burstShotBehavior == null)
        {
            Debug.LogError("BurstShotBehavior is not assigned!");
            return;
        }
        
        int baseBurstCount = 2;
        float baseDelay = 0.5f;
        
        burstShotBehavior.BurstCount = (int)Math.Floor(baseBurstCount + (rank * additionalBurstPerRank));
        burstShotBehavior.BurstDelay = Mathf.Max(0.05f, baseDelay - (rank * delayReductionPerRank));
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            burstShotBehavior, 
            b => b.SecondaryShots,
            (b, arr) => b.SecondaryShots = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (burstShotBehavior != null)
        {
            ResourceManager.RegisterSecondaryBehavior(burstShotBehavior.name, burstShotBehavior);
        }
    }
}