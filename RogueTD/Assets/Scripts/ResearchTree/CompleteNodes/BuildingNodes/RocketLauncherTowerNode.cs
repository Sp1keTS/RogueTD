

using UnityEngine;

[CreateAssetMenu(fileName = "NodeRocketLauncherTurret", menuName = "Research Tree/Turrets/Rocket Launcher Turret Node")]
public class NodeRocketLauncherTurret : ProjectileTowerNode
{
    [SerializeField] private ExplosionEffect explosionEffect;
    
    public override string TooltipText => "Rocket launcher with explosions.";
    
    public override string GetStats(int rank)
    {
        if (_ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Rocket Launcher (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        _ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
        _ProjectileTowerBlueprint.Initialize(buildingName, ProjectileTower, buildingPrefab, maxHealthPoints, buildingCost, size);
        
        if (_ProjectileTowerBlueprint != null)
        {
            var rankMultiplier = 1f + (rank * 0.2f);
            
            _ProjectileTowerBlueprint.Damage = Mathf.RoundToInt(Random.Range(10f, 15f) * rankMultiplier);
            _ProjectileTowerBlueprint.AttackSpeed = Random.Range(0.5f, 1.2f) * (1f + (rank * 0.05f));
            _ProjectileTowerBlueprint.TargetingRange = Random.Range(6f, 9f) * (1f + (rank * 0.1f));
            _ProjectileTowerBlueprint.RotatingSpeed = Random.Range(70f, 100f) + (rank * 8f);
            
            _ProjectileTowerBlueprint.ProjectileSpeed = Random.Range(20f, 30f) + (rank * 1f);
            _ProjectileTowerBlueprint.ProjectileLifetime = Random.Range(2f, 3f) + (rank * 0.2f);
            _ProjectileTowerBlueprint.Spread = Random.Range(0f, 3f);
            _ProjectileTowerBlueprint.ProjectileFragile = true;
            
            _ProjectileTowerBlueprint.ProjectileCount = 1;
            _ProjectileTowerBlueprint.ProjectileScale = Random.Range(1.2f, 1.8f);
            
            _ProjectileTowerBlueprint.MaxAmmo = Random.Range(12, 20) + (rank * 2);
            _ProjectileTowerBlueprint.CurrentAmmo = _ProjectileTowerBlueprint.MaxAmmo;
            _ProjectileTowerBlueprint.AmmoRegeneration = Random.Range(0.8f, 1.5f) + (rank * 0.2f);
            
            _ProjectileTowerBlueprint.DamageMult = Random.Range(1.2f, 1.5f) + (rank * 0.08f);
            
            LoadBasicShot();
            
            if (explosionEffect)
            {
                _ProjectileTowerBlueprint.ProjectileEffects = new ResourceReference<ProjectileEffect>[]
                {
                    new ResourceReference<ProjectileEffect> { Value = explosionEffect }
                };
            }
            
            _ProjectileTowerBlueprint.ProjectileBehaviors = null;
            _ProjectileTowerBlueprint.SecondaryShots = null;
            _ProjectileTowerBlueprint.StatusEffects = null;
            _ProjectileTowerBlueprint.TowerBehaviours = null;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (_ProjectileTowerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(_ProjectileTowerBlueprint);
        }
        if (explosionEffect)
        {
            ResourceManager.RegisterProjectileEffect(explosionEffect.name, explosionEffect);
        }
    }
}
    