using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RampingFireRate", menuName = "Tower Defense/SecondaryShot/Ramping Fire Rate")]
public class RampingFireRateBehavior : SecondaryProjectileTowerBehavior
{
    private class TowerRampState
    {
        public float currentMultiplier = 1f;
        public float timeSinceLastShot = 0f;
    }
    
    private Dictionary<ProjectileTower, TowerRampState> towerStates = new Dictionary<ProjectileTower, TowerRampState>();
    
    [SerializeField] private float maxMultiplier = 2f;
    [SerializeField] private float rampUpPerShot = 0.1f;
    [SerializeField] private float cooldownPerSecond = 0.1f;
    [SerializeField] private float resetDelay = 2f;
    
    public override void Shoot(ProjectileTower tower, ProjectileTower.ShotData shotData, Action<ProjectileTower.ShotData> nextBehavior)
    {
        if (!towerStates.ContainsKey(tower))
        {
            towerStates[tower] = new TowerRampState();
        }
        
        var state = towerStates[tower];
        
        UpdateRampState(tower, state);
        
        var modifiedShotData = new ProjectileTower.ShotData
        {
            ProjectileCount = shotData.ProjectileCount,
            Spread = shotData.Spread,
            ProjectileSpeed = shotData.ProjectileSpeed * state.currentMultiplier,
            Rotation = shotData.Rotation,
            CreateProjectileFunc = shotData.CreateProjectileFunc
        };
        
        nextBehavior?.Invoke(modifiedShotData);
        
        state.currentMultiplier = Mathf.Min(state.currentMultiplier + rampUpPerShot, maxMultiplier);
        state.timeSinceLastShot = 0f;
    }
    
    private void UpdateRampState(ProjectileTower tower, TowerRampState state)
    {
        state.timeSinceLastShot += Time.deltaTime;
        
        if (state.timeSinceLastShot > resetDelay)
        {
            state.currentMultiplier = Mathf.MoveTowards(state.currentMultiplier, 1f, Time.deltaTime * cooldownPerSecond * 2f);
        }
        else
        {
            state.currentMultiplier = Mathf.MoveTowards(state.currentMultiplier, 1f, Time.deltaTime * cooldownPerSecond);
        }
    }
    
    public void CleanupTowerState(ProjectileTower tower)
    {
        if (towerStates.ContainsKey(tower))
        {
            towerStates.Remove(tower);
        }
    }
}