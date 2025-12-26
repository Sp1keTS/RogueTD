using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "StraightMovement", menuName = "Tower Defense/ShotBehavior/Basic Shot Behavior")]
public class BasicShotBehavior : ProjectileTowerBehavior
{
    public override void Shoot(ProjectileTower tower,ProjectileTower.ShotData shotData, Action<ProjectileTower.ShotData> nextBehavior = null)
    {
        for (int proj = 0; proj < shotData.ProjectileCount; proj++)
        {
            TowerProjectile projectile = shotData.CreateProjectileFunc?.Invoke(tower.BarrelPosition);
            if (projectile)
            {
                var towerDirection = shotData.Rotation * Vector2.right;
                var randomAngle = Random.Range(-shotData.Spread, shotData.Spread);
                var direction = Quaternion.Euler(0, 0, randomAngle) * towerDirection;
                var rb = projectile.rb;
                rb.AddForce(direction * shotData.ProjectileSpeed, ForceMode2D.Impulse);
                    
                var projectileAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(projectileAngle, Vector3.forward);
            }
        }
    }
}