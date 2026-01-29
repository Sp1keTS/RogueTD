using System.Collections.Generic;
using UnityEngine;

public class NodeAmmoUpgrade : ProjectileTowerUpgradeTreeNode
{
    private float baseAmmoMultiplier;
    private float rankBonusPerLevel;
    private float regenerationBonus;
    private string description;
    
    private float rankedAmmoMultiplier;
    
    public override string TooltipText => description;

    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Ammo: <color=#00FF00>{rankedAmmoMultiplier:F2}x</color>\n" +
               $"• Regen: <color=#00FF00>{1f + (rank * regenerationBonus):F1}x</color>\n\n" +
               $"<b>Per Rank:</b> +{rankBonusPerLevel:F2}x ammo, +{regenerationBonus:F1}x regen";
    }
    
    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        blueprint.MaxAmmo = Mathf.RoundToInt(blueprint.MaxAmmo * rankedAmmoMultiplier);
        blueprint.AmmoRegeneration *= (1f + rank * regenerationBonus);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource>(); 
    }

    public NodeAmmoUpgrade(AmmoUpgradeConfig config,  int rank) 
    {
        baseAmmoMultiplier = config.BaseAmmoMultiplier;
        rankBonusPerLevel = config.RankBonusPerLevel;
        regenerationBonus = config.RegenerationBonus;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedAmmoMultiplier = baseAmmoMultiplier + (rank * rankBonusPerLevel);
    }
}