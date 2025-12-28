using UnityEngine;

[CreateAssetMenu(fileName = "AmmoUpgrade", menuName = "Research Tree/Upgrades/Ammo Upgrade")]
public class NodeAmmoUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseAmmoMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.08f;
    [SerializeField] private float regenerationBonus = 0.1f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Increases maximum ammo and regeneration rate.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        var cost = GetDynamicCost(rank);
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Ammo: <color=#00FF00>{baseAmmoMultiplier + (rank * rankBonusPerLevel):F2}x</color>\n" +
               $"• Regen: <color=#00FF00>{1f + (rank * regenerationBonus):F1}x</color>\n\n" +
               $"<b>Per Rank:</b> +{rankBonusPerLevel:F2}x ammo, +{regenerationBonus:F1}x regen";
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        
        var totalMultiplier = baseAmmoMultiplier + (rank * rankBonusPerLevel);
        
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