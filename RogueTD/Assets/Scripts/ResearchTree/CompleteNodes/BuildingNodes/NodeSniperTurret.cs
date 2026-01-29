using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeSniperTurret", menuName = "Research Tree/Turrets/Sniper Turret Node")]
public class NodeSniperTurret : ProjectileTowerNode<ProjectileTowerBlueprint>
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Long-range high damage.";
    
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

    public NodeSniperTurret(SniperTowerConfig towerConfig, int rank) : base(GetRankMultiplier(rank), towerConfig)
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