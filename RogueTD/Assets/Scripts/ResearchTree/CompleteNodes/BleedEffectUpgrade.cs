using UnityEngine;

[CreateAssetMenu(fileName = "BleedUpgrade", menuName = "Research Tree/Upgrades/Bleed Upgrade")]
public class BleedEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] BleedEffect bleedEffect;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float totalDamageIncreasePerRank = 5f;
    [SerializeField] private float durationIncreasePerRank = 0.5f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Damage over time. Based on Turret damage";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        if (bleedEffect)
        {
            return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank)}</color></size>\n\n <b>Effect (Rank {rank}):</b>\n" +
                   $"• Damage: <color=#00FF00>{15 + (rank * totalDamageIncreasePerRank):F0}</color>\n" +
                   $"• Duration: <color=#00FF00>{2 + (rank * durationIncreasePerRank):F1}s</color>\n\n" +
                   $"<b>Per Rank:</b> +{totalDamageIncreasePerRank:F0} damage, +{durationIncreasePerRank:F1}s";
        }
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               $"{description}\n\n" +
               "<color=#FF5555>Failed to load effect</color>";
    }

    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.5f));
    }

    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (!bleedEffect)
        {
            Debug.LogError("BleedEffect is not assigned in BleedEffectUpgrade!");
            return;
        }

        ResourceManager.RegisterStatusEffect(bleedEffect.name, bleedEffect);
        
        bleedEffect.TotalDamage = (int)(blueprint.Damage * (rank  * 0.17));
        bleedEffect.duration = 2 + (rank * durationIncreasePerRank);
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            bleedEffect, 
            b => b.StatusEffects,
            (b, arr) => b.StatusEffects = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (bleedEffect)
        {
            ResourceManager.RegisterStatusEffect(bleedEffect.name, bleedEffect);
        }
    }
}