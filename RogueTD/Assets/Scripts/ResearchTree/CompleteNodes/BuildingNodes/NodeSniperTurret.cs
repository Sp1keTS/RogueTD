using UnityEngine;

[CreateAssetMenu(fileName = "NodeSniperTurret", menuName = "Research Tree/Turrets/Sniper Turret Node")]
public class NodeSniperTurret : ProjectileTowerNode
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
    
    public override void Initialize(int rank)
    {
        ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
        ProjectileTowerBlueprint.Initialize(buildingName, ProjectileTower, buildingPrefab, maxHealthPoints, buildingCost, size);
        
        if (ProjectileTowerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.25f);
            
            ProjectileTowerBlueprint.Damage = Mathf.RoundToInt(Random.Range(20f, 30f) * rankMultiplier);
            ProjectileTowerBlueprint.AttackSpeed = Random.Range(0.3f, 0.7f) * (1f + (rank * 0.05f));
            ProjectileTowerBlueprint.TargetingRange = Random.Range(8f, 12f) * (1f + (rank * 0.15f));
            ProjectileTowerBlueprint.RotatingSpeed = Random.Range(60f, 90f) + (rank * 8f);
            ProjectileTowerBlueprint.ProjectileScale = 1;
            ProjectileTowerBlueprint.ProjectileSpeed = Random.Range(50f, 100f) + (rank * 2f);
            ProjectileTowerBlueprint.ProjectileLifetime = Random.Range(2.5f, 4f) + (rank * 0.2f);
            ProjectileTowerBlueprint.Spread = Random.Range(0f, 1f);
            ProjectileTowerBlueprint.ProjectileFragile = false;
            
            ProjectileTowerBlueprint.ProjectileCount = 1;
            
            ProjectileTowerBlueprint.MaxAmmo = Random.Range(8, 15) + (rank * 2);
            ProjectileTowerBlueprint.CurrentAmmo = ProjectileTowerBlueprint.MaxAmmo;
            ProjectileTowerBlueprint.AmmoRegeneration = Random.Range(0.3f, 0.7f) + (rank * 0.1f);
            
            ProjectileTowerBlueprint.DamageMult = Random.Range(1.3f, 1.8f) + (rank * 0.1f);
            
            LoadBasicShot();
            
            ProjectileTowerBlueprint.ProjectileBehaviors = null;
            ProjectileTowerBlueprint.ProjectileEffects = null;
            ProjectileTowerBlueprint.SecondaryShots = null;
            ProjectileTowerBlueprint.StatusEffects = null;
            ProjectileTowerBlueprint.TowerBehaviours = null;
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