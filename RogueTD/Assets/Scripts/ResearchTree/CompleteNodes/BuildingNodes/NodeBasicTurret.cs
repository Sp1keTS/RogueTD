using UnityEngine;

[CreateAssetMenu(fileName = "BasicTurret", menuName = "Research Tree/Turrets/Basic Turret node")]
public class NodeBasicTurret : ProjectileTowerNode
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
    
    public override void Initialize(int rank)
    {
        ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
        ProjectileTowerBlueprint.Initialize(buildingName, ProjectileTower, buildingPrefab, maxHealthPoints, buildingCost, size);
        
        if (ProjectileTowerBlueprint != null)
        {
            ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
            var rankMultiplier = 1f + (rank * 0.2f);
            
            ProjectileTowerBlueprint.Damage = Mathf.RoundToInt(5 * rankMultiplier);
            ProjectileTowerBlueprint.AttackSpeed = Random.Range(0.8f, 1.2f) * (1f + (rank * 0.1f));
            ProjectileTowerBlueprint.TargetingRange = Random.Range(4.5f, 5.5f) + (rank * 0.5f);
            ProjectileTowerBlueprint.RotatingSpeed = Random.Range(90f, 110f) + (rank * 10f);
            
            ProjectileTowerBlueprint.ProjectileSpeed = Random.Range(13f, 17f) + (rank * 1f);
            ProjectileTowerBlueprint.ProjectileLifetime = Random.Range(1.8f, 2.2f) + (rank * 0.1f);
            ProjectileTowerBlueprint.Spread = Random.Range(0f, 2f);
            ProjectileTowerBlueprint.ProjectileFragile = Random.value > 0.7f;
            
            ProjectileTowerBlueprint.ProjectileCount = 1;
            
            ProjectileTowerBlueprint.MaxAmmo = Random.Range(25, 35) + (rank * 5);
            ProjectileTowerBlueprint.CurrentAmmo = ProjectileTowerBlueprint.MaxAmmo;
            ProjectileTowerBlueprint.AmmoRegeneration = Random.Range(0.8f, 1.2f) + (rank * 0.2f);
            
            ProjectileTowerBlueprint.DamageMult = Random.Range(0.9f, 1.1f) * (1f + (rank * 0.15f));
            ProjectileTowerBlueprint.ProjectileFragile = true;
            LoadBasicShot();
            ProjectileTowerBlueprint.ProjectileBehaviors = null;
            ProjectileTowerBlueprint.ProjectileEffects = null;
            ProjectileTowerBlueprint.SecondaryShots = null;
            ProjectileTowerBlueprint.StatusEffects = null;
            ProjectileTowerBlueprint.TowerBehaviours = null;
            ProjectileTowerBlueprint.ProjectileScale = 1;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (ProjectileTowerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(ProjectileTowerBlueprint);
        }
    }
}