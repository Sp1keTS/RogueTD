using UnityEngine;

[CreateAssetMenu(fileName = "CrossShotUpgrade", menuName = "Research Tree/Upgrades/Cross Shot Upgrade")]
public class CrossShotBehaviorUpgrade : ProjectileTowerUpgradeTreeNode
{
    public override string TooltipText => "Fires in four directions.";
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Shoots in <color=#00FF00>4 directions</color>\n";
        
    }
    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint,rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    { 
        var newCrossShotBehavior = CreateInstance<CrossShotBehavior>();
        ResourceManager.RegisterSecondaryBehavior(newCrossShotBehavior.SetRankedName(rank), newCrossShotBehavior);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
        
    }

}