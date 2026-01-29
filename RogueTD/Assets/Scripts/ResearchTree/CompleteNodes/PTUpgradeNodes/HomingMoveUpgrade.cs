using System.Collections.Generic;
using UnityEngine;

public class HomingMovementUpgrade : ProjectileTowerUpgradeTreeNode
{
    private float baseRadius;
    private float baseRotationSpeed;
    private float radiusIncreasePerRank;
    private float rotationSpeedIncreasePerRank;
    private string description;
    
    private float rankedRadius;
    private float rankedRotationSpeed;
    private HomingMovement newHomingMovement;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        return $"<size=120%><color=#FFD700>Cost: {GetDynamicCost(rank):F0}</color></size>\n\n" +
               $"<b>Effect (Rank {rank}):</b>\n" +
               $"• Radius: <color=#00FF00>{rankedRadius:F1}</color>\n" +
               $"• Turn Speed: <color=#00FF00>{rankedRotationSpeed:F1}</color>\n\n" +
               $"<b>Per Rank:</b> +{radiusIncreasePerRank:F1} radius, +{rotationSpeedIncreasePerRank:F1} speed";
    }

    public override void OnActivate(int rank)
    {
        ApplyUpgrade(ProjectileTowerBlueprint, rank);
    }
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        ResourceManager.RegisterProjectileBehavior(newHomingMovement.SetRankedName(rank), newHomingMovement);
        blueprint.ProjectileBehaviors.Add(newHomingMovement);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override List<Resource> GetResources()
    {
        return new List<Resource> { newHomingMovement };
    }

    public HomingMovementUpgrade(HomingConfig config, int rank) 
    {
        baseRadius = config.BaseRadius;
        baseRotationSpeed = config.BaseRotationSpeed;
        radiusIncreasePerRank = config.RadiusIncreasePerRank;
        rotationSpeedIncreasePerRank = config.RotationSpeedIncreasePerRank;
        description = config.Description;
        Initialize(rank);
    }

    public void Initialize(int rank)
    {
        CurrentRank = rank;
        rankedRadius = baseRadius + (rank * radiusIncreasePerRank);
        rankedRotationSpeed = baseRotationSpeed + (rank * rotationSpeedIncreasePerRank);
        newHomingMovement = new HomingMovement();
        newHomingMovement.HomingRadius = rankedRadius;
        newHomingMovement.RotationSpeed = rankedRotationSpeed;
    }
}