using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

[CreateAssetMenu(fileName = "NodeFlamethrowerTurret", menuName = "Research Tree/Turrets/Flamethrower Turret Node")]
public class NodeFlamethrowerTurret : ProjectileTowerNode<ProjectileTowerBlueprint>
{

    public override string TooltipText => description;
    private string description;
    
    private BurnEffect burnEffect;
    public override string GetStats(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Flamethrower (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource>{burnEffect};
    }
    
    public NodeFlamethrowerTurret(FlamethrowerTowerConfig towerConfig, int rank) : base(GetRankMultiplier(rank), towerConfig)
    {
        description = towerConfig.Description;
        Initialize(rank);

        burnEffect.Duration = towerConfig.GetRankedDuration(rank);
        burnEffect.DamagePerTick = (int)towerConfig.GetRankedDamagePerTick(rank);
    }

    public override void OnActivate(int rank)
    {
        BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
    }

    private void Initialize(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            burnEffect = new BurnEffect();
            ProjectileTowerBlueprint.ShotBehavior = LoadShotBehavior(new BasicShotBehavior());
            ProjectileTowerBlueprint.StatusEffects = new List<StatusEffect>() { burnEffect };
            ResourceManager.RegisterStatusEffect(burnEffect.SetRankedName(rank),burnEffect);
        }
    }
}