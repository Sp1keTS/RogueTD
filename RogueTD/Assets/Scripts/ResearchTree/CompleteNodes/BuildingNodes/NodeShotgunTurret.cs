using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeShotgunTurret", menuName = "Research Tree/Turrets/Shotgun Turret Node")]
public class NodeShotgunTurret : ProjectileTowerNode<ProjectileTowerBlueprint>
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "<b>SHOTGUN TURRET</b>\n" +
        "Close-range multi-projectile.";

    

    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Stats (Rank {rank}):</b>\n" +
                   $"{ProjectileTowerBlueprint.GetTowerStats()}\n\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    public override List<Resource> GetResources()
    {
        return new List<Resource>();
    }
    public override void OnActivate(int rank)
    {
        BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
    }
    
    public NodeShotgunTurret(ShotgunTowerConfig towerConfig, int rank) : base(GetRankMultiplier(rank), towerConfig)
    {
        
        description = towerConfig.Description;
        Initialize(rank);
    }
    private void Initialize(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            ProjectileTowerBlueprint.ShotBehavior = LoadShotBehavior(new BasicShotBehavior());
        }
    }
}