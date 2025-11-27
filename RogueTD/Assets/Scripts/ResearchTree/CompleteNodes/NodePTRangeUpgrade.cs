using UnityEngine;

[CreateAssetMenu(fileName = "RangeUpgrade", menuName = "Research Tree/Upgrades/Range Upgrade")]
public class NodePTRangeUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseRangeMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    [SerializeField] private float projectileSpeedBonus = 0.08f;
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseRangeMultiplier + (rank * rankBonusPerLevel);
        blueprint.TargetingRange *= totalMultiplier;
        blueprint.ProjectileSpeed *= (1f + rank * projectileSpeedBonus);
        
        Debug.Log($"Range upgraded to {totalMultiplier:F1}x! New range: {blueprint.TargetingRange:F1}");
    }

    public override void LoadDependencies()
    {
    }
}