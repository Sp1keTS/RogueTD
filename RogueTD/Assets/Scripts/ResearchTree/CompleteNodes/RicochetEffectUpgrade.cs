using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Research Tree/Upgrades/Ricochet Upgrade")]
public class RicochetEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private RicochetEffect ricochetEffect;
    [SerializeField] private float additionalRicochetsPerRank = 0.34f;
    [SerializeField] private float angleIncreasePerRank = 5f;
    
    public override string TooltipText => "Projectiles ricochet on hit.";
    
    public override string GetStats(int rank)
    {
        if (ricochetEffect)
        {
            int baseRicochets = 1;
            float baseAngle = 15f;
            
            return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank)}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Ricochets: <color=#00FF00>{baseRicochets + (rank * additionalRicochetsPerRank)}</color>\n" +
                   $"• Angle Change: <color=#00FF00>{baseAngle + (rank * angleIncreasePerRank):F0}°</color>\n\n" +
                   $"<b>Per Rank:</b> +{additionalRicochetsPerRank} ricochets, +{angleIncreasePerRank:F0}°";
        }
        
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank)}</color></size>\n\n" +
               "<color=#FF5555>Failed to load effect</color>";
    }

    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.5f));
    }

    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (!ricochetEffect)
        {
            Debug.LogError("RicochetEffect is not assigned!");
            return;
        }
        
        ricochetEffect.MaxRicochets = 1 + (int)Math.Floor(rank * additionalRicochetsPerRank);
        ricochetEffect.AngleChange = 15f + (rank * angleIncreasePerRank);
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            ricochetEffect, 
            b => b.ProjectileEffects,
            (b, arr) => b.ProjectileEffects = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (ricochetEffect)
        {
            ResourceManager.RegisterProjectileEffect(ricochetEffect.name, ricochetEffect);
        }
    }
}