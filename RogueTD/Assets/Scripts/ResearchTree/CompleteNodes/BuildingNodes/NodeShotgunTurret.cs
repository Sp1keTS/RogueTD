using UnityEngine;

[CreateAssetMenu(fileName = "NodeShotgunTurret", menuName = "Research Tree/Turrets/Shotgun Turret Node")]
public class NodeShotgunTurret : ProjectileTowerNode
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
    
    public override void Initialize(int rank)
    {
        ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
        ProjectileTowerBlueprint.Initialize(buildingName, ProjectileTower, buildingPrefab, maxHealthPoints, buildingCost, size);
        
        if (ProjectileTowerBlueprint != null)
        {
            var rankMultiplier = 1f + (rank * 0.15f);
            
            ProjectileTowerBlueprint.Damage = Mathf.RoundToInt(Random.Range(6f, 10f) * rankMultiplier);
            ProjectileTowerBlueprint.AttackSpeed = Random.Range(0.8f, 1.5f) * (1f + (rank * 0.1f));
            ProjectileTowerBlueprint.TargetingRange = Random.Range(3f, 5f) * (1f + (rank * 0.1f));
            ProjectileTowerBlueprint.RotatingSpeed = Random.Range(100f, 150f) + (rank * 10f);
            ProjectileTowerBlueprint.ProjectileScale = 1;
            ProjectileTowerBlueprint.ProjectileSpeed = Random.Range(30f, 40f) + (rank * 1f);
            ProjectileTowerBlueprint.ProjectileLifetime = Random.Range(0.25f, 0.5f) + (rank * 0.1f);
            ProjectileTowerBlueprint.Spread = Random.Range(10f, 20f);
            ProjectileTowerBlueprint.ProjectileFragile = Random.value > 0.5f;
            
            ProjectileTowerBlueprint.ProjectileCount = Random.Range(4, 8) + Mathf.RoundToInt(rank * 0.5f);
            
            ProjectileTowerBlueprint.MaxAmmo = Random.Range(15, 25) + (rank * 3);
            ProjectileTowerBlueprint.CurrentAmmo = ProjectileTowerBlueprint.MaxAmmo;
            ProjectileTowerBlueprint.AmmoRegeneration = Random.Range(1f, 2f) + (rank * 0.2f);
            
            ProjectileTowerBlueprint.DamageMult = Random.Range(0.7f, 0.9f) + (rank * 0.05f);
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