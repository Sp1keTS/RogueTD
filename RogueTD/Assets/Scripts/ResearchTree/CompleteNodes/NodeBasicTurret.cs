using UnityEngine;

[CreateAssetMenu(fileName = "BasicTurret", menuName = "Research Tree/Turrets/Basic Turret node")]
public class NodeBasicTurret : ProjectileTowerNode
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "The standard projectile tower with balanced stats.\n" +
        "Versatile and reliable, this tower excels in most situations.\n" +
        "Good damage, range, and fire rate make it an excellent all-rounder.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        if (towerBlueprint != null)
        {
            return $"Cost: {Cost}\n" +
                   $"{description}\n\n" +
                   $"Tower Stats (Rank {rank}):\n" +
                   $"{towerBlueprint.GetTowerStats()}";
        }
        return $"FAILED TO LOAD\n\n{description}";
    }
    
    public override void Initialize(int rank)
    {
        if (towerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.2f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(5 * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(0.8f, 1.2f) * (1f + (rank * 0.1f));
            towerBlueprint.TargetingRange = Random.Range(4.5f, 5.5f) + (rank * 0.5f);
            towerBlueprint.RotatingSpeed = Random.Range(90f, 110f) + (rank * 10f);
            
            towerBlueprint.ProjectileSpeed = Random.Range(13f, 17f) + (rank * 1f);
            towerBlueprint.ProjectileLifetime = Random.Range(1.8f, 2.2f) + (rank * 0.1f);
            towerBlueprint.Spread = Random.Range(0f, 2f);
            towerBlueprint.ProjectileFragile = Random.value > 0.7f;
            
            towerBlueprint.ProjectileCount = 1;
            
            towerBlueprint.MaxAmmo = Random.Range(25, 35) + (rank * 5);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(0.8f, 1.2f) + (rank * 0.2f);
            
            towerBlueprint.DamageMult = Random.Range(0.9f, 1.1f) * (1f + (rank * 0.15f));
            towerBlueprint.ProjectileFragile = true;
            LoadBasicShot();
            towerBlueprint.ProjectileBehaviors = null;
            towerBlueprint.ProjectileEffects = null;
            towerBlueprint.SecondaryShots = null;
            towerBlueprint.StatusEffects = null;
            towerBlueprint.TowerBehaviours = null;
            towerBlueprint.ProjectileScale = 1;
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