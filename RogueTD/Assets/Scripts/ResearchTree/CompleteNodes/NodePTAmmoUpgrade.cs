using UnityEngine;

[CreateAssetMenu(fileName = "AmmoUpgrade", menuName = "Research Tree/Upgrades/Ammo Upgrade")]
public class NodeAmmoUpgrade : TowerUpgradeTreeNode
{
    [SerializeField] private float baseAmmoMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.08f;
    [SerializeField] private float regenerationBonus = 0.1f;
    
    public override void ApplyUpgrade(TowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseAmmoMultiplier + (rank * rankBonusPerLevel);
        
        if (blueprint is ProjectileTowerBlueprint projectileBlueprint)
        {
            projectileBlueprint.MaxAmmo = Mathf.RoundToInt(projectileBlueprint.MaxAmmo * totalMultiplier);
            projectileBlueprint.AmmoRegeneration *= (1f + rank * regenerationBonus);
            Debug.Log($"Ammo upgraded to {totalMultiplier:F1}x! New ammo: {projectileBlueprint.MaxAmmo}");
        }
        else
        {
            Debug.Log($"Ammo upgrade applied to non-projectile tower: {blueprint.buildingName}");
        }
    }

    public override void LoadDependencies()
    {
        // Не требует зависимостей
    }
}