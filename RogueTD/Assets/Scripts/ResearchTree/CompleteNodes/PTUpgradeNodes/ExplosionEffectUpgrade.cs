using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionUpgrade", menuName = "Research Tree/Upgrades/Explosion Upgrade")]
public class ExplosionEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private float baseRadius = 3f;
    [SerializeField] private float baseDamagePercentage = 30f; 
    
    [Header("Upgrade Settings")]
    [SerializeField] private float radiusIncreasePerRank = 0.5f;
    [SerializeField] private float damageIncreasePerRank = 10f; 
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Projectiles explode on impact.";

    private float rankedRadius;
    private int rankedDamagePercentage;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Radius: <color=#00FF00>{rankedRadius:F1}</color>\n" +
               $"• Damage: <color=#00FF00>{rankedDamagePercentage:F0}%</color> of tower damage\n\n" +
               $"<b>Per Rank:</b> +{radiusIncreasePerRank:F1} radius, +{damageIncreasePerRank:F0}% damage";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        var newExplosionEffect = CreateInstance<ExplosionEffect>();
        newExplosionEffect.DamagePercentage = rankedDamagePercentage;
        newExplosionEffect.ExplosionRadius = rankedRadius;
        ResourceManager.RegisterProjectileEffect(newExplosionEffect.SetRankedName(rank), newExplosionEffect);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        rankedRadius = baseRadius + (rank * radiusIncreasePerRank);
        rankedDamagePercentage = Mathf.RoundToInt(baseDamagePercentage + (rank * damageIncreasePerRank));
    }
}