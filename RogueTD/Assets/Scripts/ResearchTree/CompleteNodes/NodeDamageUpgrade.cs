using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Research Tree/Upgrades/Damage Upgrade")]
public class NodeDamageUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseDamageMultiplier = 1.3f;
    [SerializeField] private float rankBonusPerLevel = 0.1f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Massively increases your tower's damage output.\n" +
        "Boosts both base damage and damage multiplier for devastating attacks.\n" +
        "Essential for dealing with armored or high-health enemies.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"Cost: {Cost + Cost * Mathf.Pow(rank, 0.5f):F0}\n" +
               $"{description}\n\n" +
               $"Current Effect (Rank {rank}):\n" +
               $"• Damage Multiplier: {baseDamageMultiplier + (rank * rankBonusPerLevel):F2}x\n\n" +
               $"Rank Bonus:\n" +
               $"• +{rankBonusPerLevel:F2}x multiplier per rank";
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        GameState.Instance.SpendCurrency((int)(Cost * Mathf.Pow(rank, 0.5f)));
        
        float totalMultiplier = baseDamageMultiplier + (rank * rankBonusPerLevel);
        blueprint.Damage = Mathf.RoundToInt(blueprint.Damage * totalMultiplier);
        blueprint.DamageMult *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}