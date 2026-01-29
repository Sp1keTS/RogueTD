using System.Collections.Generic;
using UnityEngine;

public class NodeAttackSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    private float baseSpeedMultiplier;
    private float rankBonusPerLevel;
    private string description;
    
    private float rankedSpeedMultiplier;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Speed: <color=#00FF00>{rankedSpeedMultiplier:F2}x</color>\n\n" +
               $"<b>Per Rank:</b> +{rankBonusPerLevel:F2}x";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        blueprint.AttackSpeed *= rankedSpeedMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource>(); // Нет ресурсов для регистрации
    }

    public NodeAttackSpeedUpgrade(AttackSpeedConfig config, int rank) 
    {
        baseSpeedMultiplier = config.BaseSpeedMultiplier;
        rankBonusPerLevel = config.RankBonusPerLevel;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank =  rank;
        rankedSpeedMultiplier = baseSpeedMultiplier + (rank * rankBonusPerLevel);
    }
}