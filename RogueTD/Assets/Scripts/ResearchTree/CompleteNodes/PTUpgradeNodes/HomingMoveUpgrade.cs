using UnityEngine;

[CreateAssetMenu(fileName = "HomingUpgrade", menuName = "Research Tree/Upgrades/Homing Upgrade")]
public class HomingMovementUpgrade : ProjectileTowerUpgradeTreeNode
{
    [Header("Base Settings")]
    [SerializeField] private float baseRadius = 5f;
    [SerializeField] private float baseRotationSpeed = 5f;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float radiusIncreasePerRank = 0.5f;
    [SerializeField] private float rotationSpeedIncreasePerRank = 1f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Projectiles track enemies.";

    private float rankedRadius;
    private float rankedRotationSpeed;
    
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
        var newHomingMovement = CreateInstance<HomingMovement>();
        newHomingMovement.HomingRadius = rankedRadius;
        newHomingMovement.RotationSpeed = rankedRotationSpeed;
        ResourceManager.RegisterProjectileBehavior(newHomingMovement.SetRankedName(rank), newHomingMovement);
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void Initialize(int rank)
    {
        rankedRadius = baseRadius + (rank * radiusIncreasePerRank);
        rankedRotationSpeed = baseRotationSpeed + (rank * rotationSpeedIncreasePerRank);
    }
}