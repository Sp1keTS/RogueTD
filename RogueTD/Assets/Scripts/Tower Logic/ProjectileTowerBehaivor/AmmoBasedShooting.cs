using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AmmoBasedShooting", menuName = "Tower Defense/SecondaryShot/Ammo Based Shooting")]
public class AmmoBasedShootBehavior : SecondaryProjectileTowerBehavior
{
    [System.Serializable]
    private class TowerAmmoState
    {
        public bool requireFullAmmo = false;
        public bool canShoot = true;
    }
    
    private Dictionary<ProjectileTower, TowerAmmoState> towerStates = new Dictionary<ProjectileTower, TowerAmmoState>();
    
    [SerializeField] private bool defaultRequireFullAmmo = false;
    
    public override void Shoot(ProjectileTower tower, ProjectileTower.ShotData shotData, Action<ProjectileTower.ShotData> nextBehavior)
    {
        if (!towerStates.ContainsKey(tower))
        {
            towerStates[tower] = new TowerAmmoState 
            { 
                requireFullAmmo = defaultRequireFullAmmo,
                canShoot = true
            };
        }
        
        var state = towerStates[tower];
        
        if (state.canShoot && HasEnoughAmmo(tower, state))
        {
            ConsumeAmmo(tower, state);
            nextBehavior?.Invoke(shotData);
        }
    }
    
    private bool HasEnoughAmmo(ProjectileTower tower, TowerAmmoState state)
    {
        if (state.requireFullAmmo)
        {
            return Mathf.Approximately(tower.CurrentAmmo, tower.MaxAmmo);
        }
        else
        {
            return tower.CurrentAmmo >= 1f;
        }
    }
    
    private void ConsumeAmmo(ProjectileTower tower, TowerAmmoState state)
    {
        if (state.requireFullAmmo)
        {
            tower.CurrentAmmo = 0f;
        }
        else
        {
            tower.CurrentAmmo = Mathf.Max(0f, tower.CurrentAmmo - 1f);
        }
    }
    
    public void SetCanShoot(ProjectileTower tower, bool canShoot)
    {
        if (!towerStates.ContainsKey(tower))
        {
            towerStates[tower] = new TowerAmmoState();
        }
        
        towerStates[tower].canShoot = canShoot;
    }
    
    public void SetRequireFullAmmo(ProjectileTower tower, bool requireFull)
    {
        if (!towerStates.ContainsKey(tower))
        {
            towerStates[tower] = new TowerAmmoState();
        }
        
        towerStates[tower].requireFullAmmo = requireFull;
    }
    
    public void CleanupTowerState(ProjectileTower tower)
    {
        if (towerStates.ContainsKey(tower))
        {
            towerStates.Remove(tower);
        }
    }
}