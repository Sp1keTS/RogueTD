using System.Collections.Generic;
using UnityEngine;

public class NodeMachineGunTurret : ProjectileTowerNode<ProjectileTowerBlueprint>
{
    public override string TooltipText => description;
    private string description;

    private AmmoBasedShootBehavior ammoBehavior;
    private RampingFireRateBehavior rampingBehavior;
    
    public override string GetStats(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Machine Gun (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource>{ammoBehavior, rampingBehavior};
    }

    public override void OnActivate(int rank)
    {
        BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
    }
    public NodeMachineGunTurret(MachineGunTowerConfig towerConfig, int rank) : base(GetRankMultiplier(rank), towerConfig)
    {
        description = towerConfig.Description;
        Initialize(rank);
        
    }

    private void Initialize(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            ammoBehavior = new AmmoBasedShootBehavior();
            rampingBehavior = new RampingFireRateBehavior();
            
            ProjectileTowerBlueprint.ShotBehavior = LoadShotBehavior(new BasicShotBehavior());
            ProjectileTowerBlueprint.SecondaryShots = new List<SecondaryProjectileTowerBehavior>() { ammoBehavior };
            ResourceManager.RegisterSecondaryBehavior(ammoBehavior.SetRankedName(rank),ammoBehavior);
            ProjectileTowerBlueprint.SecondaryShots = new List<SecondaryProjectileTowerBehavior>() { rampingBehavior };
            ResourceManager.RegisterSecondaryBehavior(rampingBehavior.SetRankedName(rank),rampingBehavior);
        }
    }
}