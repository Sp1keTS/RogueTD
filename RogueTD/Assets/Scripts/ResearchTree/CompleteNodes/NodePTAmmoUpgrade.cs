using UnityEngine;

[CreateAssetMenu(fileName = "AmmoUpgrade", menuName = "Research Tree/Upgrades/Ammo Upgrade")]
public class NodePTAmmoUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseAmmoMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.08f;
    [SerializeField] private float regenerationBonus = 0.1f;
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseAmmoMultiplier + (rank * rankBonusPerLevel);
        blueprint.MaxAmmo = Mathf.RoundToInt(blueprint.MaxAmmo * totalMultiplier);
        blueprint.AmmoRegeneration *= (1f + rank * regenerationBonus);
        
        Debug.Log($"Ammo upgraded to {totalMultiplier:F1}x! New ammo: {blueprint.MaxAmmo}");
    }

    public override void LoadDependencies()
    {
    }
}