using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpeedUpgrade", menuName = "Research Tree/Upgrades/ProjectileSpeed Upgrade")]
public class NodePTProjectileSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private float baseSpeedMultiplier = 1.2f;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Increases projectile velocity.";

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
        blueprint.ProjectileSpeed *= rankedSpeedMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        base.Initialize(rank);
        rankedSpeedMultiplier = baseSpeedMultiplier + (rank * rankBonusPerLevel);
    }
}