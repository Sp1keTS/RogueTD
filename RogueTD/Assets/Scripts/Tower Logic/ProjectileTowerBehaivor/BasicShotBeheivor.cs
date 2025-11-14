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
            if (projectile != null)
            {
                Vector2 towerDirection = shotData.Rotation * Vector2.right;
                float randomAngle = Random.Range(-shotData.Spread, shotData.Spread);
                Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * towerDirection;
                Rigidbody2D rb = projectile.rb;
                rb.AddForce(direction * shotData.ProjectileSpeed, ForceMode2D.Impulse);
                    
                float projectileAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(projectileAngle, Vector3.forward);
            }
        }
    }
}