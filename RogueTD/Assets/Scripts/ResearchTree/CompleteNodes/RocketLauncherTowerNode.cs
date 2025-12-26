

using UnityEngine;

[CreateAssetMenu(fileName = "NodeRocketLauncherTurret", menuName = "Research Tree/Turrets/Rocket Launcher Turret Node")]
public class NodeRocketLauncherTurret : ProjectileTowerNode
{
    [SerializeField] private ExplosionEffect explosionEffect;
    
    public override string TooltipText => "Rocket launcher with explosions.";
    
    public override string GetStats(int rank)
    {
        if (towerBlueprint)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Rocket Launcher (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        if (towerBlueprint)
        {
            float rankMultiplier = 1f + (rank * 0.2f);
            
            towerBlueprint.Damage = Mathf.RoundToInt(Random.Range(10f, 15f) * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(0.5f, 1.2f) * (1f + (rank * 0.05f));
            towerBlueprint.TargetingRange = Random.Range(6f, 9f) * (1f + (rank * 0.1f));
            towerBlueprint.RotatingSpeed = Random.Range(70f, 100f) + (rank * 8f);
            
            towerBlueprint.ProjectileSpeed = Random.Range(20f, 30f) + (rank * 1f);
            towerBlueprint.ProjectileLifetime = Random.Range(2f, 3f) + (rank * 0.2f);
            towerBlueprint.Spread = Random.Range(0f, 3f);
            towerBlueprint.ProjectileFragile = Random.value > 0.3f;
            
            towerBlueprint.ProjectileCount = 1;
            towerBlueprint.ProjectileScale = Random.Range(1.2f, 1.8f);
            
            towerBlueprint.MaxAmmo = Random.Range(12, 20) + (rank * 2);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(0.8f, 1.5f) + (rank * 0.2f);
            
            towerBlueprint.DamageMult = Random.Range(1.2f, 1.5f) + (rank * 0.08f);
            
            LoadBasicShot();
            
            if (explosionEffect)
            {
                towerBlueprint.ProjectileEffects = new ResourceReference<ProjectileEffect>[]
                {
                    new ResourceReference<ProjectileEffect> { Value = explosionEffect }
                };
            }
            
            towerBlueprint.ProjectileBehaviors = null;
            towerBlueprint.SecondaryShots = null;
            towerBlueprint.StatusEffects = null;
            towerBlueprint.TowerBehaviours = null;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (towerBlueprint)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(towerBlueprint);
        }
        if (explosionEffect)
        {
            ResourceManager.RegisterProjectileEffect(explosionEffect.name, explosionEffect);
        }
    }
}
    