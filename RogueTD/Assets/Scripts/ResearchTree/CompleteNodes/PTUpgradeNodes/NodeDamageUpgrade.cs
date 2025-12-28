using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Research Tree/Upgrades/Damage Upgrade")]
public class NodeDamageUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseDamageMultiplier = 1.3f;
    [SerializeField] private float rankBonusPerLevel = 0.1f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Increases damage output.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        float cost = GetDynamicCost(rank);
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Damage: <color=#00FF00>{baseDamageMultiplier + (rank * rankBonusPerLevel):F2}x</color>\n\n" +
               $"<b>Per Rank:</b> +{rankBonusPerLevel:F2}x";
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        
        var totalMultiplier = baseDamageMultiplier + (rank * rankBonusPerLevel);
        blueprint.Damage = Mathf.RoundToInt(blueprint.Damage * totalMultiplier);
        blueprint.DamageMult *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}