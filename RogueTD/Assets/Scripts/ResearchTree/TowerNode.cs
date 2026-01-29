using UnityEngine;

public abstract class TowerNode<TBlueprint> : BuildingNode<TBlueprint> where TBlueprint : TowerBlueprint, new()
{
    public TowerBlueprint TowerBlueprint
    {
        get => (TowerBlueprint) _buildingBlueprint;
        set => _buildingBlueprint = value;
    }

    public TowerNode(float rankMultiplier, TowerNodeConfig towerConfig) : base(rankMultiplier, towerConfig)
    {
        Debug.Log(_buildingBlueprint.GetType());
        TowerBlueprint.TowerPrefab = towerConfig.Tower;
        TowerBlueprint.TargetingRange = towerConfig.TargetingRange * rankMultiplier;
        TowerBlueprint.DamageMult = towerConfig.DamageMult;
        TowerBlueprint.AttackSpeed = towerConfig.AttackSpeed * rankMultiplier;
        TowerBlueprint.RotatingSpeed = towerConfig.RotatingSpeed * rankMultiplier;
        TowerBlueprint.Damage = (int)(towerConfig.Damage * rankMultiplier);
        TowerBlueprint.MaxAmmo = (int)(towerConfig.MaxAmmo * rankMultiplier);
        TowerBlueprint.CurrentAmmo = TowerBlueprint.MaxAmmo;
        TowerBlueprint.AmmoRegeneration = towerConfig.AmmoRegeneration;
        TowerBlueprint.AttackAngle = towerConfig.AttackAngle;
    }
}