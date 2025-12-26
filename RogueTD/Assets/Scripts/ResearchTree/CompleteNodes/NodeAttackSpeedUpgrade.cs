using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Research Tree/Upgrades/Attack Speed Upgrade")]
public class NodeAttackSpeedUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private float baseSpeedMultiplier = 1.25f;
    [SerializeField] private float rankBonusPerLevel = 0.07f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Increases rate of fire.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        var cost = GetDynamicCost(rank);
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Speed: <color=#00FF00>{baseSpeedMultiplier + (rank * rankBonusPerLevel):F2}x</color>\n\n" +
               $"<b>Per Rank:</b> +{rankBonusPerLevel:F2}x";
    }
    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.5f));
    }
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        var totalMultiplier = baseSpeedMultiplier + (rank * rankBonusPerLevel);
        blueprint.AttackSpeed *= totalMultiplier;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
    }
}