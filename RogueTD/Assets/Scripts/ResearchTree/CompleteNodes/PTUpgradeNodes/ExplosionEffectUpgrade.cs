using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    private float baseRadius;
    private float baseDamagePercentage;
    private float radiusIncreasePerRank;
    private float damageIncreasePerRank;
    private string description;
    
    private float rankedRadius;
    private int rankedDamagePercentage;
    private ExplosionEffect newExplosionEffect;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Radius: <color=#00FF00>{rankedRadius:F1}</color>\n" +
               $"• Damage: <color=#00FF00>{rankedDamagePercentage:F0}%</color> of tower damage\n\n" +
               $"<b>Per Rank:</b> +{radiusIncreasePerRank:F1} radius, +{damageIncreasePerRank:F0}% damage";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        ResourceManager.RegisterProjectileEffect(newExplosionEffect.SetRankedName(rank), newExplosionEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newExplosionEffect };
    }

    public ExplosionEffectUpgrade(ExplosionConfig config, int rank) 
    {
        baseRadius = config.BaseRadius;
        baseDamagePercentage = config.BaseDamagePercentage;
        radiusIncreasePerRank = config.RadiusIncreasePerRank;
        damageIncreasePerRank = config.DamageIncreasePerRank;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedRadius = baseRadius + (rank * radiusIncreasePerRank);
        rankedDamagePercentage = Mathf.RoundToInt(baseDamagePercentage + (rank * damageIncreasePerRank));
        newExplosionEffect = new ExplosionEffect();
        newExplosionEffect.DamagePercentage = rankedDamagePercentage;
        newExplosionEffect.ExplosionRadius = rankedRadius;
    }
}