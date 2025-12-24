using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpeedUpgrade", menuName = "Research Tree/Upgrades/ProjectileSpeed Upgrade")]
public class NodePTProjectileSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseRangeMultiplier = 1.2f;
    [SerializeField] private float rankBonusPerLevel = 0.05f;
    [SerializeField] private float projectileSpeedBonus = 0.08f;
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseRangeMultiplier + (rank * rankBonusPerLevel);
        blueprint.ProjectileSpeed *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}
