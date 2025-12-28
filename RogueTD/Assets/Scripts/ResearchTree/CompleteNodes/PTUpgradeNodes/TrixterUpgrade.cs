using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TrixterUpgrade", menuName = "Research Tree/Upgrades/Trixter Upgrade")]
public class TrixterUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private HomingRicochetEffect homingRicochetEffect;
    [SerializeField] private float additionalRicochetsPerRank = 0.34f;
    [SerializeField] private float radiusIncreasePerRank = 1f;
    
    public override string TooltipText => "Ricochet to nearest enemy.";
    
    public override string GetStats(int rank)
    {
        if (homingRicochetEffect)
        {
            var baseRicochets = 3;
            var baseRadius = 5f;
            
            return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank)}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Ricochets: <color=#00FF00>{baseRicochets + (rank * additionalRicochetsPerRank)}</color>\n" +
                   $"• Homing Radius: <color=#00FF00>{baseRadius + (rank * radiusIncreasePerRank):F1}</color>\n\n" +
                   $"<b>Per Rank:</b> +{additionalRicochetsPerRank} ricochets, +{radiusIncreasePerRank:F1} radius";
        }
        
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank)}</color></size>\n\n" +
               "<color=#FF5555>Failed to load effect</color>";
    }

    

    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (!homingRicochetEffect)
        {
            Debug.LogError("HomingRicochetEffect is not assigned!");
            return;
        }
        
        RemoveRegularRicochet(blueprint);
        
        homingRicochetEffect.MaxRicochets =  1 + (int)Math.Floor(rank * additionalRicochetsPerRank);
        homingRicochetEffect.HomingRadius = 5f + (rank * radiusIncreasePerRank);
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            homingRicochetEffect, 
            b => b.ProjectileEffects,
            (b, arr) => b.ProjectileEffects = arr
        );
        blueprint.ProjectileFragile = false;
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }
    
    private void RemoveRegularRicochet(ProjectileTowerBlueprint blueprint)
    {
        if (blueprint.ProjectileEffects != null)
        {
            var effects = new System.Collections.Generic.List<ResourceReference<ProjectileEffect>>();
            foreach (var effectRef in blueprint.ProjectileEffects)
            {
                if (effectRef.Value && !(effectRef.Value is RicochetEffect))
                {
                    effects.Add(effectRef);
                }
            }
            blueprint.ProjectileEffects = effects.ToArray();
        }
    }

    public override void LoadDependencies()
    {
        if (homingRicochetEffect)
        {
            ResourceManager.RegisterProjectileEffect(homingRicochetEffect.name, homingRicochetEffect);
        }
    }
}