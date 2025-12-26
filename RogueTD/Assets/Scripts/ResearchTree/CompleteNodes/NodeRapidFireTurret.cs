using UnityEngine;

[CreateAssetMenu(fileName = "NodeRapidFireTurret", menuName = "Research Tree/Upgrades/Rapid Fire Turret Node")]
public class NodeRapidFireTurret : ProjectileTowerNode
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "High fire rate, low damage.";
    
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
            float rankMultiplier = 1f + (rank * 0.12f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(Random.Range(1f, 2f) * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(10f, 20f) * (1f + (rank * 0.2f)); 
            towerBlueprint.TargetingRange = Random.Range(4f, 7f) * (1f + (rank * 0.1f));
            towerBlueprint.RotatingSpeed = Random.Range(130f, 180f) + (rank * 15f);
            
            towerBlueprint.ProjectileSpeed = Random.Range(20f, 30f) + (rank * 1.5f);
            towerBlueprint.ProjectileLifetime = Random.Range(1.5f, 2.5f) + (rank * 0.1f);
            towerBlueprint.Spread = Random.Range(3f, 8f);
            towerBlueprint.ProjectileFragile = Random.value > 0.7f;
            towerBlueprint.ProjectileScale = 1;
            towerBlueprint.ProjectileCount = 1;
            
            towerBlueprint.MaxAmmo = Random.Range(40, 60) + (rank * 6);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(2f, 3f) + (rank * 0.3f);
            
            towerBlueprint.DamageMult = Random.Range(0.6f, 0.8f) + (rank * 0.04f);
            
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