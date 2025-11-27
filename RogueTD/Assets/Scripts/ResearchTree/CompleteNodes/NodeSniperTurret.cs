using UnityEngine;

[CreateAssetMenu(fileName = "NodeSniperTurret", menuName = "Research Tree/Turrets/Sniper Turret Node")]
public class NodeSniperTurret : ProjectileTowerNode
{
    [SerializeField] private BasicShotBehavior basicShotBehavior;
    
    public override void Initialize(int rank)
    {
        if (towerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.25f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(Random.Range(20f, 30f) * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(0.3f, 0.7f) * (1f + (rank * 0.05f));
            towerBlueprint.TargetingRange = Random.Range(8f, 12f) * (1f + (rank * 0.15f));
            towerBlueprint.RotatingSpeed = Random.Range(60f, 90f) + (rank * 8f);
            
            towerBlueprint.ProjectileSpeed = Random.Range(20f, 30f) + (rank * 2f);
            towerBlueprint.ProjectileLifetime = Random.Range(2.5f, 4f) + (rank * 0.2f);
            towerBlueprint.Spread = Random.Range(0f, 1f); // Минимальный разброс
            towerBlueprint.ProjectileFragile = false; 
            
            towerBlueprint.ProjectileCount = 1;
            
            towerBlueprint.MaxAmmo = Random.Range(8, 15) + (rank * 2);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(0.3f, 0.7f) + (rank * 0.1f);
            
            towerBlueprint.DamageMult = Random.Range(1.3f, 1.8f) + (rank * 0.1f);
            
            if (basicShotBehavior != null)
            {
                towerBlueprint.ShotBehavior = new ResourceReference<ProjectileTowerBehavior> 
                { 
                    Value = basicShotBehavior 
                };
                ResourceManager.RegisterTowerBehavior(basicShotBehavior.name, basicShotBehavior);
            }
            
            towerBlueprint.ProjectileBehaviors = null;
            towerBlueprint.ProjectileEffects = null;
            towerBlueprint.SecondaryShots = null;
            towerBlueprint.StatusEffects = null;
            towerBlueprint.TowerBehaviours = null;
            
        }
    }
}