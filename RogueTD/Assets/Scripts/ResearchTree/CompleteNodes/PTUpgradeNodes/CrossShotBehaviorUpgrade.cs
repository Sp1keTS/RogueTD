using System.Collections.Generic;
using UnityEngine;

public class CrossShotBehaviorUpgrade : ProjectileTowerUpgradeTreeNode
{
    private string description;
    private CrossShotBehavior newCrossShotBehavior;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Shoots in <color=#00FF00>4 directions</color>\n";
    }
    
    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    { 
        ResourceManager.RegisterSecondaryBehavior(newCrossShotBehavior.SetRankedName(rank), newCrossShotBehavior);
        blueprint.SecondaryShots.Add(newCrossShotBehavior);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newCrossShotBehavior };
    }

    public CrossShotBehaviorUpgrade(CrossShotConfig config, int rank) 
    {
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        newCrossShotBehavior = new CrossShotBehavior();
    }
}