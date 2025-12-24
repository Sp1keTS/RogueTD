using UnityEngine;

[CreateAssetMenu(fileName = "AmmoUpgrade", menuName = "Research Tree/Upgrades/Ammo Upgrade")]
public class NodeAmmoUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseAmmoMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.08f;
    [SerializeField] private float regenerationBonus = 0.1f;
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseAmmoMultiplier + (rank * rankBonusPerLevel);
        
        if (blueprint is ProjectileTowerBlueprint projectileBlueprint)
        {
            projectileBlueprint.MaxAmmo = Mathf.RoundToInt(projectileBlueprint.MaxAmmo * totalMultiplier);
            projectileBlueprint.AmmoRegeneration *= (1f + rank * regenerationBonus);
        }
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}