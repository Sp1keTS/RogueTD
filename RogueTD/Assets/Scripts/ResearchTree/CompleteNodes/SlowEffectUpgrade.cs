using UnityEngine;

[CreateAssetMenu(fileName = "SlowUpgrade", menuName = "Research Tree/Upgrades/Slow Upgrade")]
public class SlowEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private SlowStatusEffect slowEffect;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float slowPercentIncreasePerRank = 0.1f;
    [SerializeField] private float durationIncreasePerRank = 0.5f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Slows enemy movement speed.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        float cost = Cost + Cost * Mathf.Pow(rank, 0.5f);
        
        if (slowEffect)
        {
            return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Slow: <color=#00FF00>{(0.5f + (rank * slowPercentIncreasePerRank)) * 100:F0}%</color>\n" +
                   $"• Duration: <color=#00FF00>{2 + (rank * durationIncreasePerRank):F1}s</color>\n\n" +
                   $"<b>Per Rank:</b> +{slowPercentIncreasePerRank * 100:F0}%, +{durationIncreasePerRank:F1}s";
        }
        
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               $"{description}\n\n" +
               "<color=#FF5555>Failed to load effect</color>";
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (slowEffect == null)
        {
            Debug.LogError("SlowEffect is not assigned in SlowEffectUpgrade!");
            return;
        }

        GameState.Instance.SpendCurrency((int)(Cost * Mathf.Pow(rank, 0.5f)));
        ResourceManager.RegisterStatusEffect(slowEffect.name, slowEffect);
        
        slowEffect.SlowPercent = Mathf.Clamp01(0.5f + (rank * slowPercentIncreasePerRank));
        slowEffect.duration = 2 + (rank * durationIncreasePerRank);
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            slowEffect, 
            b => b.StatusEffects,
            (b, arr) => b.StatusEffects = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (slowEffect != null)
        {
            ResourceManager.RegisterStatusEffect(slowEffect.name, slowEffect);
        }
    }
}