using UnityEngine;

[CreateAssetMenu(fileName = "NodeRapidFireTurret", menuName = "Research Tree/Upgrades/Rapid Fire Turret Node")]
public class NodeRapidFireTurret : ProjectileTowerNode
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "High fire rate, low damage.";
    
    [Header("Magazine Settings")]
    [SerializeField] private AmmoBasedShootingBehavior ammoBehavior;
    [SerializeField] private float magazineSizeMultiplier = 1.5f;
    [SerializeField] private float reloadSpeedMultiplier = 1.3f;
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        if (_ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Stats (Rank {rank}):</b>\n" +
                   $"{_ProjectileTowerBlueprint.GetTowerStats()}\n\n" +
                   $"<b>Special:</b>\n" +
                   $"• Magazine system\n" +
                   $"• Fires until empty\n" +
                   $"• Pauses to reload";
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
            var rankMultiplier = 1f + (rank * 0.12f);
            
            _ProjectileTowerBlueprint.Damage = Mathf.RoundToInt(Random.Range(1f, 2f) * rankMultiplier);
            _ProjectileTowerBlueprint.AttackSpeed = Random.Range(3f, 7f) * (1f + (rank * 0.2f)); 
            _ProjectileTowerBlueprint.TargetingRange = Random.Range(4f, 7f) * (1f + (rank * 0.1f));
            _ProjectileTowerBlueprint.RotatingSpeed = Random.Range(130f, 180f) + (rank * 15f);
            
            _ProjectileTowerBlueprint.ProjectileSpeed = Random.Range(20f, 30f) + (rank * 1.5f);
            _ProjectileTowerBlueprint.ProjectileLifetime = Random.Range(1.5f, 2.5f) + (rank * 0.1f);
            _ProjectileTowerBlueprint.Spread = Random.Range(3f, 8f);
            _ProjectileTowerBlueprint.ProjectileFragile = true;
            _ProjectileTowerBlueprint.ProjectileScale = 1;
            _ProjectileTowerBlueprint.ProjectileCount = 1;
            
            _ProjectileTowerBlueprint.MaxAmmo = Mathf.RoundToInt(Random.Range(40, 60) * magazineSizeMultiplier) + (rank * 8);
            _ProjectileTowerBlueprint.CurrentAmmo = _ProjectileTowerBlueprint.MaxAmmo;
            _ProjectileTowerBlueprint.AmmoRegeneration = Random.Range(2f, 3f) * reloadSpeedMultiplier + (rank * 0.3f);
            
            _ProjectileTowerBlueprint.DamageMult = Random.Range(0.6f, 0.8f) + (rank * 0.04f);
            
            LoadBasicShot();
            
            if (ammoBehavior)
            {
                _ProjectileTowerBlueprint.SecondaryShots = new ResourceReference<SecondaryProjectileTowerBehavior>[]
                {
                    new ResourceReference<SecondaryProjectileTowerBehavior> { Value = ammoBehavior }
                };
            }
            
            _ProjectileTowerBlueprint.ProjectileBehaviors = null;
            _ProjectileTowerBlueprint.ProjectileEffects = null;
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
        if (ammoBehavior)
        {
            ResourceManager.RegisterSecondaryBehavior(ammoBehavior.name, ammoBehavior);
        }
    }
}