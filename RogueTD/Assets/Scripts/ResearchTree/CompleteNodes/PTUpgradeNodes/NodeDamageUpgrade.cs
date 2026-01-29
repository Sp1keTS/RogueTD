using System.Collections.Generic;
using UnityEngine;

public class NodeDamageUpgrade : ProjectileTowerUpgradeTreeNode
{
    private float baseDamageMultiplier;
    private float rankBonusPerLevel;
    private string description;
    
    private float rankedDamageMultiplier;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Damage: <color=#00FF00>{rankedDamageMultiplier:F2}x</color>\n\n" +
               $"<b>Per Rank:</b> +{rankBonusPerLevel:F2}x";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        blueprint.Damage = Mathf.RoundToInt(blueprint.Damage * rankedDamageMultiplier);
        blueprint.DamageMult *= rankedDamageMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource>(); // Нет ресурсов для регистрации
    }

    public NodeDamageUpgrade(DamageConfig config, int rank) 
    {
        baseDamageMultiplier = config.BaseDamageMultiplier;
        rankBonusPerLevel = config.RankBonusPerLevel;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedDamageMultiplier = baseDamageMultiplier + (rank * rankBonusPerLevel);
    }
}