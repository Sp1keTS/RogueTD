using UnityEngine;

[CreateAssetMenu(fileName = "RangeUpgrade", menuName = "Research Tree/Upgrades/Range Upgrade")]
public class NodeRangeUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseRangeMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    [SerializeField] private float projectileSpeedBonus = 0.08f;
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseRangeMultiplier + (rank * rankBonusPerLevel);
        blueprint.TargetingRange *= totalMultiplier;
    }

    public override void LoadDependencies()
    {
    }
}