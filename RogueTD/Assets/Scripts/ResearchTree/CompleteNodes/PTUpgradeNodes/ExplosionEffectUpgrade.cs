using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionUpgrade", menuName = "Research Tree/Upgrades/Explosion Upgrade")]
public class ExplosionEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private ExplosionEffect explosionEffect;
    [SerializeField] private float radiusIncreasePerRank = 0.5f;
    [SerializeField] private float damageIncreaseMultiplierPerRank = 0.1f;
    
    public override string TooltipText => "Projectiles explode on impact.";
    
    public override string GetStats(int rank)
    {
        float cost = GetDynamicCost(rank);
        
        if (explosionEffect != null)
        {
            float baseRadius = 3f;
            float baseDamagePercentage = 0.5f;
            
            return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Radius: <color=#00FF00>{baseRadius + (rank * radiusIncreasePerRank):F1}</color>\n" +
                   $"• Damage: <color=#00FF00>{(baseDamagePercentage + (rank * damageIncreaseMultiplierPerRank)) * 100:F0}%</color> of tower damage\n\n" +
                   $"<b>Per Rank:</b> +{radiusIncreasePerRank:F1} radius, +{damageIncreaseMultiplierPerRank * 100:F0}% damage";
        }
        
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load effect</color>";
    }
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (!explosionEffect)
        {
            Debug.LogError("ExplosionEffect is not assigned!");
            return;
        }

        
        var baseRadius = 3f;
        var baseDamagePercentage = 0.5f;
        
        explosionEffect.ExplosionRadius = baseRadius + (rank * radiusIncreasePerRank);
        explosionEffect.DamagePercentage = Mathf.RoundToInt((baseDamagePercentage + (rank * damageIncreaseMultiplierPerRank)) * 100);
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            explosionEffect, 
            b => b.ProjectileEffects,
            (b, arr) => b.ProjectileEffects = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (explosionEffect)
        {
            ResourceManager.RegisterProjectileEffect(explosionEffect.name, explosionEffect);
        }
    }
}