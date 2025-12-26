using UnityEngine;

[CreateAssetMenu(fileName = "HomingUpgrade", menuName = "Research Tree/Upgrades/Homing Upgrade")]
public class HomingMovementUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] private HomingMovement homingMovement;
    [SerializeField] private float radiusIncreasePerRank = 0.5f;
    [SerializeField] private float rotationSpeedIncreasePerRank = 1f;
    
    public override string TooltipText => "Projectiles track enemies.";
    
    public override string GetStats(int rank)
    {
        var cost = GetDynamicCost(rank);
        
        if (homingMovement)
        {
            var baseRadius = 5f;
            var baseRotationSpeed = 5f;
            
            return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
                   $"<b>Effect (Rank {rank}):</b>\n" +
                   $"• Radius: <color=#00FF00>{baseRadius + (rank * radiusIncreasePerRank):F1}</color>\n" +
                   $"• Turn Speed: <color=#00FF00>{baseRotationSpeed + (rank * rotationSpeedIncreasePerRank):F1}</color>\n\n" +
                   $"<b>Per Rank:</b> +{radiusIncreasePerRank:F1} radius, +{rotationSpeedIncreasePerRank:F1} speed";
        }
        
        return $"<size=120%><color=#FFD700>Cost: {cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load effect</color>";
    }
    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.5f));
    }
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (!homingMovement)
        {
            Debug.LogError("HomingMovement is not assigned!");
            return;
        }
        
        var baseRadius = 5f;
        var baseRotationSpeed = 5f;
        
        homingMovement.HomingRadius = baseRadius + (rank * radiusIncreasePerRank);
        homingMovement.RotationSpeed = baseRotationSpeed + (rank * rotationSpeedIncreasePerRank);
        
        EffectUtils.AddEffectToBlueprint(
            blueprint, 
            homingMovement, 
            b => b.ProjectileBehaviors,
            (b, arr) => b.ProjectileBehaviors = arr
        );
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    public override void LoadDependencies()
    {
        if (homingMovement)
        {
            ResourceManager.RegisterProjectileBehavior(homingMovement.name, homingMovement);
        }
    }
}