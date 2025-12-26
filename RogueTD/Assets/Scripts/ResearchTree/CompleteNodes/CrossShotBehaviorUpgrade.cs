using UnityEngine;

[CreateAssetMenu(fileName = "CrossShotUpgrade", menuName = "Research Tree/Upgrades/Cross Shot Upgrade")]
public class CrossShotBehaviorUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private CrossShotBehavior crossShotBehavior;
    
    public override string TooltipText => "Fires in four directions.";
    
    public override string GetStats(int rank)
    {
        float cost = GetDynamicCost(rank);
        
        if (crossShotBehavior != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Shoots in <color=#00FF00>4 directions</color>\n" +
                   $"• Angles: <color=#00FF00>0°, 90°, 180°, 270°</color>\n\n" +
                   $"Full coverage in all directions";
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
        if (crossShotBehavior == null)
        {
            Debug.LogError("CrossShotBehavior is not assigned!");
            return;
        }

        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            crossShotBehavior, 
            b => b.SecondaryShots,
            (b, arr) => b.SecondaryShots = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (crossShotBehavior != null)
        {
            ResourceManager.RegisterSecondaryBehavior(crossShotBehavior.name, crossShotBehavior);
        }
    }
}