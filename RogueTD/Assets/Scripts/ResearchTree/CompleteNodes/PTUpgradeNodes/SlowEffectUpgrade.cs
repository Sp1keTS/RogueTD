using UnityEngine;

[CreateAssetMenu(fileName = "SlowUpgrade", menuName = "Research Tree/Upgrades/Slow Upgrade")]
public class SlowEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private float baseSlowPercent = 0.5f;
    [SerializeField] private float baseDuration = 2f;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float slowPercentIncreasePerRank = 0.1f;
    [SerializeField] private float durationIncreasePerRank = 0.5f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Slows enemy movement speed.";

    private float rankedSlowPercent;
    private float rankedDuration;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Slow: <color=#00FF00>{rankedSlowPercent * 100:F0}%</color>\n" +
               $"• Duration: <color=#00FF00>{rankedDuration:F1}s</color>\n\n" +
               $"<b>Per Rank:</b> +{slowPercentIncreasePerRank * 100:F0}%, +{durationIncreasePerRank:F1}s";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        var newSlowEffect = CreateInstance<SlowStatusEffect>();
        newSlowEffect.SlowPercent = rankedSlowPercent;
        newSlowEffect.Duration = rankedDuration;
        ResourceManager.RegisterStatusEffect(newSlowEffect.SetRankedName(rank), newSlowEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        base.Initialize(rank);
        rankedSlowPercent = Mathf.Clamp01(baseSlowPercent + (rank * slowPercentIncreasePerRank));
        rankedDuration = baseDuration + (rank * durationIncreasePerRank);
    }
}