using UnityEngine;

[CreateAssetMenu(fileName = "NodeFlamethrowerTurret", menuName = "Research Tree/Turrets/Flamethrower Turret Node")]
public class NodeFlamethrowerTurret : ProjectileTowerNode
{
    
    [SerializeField] private BurnEffect burnEffect;
    
    public override string TooltipText => "Flamethrower tower.";
    
    public override string GetStats(int rank)
    {
        if (ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Flamethrower (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        
        ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
        ProjectileTowerBlueprint.Initialize(buildingName, ProjectileTower, buildingPrefab, maxHealthPoints, buildingCost, size);
        
        if (ProjectileTowerBlueprint != null)
        {
            LoadBasicShot();
            LoadBasicStats(rank, 0.5f);
            if (burnEffect)
            {
                ProjectileTowerBlueprint.StatusEffects = new ResourceReference<StatusEffect>[]
                {
                    new ResourceReference<StatusEffect> { Value = burnEffect }
                };
            }
            
            
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (ProjectileTowerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
        }
        if (burnEffect)
        {
            ResourceManager.RegisterStatusEffect(burnEffect.name, burnEffect);
        }
    }
}