

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeRocketLauncherTurret", menuName = "Research Tree/Turrets/Rocket Launcher Turret Node")]
public class NodeRocketLauncherTurret : ProjectileTowerNode<ProjectileTowerBlueprint>
{
    public override string TooltipText => description;
    private string description;

    private ExplosionEffect explosionEffect;
    public override string GetStats(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Rocket Launcher (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource>{explosionEffect};
    }

    public override void OnActivate(int rank)
    {
        BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
    }
    
    public NodeRocketLauncherTurret(RocketLauncherTowerConfig towerConfig, int rank) : base(GetRankMultiplier(rank), towerConfig)
    {
        description = towerConfig.Description;
        Initialize(rank);
    }
    private void Initialize(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            explosionEffect = new ExplosionEffect();
            ProjectileTowerBlueprint.ShotBehavior = LoadShotBehavior(new BasicShotBehavior());
            ProjectileTowerBlueprint.ProjectileEffects = new List<ProjectileEffect>() { explosionEffect };
            ResourceManager.RegisterProjectileEffect(explosionEffect.SetRankedName(rank),explosionEffect);
        }
    }
}
    