using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowUpgrade", menuName = "Research Tree/Upgrades/Slow Upgrade")]
public class SlowEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private SlowStatusEffect slowEffect;
    [SerializeField] private float slowPercentIncreasePerRank = 0.1f;
    [SerializeField] private float durationIncreasePerRank = 0.5f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Adds a slowing effect to your tower's projectiles.\n" +
        "Reduces enemy movement speed for {duration:F1} seconds.\n" +
        "Higher ranks increase the slow percentage and duration.";
    
    public override string TooltipText
    {
        get
        {
            if (slowEffect)
            {
                return $"SLOW EFFECT UPGRADE (Rank {CurrentRank})\n\n" +
                       $"{description}\n\n" +
                       $"Current Effect (Rank {CurrentRank}):\n" +
                       $"• Slow Percentage: {(0.5f + (CurrentRank * slowPercentIncreasePerRank)) * 100:F0}%\n" +
                       $"• Duration: {2 + (CurrentRank * durationIncreasePerRank):F1} seconds\n\n" +
                       $"Rank Bonus:\n" +
                       $"• +{slowPercentIncreasePerRank * 100:F0}% slow per rank\n" +
                       $"• +{durationIncreasePerRank:F1}s duration per rank";
            }
            return $"SLOW EFFECT UPGRADE\n\n{description}";
        }
    }
    
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (slowEffect == null)
        {
            Debug.LogError("SlowEffect is not assigned in SlowEffectUpgrade!");
            return;
        }

        
        ResourceManager.RegisterStatusEffect(slowEffect.name, slowEffect);
        
        slowEffect.SlowPercent = Mathf.Clamp01(0.4f + (float)Math.Pow(rank * slowPercentIncreasePerRank, 0.7));
        slowEffect.duration = 2 + (rank * durationIncreasePerRank);
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            slowEffect, 
            b => b.StatusEffects,
            (b, arr) => b.StatusEffects = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (slowEffect != null)
        {
            ResourceManager.RegisterStatusEffect(slowEffect.name, slowEffect);
        }
    }
}