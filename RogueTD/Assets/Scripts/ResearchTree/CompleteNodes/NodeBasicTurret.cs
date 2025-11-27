using UnityEngine;

[CreateAssetMenu(fileName = "BasicTurret", menuName = "Research Tree/Turrets/Basic Turret node")]
public class NodeBasicTurret : ProjectileTowerNode
{
    [SerializeField] private BasicShotBehavior basicShotBehavior;
    
    public override void Initialize(int rank)
    {
        if (towerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.2f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(5 * rankMultiplier);
            towerBlueprint.AttackSpeed = 1f + (rank * 0.1f);
            towerBlueprint.TargetingRange = 5f + (rank * 0.5f);
            towerBlueprint.RotatingSpeed = 100f + (rank * 10f);
            
            towerBlueprint.ProjectileSpeed = 15f + (rank * 1f);
            towerBlueprint.ProjectileLifetime = 2f + (rank * 0.1f);
            towerBlueprint.Spread = Random.Range(0f, 2f);
            towerBlueprint.ProjectileFragile = Random.value > 0.7f;
            
            towerBlueprint.ProjectileCount = 1;
            
            towerBlueprint.MaxAmmo = 30 + (rank * 5);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = 1f + (rank * 0.2f);
            
            towerBlueprint.DamageMult = 1f + (rank * 0.15f);
            towerBlueprint.ProjectileFragile = true;
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