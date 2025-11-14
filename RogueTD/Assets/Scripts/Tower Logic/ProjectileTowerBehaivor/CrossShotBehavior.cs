using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CrossShotBehavior", menuName = "Tower Defense/ShotBehavior/Cross Shot Behavior")]
public class CrossShotBehavior : SecondaryProjectileTowerBehavior
{
    public override void Shoot(ProjectileTower tower, ProjectileTower.ShotData shotData, Action<ProjectileTower.ShotData> nextBehavior)
    {
        for (int i = 0; i < 4; i++)
        {
            var rotatedShotData = new ProjectileTower.ShotData
            {
                ProjectileCount = shotData.ProjectileCount,
                Spread = shotData.Spread,
                ProjectileSpeed = shotData.ProjectileSpeed,
                Rotation = shotData.Rotation * Quaternion.Euler(0, 0, i * 90f), // 0°, 90°, 180°, 270°
                CreateProjectileFunc = shotData.CreateProjectileFunc
            };
            
            nextBehavior?.Invoke(rotatedShotData);
        }
    }
}