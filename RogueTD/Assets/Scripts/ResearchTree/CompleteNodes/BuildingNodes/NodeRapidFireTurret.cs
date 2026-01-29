using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeRapidFireTurret", menuName = "Research Tree/Upgrades/Rapid Fire Turret Node")]
public class NodeRapidFireTurret : ProjectileTowerNode<ProjectileTowerBlueprint>
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "High fire rate, low damage.";
    
    [Header("Magazine Settings")]
    [SerializeField] private float magazineSizeMultiplier = 1.5f;
    [SerializeField] private float reloadSpeedMultiplier = 1.3f;
    
    public override string TooltipText => description;
    
    private AmmoBasedShootBehavior newAmmoBehavior;
    public override string GetStats(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Stats (Rank {rank}):</b>\n" +
                   $"{ProjectileTowerBlueprint.GetTowerStats()}\n\n" +
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
        BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
    }
    public NodeRapidFireTurret(RapidFireTowerConfig towerConfig, int rank) : base(GetRankMultiplier(rank), towerConfig)
    {
        description = towerConfig.Description;
        magazineSizeMultiplier = towerConfig.MagazineSizeMultiplier;
        reloadSpeedMultiplier = towerConfig.ReloadSpeedMultiplier;
        Initialize(rank);
    }
    public override List<Resource> GetResources()
    {
        return new List<Resource>{newAmmoBehavior};
    }
    
    private void Initialize(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            newAmmoBehavior = new AmmoBasedShootBehavior();
            ProjectileTowerBlueprint.ShotBehavior = LoadShotBehavior(new BasicShotBehavior());
            ProjectileTowerBlueprint.SecondaryShots = new List<SecondaryProjectileTowerBehavior>() { newAmmoBehavior };
            ResourceManager.RegisterSecondaryBehavior(newAmmoBehavior.SetRankedName(rank),newAmmoBehavior);
        }
    }
}