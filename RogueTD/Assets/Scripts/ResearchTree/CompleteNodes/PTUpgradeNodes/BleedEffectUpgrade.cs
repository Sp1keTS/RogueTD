using UnityEngine;

[CreateAssetMenu(fileName = "BleedUpgrade", menuName = "Research Tree/Upgrades/Bleed Upgrade")]
public class BleedEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    
    [Header("Upgrade Settings")]
    [SerializeField] private float damageIncreasePerRank = 5f;
    [SerializeField] private float durationIncreasePerRank = 0.5f;
    [SerializeField] private float baseDuration;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Damage over time. Based on Turret damage";

    private int rankedDamage;
    private float rankedDuration;
    
    public override string TooltipText => description;

    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank)}</color></size>\n\n " +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Damage: <color=#00FF00> base tower damage + {(rank * damageIncreasePerRank):F0}</color>\n" +
               $"• Duration: <color=#00FF00>{rankedDuration:F1}s</color>\n\n" +
               $"<b>Per Rank:</b> +{damageIncreasePerRank:F0} damage, +{durationIncreasePerRank:F1}s";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint,rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        var newBleedEffect = CreateInstance<BleedEffect>();
        newBleedEffect.Damage = rankedDamage;
        newBleedEffect.Duration = rankedDuration;
        ResourceManager.RegisterStatusEffect(newBleedEffect.SetRankedName(rank), newBleedEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        rankedDamage = (int)(ProjectileTowerBlueprint.Damage * (rank * 0.17));
        rankedDuration = baseDuration + (rank * durationIncreasePerRank);
    }
}