

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeRocketLauncherTurret", menuName = "Research Tree/Turrets/Rocket Launcher Turret Node")]
public class NodeRocketLauncherTurret : ProjectileTowerNode
{
    [SerializeField] private ExplosionEffect explosionEffect;
    
    public override string TooltipText => "Rocket launcher with explosions.";
    
    public override string GetStats(int rank)
    {
        if (_ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Rocket Launcher (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void OnActivate(int rank)
    {
        BlueprintManager.InsertProjectileTowerBlueprint(_ProjectileTowerBlueprint);
    }

    public override void Initialize(int rank)
    {
        SetupNode(rank);
    }

    private void SetupNode(int rank)
    {
        CreateBlueprint();
        if (_ProjectileTowerBlueprint != null)
        {
            LoadBasicShot();
            LoadBasicStats(rank, 1.05f * rank);
            _ProjectileTowerBlueprint.ProjectileEffects = new List<ProjectileEffect>() { explosionEffect };
            ResourceManager.RegisterProjectileEffect(explosionEffect.SetRankedName(rank),explosionEffect);
        }
    }
}
    