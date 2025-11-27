using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Research Tree/Upgrades/Damage Upgrade")]
public class NodePTDamageUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseDamageMultiplier = 1.3f;
    [SerializeField] private float rankBonusPerLevel = 0.1f;
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        float totalMultiplier = baseDamageMultiplier + (rank * rankBonusPerLevel);
        blueprint.Damage = Mathf.RoundToInt(blueprint.Damage * totalMultiplier);
        blueprint.DamageMult *= totalMultiplier;
        
        Debug.Log($"Damage upgraded to {totalMultiplier:F1}x! New damage: {blueprint.Damage}");
    }

    public override void LoadDependencies()
    {
    }
}