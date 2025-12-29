using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeMachineGunTurret", menuName = "Research Tree/Turrets/Machine Gun Turret Node")]
public class NodeMachineGunTurret : ProjectileTowerNode
{
    [SerializeField] private RampingFireRateBehavior rampingBehavior;
    [SerializeField] private AmmoBasedShootBehavior ammoBehavior;
    
    public override string TooltipText => "Machine gun with ramping fire rate.";
    
    public override string GetStats(int rank)
    {
        if (_ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Machine Gun (Rank {rank}):</b>\n";
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
            _ProjectileTowerBlueprint.SecondaryShots = new List<SecondaryProjectileTowerBehavior>() { ammoBehavior };
            ResourceManager.RegisterSecondaryBehavior(ammoBehavior.SetRankedName(rank),ammoBehavior);
            _ProjectileTowerBlueprint.SecondaryShots = new List<SecondaryProjectileTowerBehavior>() { rampingBehavior };
            ResourceManager.RegisterSecondaryBehavior(rampingBehavior.SetRankedName(rank),rampingBehavior);
        }
    }
}