using UnityEngine;
using Random = UnityEngine.Random;

public class BasicShotBehavior : ProjectileTowerBehavior
{
    public override void Shoot(ProjectileTower projectileTower, ProjectileTower.ShotData shotData)
    {
        for (int proj = 0; proj < shotData.ProjectileCount; proj++)
        {
            // Используем делегат для создания снаряда
            TowerProjectile projectile = shotData.CreateProjectileFunc?.Invoke();
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