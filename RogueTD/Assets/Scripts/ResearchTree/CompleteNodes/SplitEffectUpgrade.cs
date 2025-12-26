using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SplitUpgrade", menuName = "Research Tree/Upgrades/Split Upgrade")]
public class SplitEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private SplitEffect splitEffect;
    [SerializeField] private float additionalSplitPerRank = 0.25f;
    [SerializeField] private float angleIncreasePerRank = 10f;
    
    public override string TooltipText => "Projectiles split on impact.";
    
    public override string GetStats(int rank)
    {
        var cost = GetDynamicCost(rank);
        
        if (splitEffect)
        {
            int baseSplitCount = 2;
            float baseAngle = 30f;
            
            return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Split Count: <color=#00FF00>{baseSplitCount + (rank * additionalSplitPerRank)}</color>\n" +
                   $"• Angle: <color=#00FF00>{baseAngle + (rank * angleIncreasePerRank):F0}°</color>\n\n" +
                   $"<b>Per Rank:</b> +{additionalSplitPerRank} projectiles, +{angleIncreasePerRank:F0}°";
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
        if (!splitEffect)
        {
            Debug.LogError("SplitEffect is not assigned!");
            return;
        }

        var totalCost = Cost + Cost * Mathf.Pow(rank, 0.5f);
        
        var baseSplitCount = 2;
        var baseAngle = 30f;
        
        splitEffect.SplitCount = (int)Math.Floor(baseSplitCount + (rank * additionalSplitPerRank));
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            splitEffect, 
            b => b.ProjectileEffects,
            (b, arr) => b.ProjectileEffects = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (splitEffect)
        {
            ResourceManager.RegisterProjectileEffect(splitEffect.name, splitEffect);
        }
    }
}