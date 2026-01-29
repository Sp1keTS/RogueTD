using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicTurret", menuName = "Research Tree/Turrets/Basic Turret node")]
public class NodeBasicTurret : ProjectileTowerNode<ProjectileTowerBlueprint>
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Balanced all-rounder tower.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Stats (Rank {rank}):</b>\n" +
                   $"{ProjectileTowerBlueprint.GetTowerStats()}";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    public NodeBasicTurret(BasicTurretConfig config, int rank) : base(GetRankMultiplier(rank), config)
    {
        description = config.Description;
        Initialize(rank);
    }
    public override List<Resource> GetResources()
    {
        return new List<Resource>();
    }

    public override void OnActivate(int rank)
    {
        BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
    }
    public void Initialize(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            ProjectileTowerBlueprint.ShotBehavior = LoadShotBehavior(new BasicShotBehavior());
        }
    }
}