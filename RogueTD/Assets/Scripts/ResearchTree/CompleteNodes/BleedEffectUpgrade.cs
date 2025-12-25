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
        "Adds a powerful bleeding effect to your tower's projectiles.\n" +
        "Inflicts damage over time to enemies, causing them to bleed.\n" +
        "Higher ranks increase bleed damage and duration.";
    
    public override string TooltipText
    {
        get
        {
            if (bleedEffect != null)
            {
                return $"BLEED EFFECT UPGRADE (Rank {CurrentRank})\n\n" +
                       $"{description}\n\n" +
                       $"Current Effect (Rank {CurrentRank}):\n" +
                       $"• Total Bleed Damage: {15 + (CurrentRank * totalDamageIncreasePerRank):F0}\n" +
                       $"• Duration: {2 + (CurrentRank * durationIncreasePerRank):F1} seconds\n" +
                       $"• Damage per Tick: {(15 + (CurrentRank * totalDamageIncreasePerRank)) / ((2 + (CurrentRank * durationIncreasePerRank)) / 0.5f):F1}\n\n" +
                       $"Rank Bonus:\n" +
                       $"• +{totalDamageIncreasePerRank:F0} total damage per rank\n" +
                       $"• +{durationIncreasePerRank:F1}s duration per rank";
            }
            return $"BLEED EFFECT UPGRADE\n\n{description}";
        }
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