using UnityEngine;

[CreateAssetMenu(fileName = "BasicTurret", menuName = "Research Tree/Turrets/Basic Turret node")]
public class NodeBasicTurret : ProjectileTowerNode
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Balanced all-rounder tower.";
    
    public override string TooltipText => description;
    
    public override string GetStats(int rank)
    {
        if (_ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Stats (Rank {rank}):</b>\n" +
                   $"{_ProjectileTowerBlueprint.GetTowerStats()}";
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
            _ProjectileTowerBlueprint = new ProjectileTowerBlueprint();
            var rankMultiplier = 1f + (rank * 0.2f);
            
            _ProjectileTowerBlueprint.Damage = Mathf.RoundToInt(5 * rankMultiplier);
            _ProjectileTowerBlueprint.AttackSpeed = Random.Range(0.8f, 1.2f) * (1f + (rank * 0.1f));
            _ProjectileTowerBlueprint.TargetingRange = Random.Range(4.5f, 5.5f) + (rank * 0.5f);
            _ProjectileTowerBlueprint.RotatingSpeed = Random.Range(90f, 110f) + (rank * 10f);
            
            _ProjectileTowerBlueprint.ProjectileSpeed = Random.Range(13f, 17f) + (rank * 1f);
            _ProjectileTowerBlueprint.ProjectileLifetime = Random.Range(1.8f, 2.2f) + (rank * 0.1f);
            _ProjectileTowerBlueprint.Spread = Random.Range(0f, 2f);
            _ProjectileTowerBlueprint.ProjectileFragile = Random.value > 0.7f;
            
            _ProjectileTowerBlueprint.ProjectileCount = 1;
            
            _ProjectileTowerBlueprint.MaxAmmo = Random.Range(25, 35) + (rank * 5);
            _ProjectileTowerBlueprint.CurrentAmmo = _ProjectileTowerBlueprint.MaxAmmo;
            _ProjectileTowerBlueprint.AmmoRegeneration = Random.Range(0.8f, 1.2f) + (rank * 0.2f);
            
            _ProjectileTowerBlueprint.DamageMult = Random.Range(0.9f, 1.1f) * (1f + (rank * 0.15f));
            _ProjectileTowerBlueprint.ProjectileFragile = true;
            LoadBasicShot();
            _ProjectileTowerBlueprint.ProjectileBehaviors = null;
            _ProjectileTowerBlueprint.ProjectileEffects = null;
            _ProjectileTowerBlueprint.SecondaryShots = null;
            _ProjectileTowerBlueprint.StatusEffects = null;
            _ProjectileTowerBlueprint.TowerBehaviours = null;
            _ProjectileTowerBlueprint.ProjectileScale = 1;
        }
    }
    
    public override void LoadDependencies()
    {
        LoadBasicShot();
        if (_ProjectileTowerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(_ProjectileTowerBlueprint);
        }
    }
}