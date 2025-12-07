using UnityEngine;
[CreateAssetMenu(fileName = "NodeShotgunTurret", menuName = "Research Tree/Turrets/Shotgun Turret Node")]
public class NodeShotgunTurret : ProjectileTowerNode
{
    public override void Initialize(int rank)
    {
        if (towerBlueprint != null)
        {
            float rankMultiplier = 1f + (rank * 0.15f);
            
            // Случайные характеристики для дробовика
            towerBlueprint.Damage = Mathf.RoundToInt(Random.Range(6f, 10f) * rankMultiplier);
            towerBlueprint.AttackSpeed = Random.Range(0.8f, 1.5f) * (1f + (rank * 0.1f));
            towerBlueprint.TargetingRange = Random.Range(3f, 5f) * (1f + (rank * 0.1f));
            towerBlueprint.RotatingSpeed = Random.Range(100f, 150f) + (rank * 10f);
            
            towerBlueprint.ProjectileSpeed = Random.Range(30f, 40f) + (rank * 1f);
            towerBlueprint.ProjectileLifetime = Random.Range(0.25f, 0.5f) + (rank * 0.1f);
            towerBlueprint.Spread = Random.Range(10f, 20f); // Большой разброс
            towerBlueprint.ProjectileFragile = Random.value > 0.5f;
            
            // Много снарядов
            towerBlueprint.ProjectileCount = Random.Range(4, 8) + Mathf.RoundToInt(rank * 0.5f);
            
            towerBlueprint.MaxAmmo = Random.Range(15, 25) + (rank * 3);
            towerBlueprint.CurrentAmmo = towerBlueprint.MaxAmmo;
            towerBlueprint.AmmoRegeneration = Random.Range(1f, 2f) + (rank * 0.2f);
            
            towerBlueprint.DamageMult = Random.Range(0.7f, 0.9f) + (rank * 0.05f);
            LoadBasicShot();
            // Очищаем остальные поведения
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