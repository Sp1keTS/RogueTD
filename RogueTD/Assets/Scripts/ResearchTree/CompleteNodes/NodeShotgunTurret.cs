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
        if (towerBlueprint)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Stats (Rank {rank}):</b>\n" +
                   $"{towerBlueprint.GetTowerStats()}\n\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        if (towerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.15f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(Random.Range(6f, 10f) * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(0.8f, 1.5f) * (1f + (rank * 0.1f));
            towerBlueprint.TargetingRange = Random.Range(3f, 5f) * (1f + (rank * 0.1f));
            towerBlueprint.RotatingSpeed = Random.Range(100f, 150f) + (rank * 10f);
            towerBlueprint.ProjectileScale = 1;
            towerBlueprint.ProjectileSpeed = Random.Range(30f, 40f) + (rank * 1f);
            towerBlueprint.ProjectileLifetime = Random.Range(0.25f, 0.5f) + (rank * 0.1f);
            towerBlueprint.Spread = Random.Range(10f, 20f);
            towerBlueprint.ProjectileFragile = Random.value > 0.5f;
            
            towerBlueprint.ProjectileCount = Random.Range(4, 8) + Mathf.RoundToInt(rank * 0.5f);
            
            towerBlueprint.MaxAmmo = Random.Range(15, 25) + (rank * 3);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(1f, 2f) + (rank * 0.2f);
            
            towerBlueprint.DamageMult = Random.Range(0.7f, 0.9f) + (rank * 0.05f);
            LoadBasicShot();
            
            towerBlueprint.ProjectileBehaviors = null;
            towerBlueprint.ProjectileEffects = null;
            towerBlueprint.SecondaryShots = null;
            towerBlueprint.StatusEffects = null;
            towerBlueprint.TowerBehaviours = null;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (towerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(towerBlueprint);
        }
    }
}