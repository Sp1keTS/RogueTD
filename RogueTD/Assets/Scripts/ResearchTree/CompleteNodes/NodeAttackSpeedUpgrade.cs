using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Research Tree/Upgrades/Attack Speed Upgrade")]
public class NodeAttackSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseSpeedMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.07f;
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseSpeedMultiplier + (rank * rankBonusPerLevel);
        blueprint.AttackSpeed *= totalMultiplier;
        
    }

    public override void LoadDependencies()
    {
    }
}