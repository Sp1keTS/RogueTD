using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeRapidFireTurret", menuName = "Research Tree/Upgrades/Rapid Fire Turret Node")]
public class NodeRapidFireTurret : ProjectileTowerNode
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "High fire rate, low damage.";
    
    [Header("Magazine Settings")]
    [SerializeField] private float magazineSizeMultiplier = 1.5f;
    [SerializeField] private float reloadSpeedMultiplier = 1.3f;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        if (_ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Stats (Rank {rank}):</b>\n" +
                   $"{_ProjectileTowerBlueprint.GetTowerStats()}\n\n" +
                   $"<b>Special:</b>\n" +
                   $"• Magazine system\n" +
                   $"• Fires until empty\n" +
                   $"• Pauses to reload";
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
        if (_ProjectileTowerBlueprint != null)
        {
            _ProjectileTowerBlueprint.BuildingName = buildingName;
            var ammoBehavior = new AmmoBasedShootBehavior();
            LoadBasicShot();
            LoadBasicStats(rank, 1.05f * rank);
            _ProjectileTowerBlueprint.SecondaryShots = new List<SecondaryProjectileTowerBehavior>() { ammoBehavior };
            ResourceManager.RegisterSecondaryBehavior(ammoBehavior.SetRankedName(rank),ammoBehavior);
        }
    }
}